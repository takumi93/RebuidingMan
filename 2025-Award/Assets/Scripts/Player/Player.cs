using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //プレイヤーの移動速度を指定します
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int attackPower = 50;
    //攻撃が当たった時のエフェクトを指定します。
    [SerializeField]
    GameObject hitEffect = null;
    //HitReticleを指定します。
    [SerializeField]
    HitReticle hitReticle = null;

    PartHp partHp;

    RobotCreate robotCreate;

    Vector2 moveInput;
    //事前に宣言しておく変数
    new Rigidbody rigidbody;

    // ロボットの作成地点を指定
    [SerializeField]public Transform plantPoint;

    AudioSource audioSource;

    [SerializeField] AudioClip SwingSound = null;
    [SerializeField] AudioClip GrabSound = null;
    [SerializeField] AudioClip AttackSound = null;
    [SerializeField] AudioClip CreateSound = null;

    // Hitに格納しているオブジェクトのレイヤーを格納する変数を宣言
    private int hitLayer;

    private PlayerInventory inventory;

    // 新規記入
    [SerializeField] Image Reticle = null;
    PlayerStateManager StateManager;

    Ray ray;

    // Rayが当たったオブジェクトを格納する変数を宣言
    public RaycastHit hit;

    // 拾える距離
    [SerializeField] private float InteractDistance = 10.0f;
    // アイテムLayerの指定
    [SerializeField] private LayerMask itemLayer;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private LayerMask playerLayer;

    public bool IsEnemy { get; private set; }

    public PlayerAnimation Animation { get; set; }

    public RobotHPManager hpManager { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();

        Animation = GetComponentInChildren<PlayerAnimation>(true);
        robotCreate = GetComponent<RobotCreate>();
    }

    private void Start()
    {
        StateManager = new PlayerStateManager(this);
    }

    /// <summary>
    /// PlayerControllerに送るUpdateの処理
    /// </summary>
    /// <param name="inputInfo">入力情報</param>
    public void Tick(InputInfo inputInfo)
    {
        // ポーズ中は入力処理を受け付けない
        if (StageScene.Instance.optionState == StageScene.OptionState.Pause)
        {
            return;
        }
        StateManager.CurrentState.Tick(this, inputInfo);
        Look(inputInfo);
        SetUpViewPoint();
        RecognitionEnemy();
    }

    /// <summary>
    /// ラッパー関数として登録
    /// プレイヤーの状態遷移をする処理
    /// </summary>
    /// <param name="newstate"></param>
    public void ChangeState(IPlayerState newstate)
    {
        StateManager.ChangeState(newstate);
    }

    /// <summary>
    /// プレイヤーの移動
    /// カメラの移動はCinemachineを使用しているため不要
    /// </summary>
    /// <param name="inputInfo"></param>
    public void Move(InputInfo inputInfo)
    {
        Vector3 move = transform.TransformDirection(inputInfo.Move);

        // 等速度運動
        var velocity = rigidbody.linearVelocity;
        velocity.x = move.x * moveSpeed;
        velocity.z = move.z * moveSpeed;
        rigidbody.linearVelocity = velocity;
    }

    /// <summary>
    /// プレイヤーのカメラの中心にあるオブジェクトの情報を取得する
    /// </summary>
    public void SetUpViewPoint()
    {
        // Rayを飛ばす位置を指定（メインカメラを指定）
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 取得したオブジェクトを格納する値を宣言
        // Rayがオブジェクトに当たった時hitに値を入れる
        // アイテム以外にも参照できるようにLayer指定は行わない
        Physics.Raycast(ray, out hit, InteractDistance);
    }

    /// <summary>
    /// SetUpViewPointで取得したhitにある物が敵であるか識別する
    /// その後レティクルの色を変更する
    /// </summary>
    public void RecognitionEnemy()
    {
        if (hit.collider != null)
        {
            hitLayer = hit.transform.parent.gameObject.layer;
            if(1 << hitLayer == (int)enemyLayer)
            {
                Reticle.color = Color.red;
                IsEnemy = true;
                return;
            }
        }
        Reticle.color = Color.white;
        IsEnemy = false;
    }

    /// <summary>
    /// 視点移動（Cinemachineを使ってるためプレイヤーの回転のみ）
    /// </summary>
    /// <param name="inputInfo"></param>
    public void Look(InputInfo inputInfo)
    {
        Quaternion cameraRtation = Camera.main.transform.rotation;
        //プレイヤーを回転
        transform.rotation = new Quaternion(0, cameraRtation.y, 0, cameraRtation.w);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    public void Attack()
    {
        if(hit.collider != null)
        {
            audioSource.PlayOneShot(SwingSound);
            if (IsEnemy)
            {
                partHp = hit.transform.GetComponent<PartHp>();
                StartCoroutine(AttackAnim(hit));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hit"></param>
    /// <returns></returns>
    IEnumerator AttackAnim(RaycastHit hit)
    {
        //準備時間まで待つ
        yield return new WaitForSeconds(1.0f);
        partHp.PartDamage(attackPower);
        //レティクルを表示。
        hitReticle.Show();
        Vector3 spawnPos = hit.point - hit.normal * 0.1f;
        //ヒットエフェクトを表示。
        Instantiate(hitEffect, spawnPos, Quaternion.identity);
        audioSource.PlayOneShot(AttackSound);
        //準備時間まで待つ
        yield return new WaitForSeconds(0.05f);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Interact()
    {
        // hitに格納されているとき
        if (hit.collider != null)
        {
            hitLayer = hit.transform.parent.gameObject.layer;

            if (1 << hitLayer == (int)playerLayer || 1 << hitLayer == (int)enemyLayer)
            {
                return;
            }
            else
            {
                if (inventory.AddPart(hit.transform.gameObject))
                {
                    Destroy(hit.transform.gameObject); // 拾ったら消す
                    audioSource.PlayOneShot(GrabSound);
                }
            }
        }
    }

    public void OnCreate(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            robotCreate.CreateRobot();
            audioSource.PlayOneShot(CreateSound);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        // マウスカーソルを非表示に設定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
