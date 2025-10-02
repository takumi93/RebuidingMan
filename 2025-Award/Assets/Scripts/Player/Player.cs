using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //プレイヤーの移動速度を指定します
    [SerializeField]
    private float moveSpeed;
    //攻撃範囲の距離を指定します
    [SerializeField]
    private float playerAttackRange = 10;
    [SerializeField]
    private int attackPower = 50;
    //つかんだ時につかんだものが来る場所を指定します。
    [SerializeField]
    private Vector3 grabPosition = Vector3.down;
    //攻撃が当たった時のエフェクトを指定します。
    [SerializeField]
    GameObject hitEffect = null;
    //HitReticleを指定します。
    [SerializeField]
    HitReticle hitReticle = null;

    RobotHPManager hpManager;

    [SerializeField]
    RobotCreate robotCreate = null;

    Vector2 moveInput;
    //事前に宣言しておく変数
    new Rigidbody rigidbody;
    //public Transform grabObjectTransform = null;

    // ロボットの作成地点を指定
    [SerializeField]public Transform plantPoint;

    // 味方のレイヤーを格納する変数
    private int playerLayer;
    // 敵のレイヤーを格納する変数
    private int enemyLayer;
    // 頭のレイヤーを格納する変数
    private int headLayer;
    // 胴のレイヤーを格納する変数
    private int bodyLayer;
    // 足のレイヤーを格納する変数
    private int legLayer;

    // Animatorを格納する変数
    private Animator animator;
    // 歩いているかの判定をするための変数
    private bool isWalk = false;


    float coolTime = 1.5f;
    float lapseTime;

    private bool isAttackable;

    AudioSource audioSource;

    [SerializeField] AudioClip SwingSound = null;
    [SerializeField] AudioClip GrabSound = null;
    [SerializeField] AudioClip AttackSound = null;
    [SerializeField] AudioClip CreateSound = null;

    // Rayが当たったオブジェクトを格納する変数を宣言
    RaycastHit hit;

    // Hitに格納しているオブジェクトのレイヤーを格納する変数を宣言
    private int hitLayer;

    private PlayerInventory inventory;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        animator = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        isAttackable = true;

        // ステージシーンに登録しているレイヤーを格納
        playerLayer = StageScene.Instance.PlayerLayer;
        enemyLayer = StageScene.Instance.EnemyLayer;
        headLayer = StageScene.Instance.HeadLayer;
        bodyLayer = StageScene.Instance.BodyLayer;
        legLayer = StageScene.Instance.LegLayer;

        inventory = this.GetComponent<PlayerInventory>();

    }

    // Update is called once per frame
    void Update()
    {
        //カメラの中央からRayを飛ばす
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        
        // Rayを攻撃範囲の距離分、飛ばしてhitに格納
        Physics.Raycast(ray, out hit, playerAttackRange);

        Look();

        // 攻撃のクールタイム
        // isAttackableがfalseなら、直前のフレームからの経過時間を足す
        if (!isAttackable)
        {
            lapseTime += Time.deltaTime;

            //lapsetimeがクールタイムを越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (lapseTime >= coolTime)
            {
                isAttackable = true;
                lapseTime = 0.0f;
            }
        }
    }

    private void FixedUpdate()
    {
        // onMoveクラスにて取得した値が0ではないとき
        if (moveInput != Vector2.zero)
        {
            //ベクトルをワールド空間へ変換
            var speed = new Vector3(moveInput.x, 0, moveInput.y);
            speed = transform.TransformDirection(speed);

            //加速度をRigidbodyに代入
            var velocity = rigidbody.linearVelocity;
            velocity.x = speed.x * moveSpeed;
            velocity.y = 0;
            velocity.z = speed.z * moveSpeed;
            rigidbody.linearVelocity = velocity;

        }
        else
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        //y軸をゼロに設定。
        transform.position = new Vector3(transform.position.x, 0,transform.position.z);
        animator.SetBool("Walk", isWalk);
    }

    // 移動キーが押された際にInputSystemを通して値を取得する
    public void OnMove(InputAction.CallbackContext context)
    {
        // 移動キーをmoveInputに格納
        moveInput = context.ReadValue<Vector2>();
        // 歩いているのか判定をisWalkに格納
        if (context.started)
        {
            isWalk = true;
        }
        else if (context.canceled)
        {
            isWalk = false;
        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // 攻撃可能な場合
            if(isAttackable)
            {
                isAttackable = false;
                // 攻撃のアニメーションを再生
                animator.SetTrigger("Attack");

                // hitに何も格納されていないとき
                if (hit.collider != null)
                {
                    audioSource.PlayOneShot(SwingSound);

                    hitLayer = hit.transform.gameObject.layer;

                    // hitに格納されているオブジェクトのレイヤーが頭または胴、足の時
                    if (hitLayer == headLayer || hitLayer == bodyLayer || hitLayer == legLayer)
                    {
                        Attack(hit, hitLayer);
                    }
                    // それ以外は無視
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started) {
            // hitに格納されているとき
            if (hit.collider != null)
            {
                hitLayer = hit.transform.parent.gameObject.layer;

                if (hitLayer == playerLayer || hitLayer == enemyLayer)
                {
                    return ;
                }
                else
                {
                    PartsPickup pickup = hit.collider.GetComponent<PartsPickup>();
                    if (pickup != null)
                    {
                        PartsData part = pickup.GetPartData();
                        inventory.AddPart(part);

                        Destroy(pickup.gameObject); // 拾ったら消す
                        audioSource.PlayOneShot(GrabSound);
                    }
                }
            }
        }
    }

    public void OnDiscard(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            
        }
    }

    private void Look()
    {
        Quaternion cameraRtation = Camera.main.transform.rotation;
        //プレイヤーを回転
        this.transform.rotation = new Quaternion(0, cameraRtation.y, 0, cameraRtation.w);
    }

    /// <summary>
    ///     攻撃メソッド。攻撃方法を記載
    ///     攻撃方法は味方と敵で処理を分ける
    /// </summary>
    public void Attack(RaycastHit hit, int layer)
    {
        // HPを管理しているスクリプトを取得
        hpManager = hit.transform.parent.GetComponent<RobotHPManager>();
        // layerが敵の時
        if (hit.transform.parent.gameObject.layer == enemyLayer)
        {
            StartCoroutine(SwingAttack(hit, layer));
        }
    }

    IEnumerator SwingAttack(RaycastHit hit, int layer)
    {
        //準備時間まで待つ
        yield return new WaitForSeconds(1.0f);
        hpManager.HitDamegePoint(attackPower, layer);
        //レティクルを表示。
        hitReticle.Show();
        //ヒットエフェクトを表示。
        Instantiate(hitEffect, hit.point, Quaternion.identity);
        audioSource.PlayOneShot(AttackSound);
        //準備時間まで待つ
        yield return new WaitForSeconds(0.05f);
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StageScene.Instance.Cancel();
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
