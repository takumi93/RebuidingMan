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
    //攻撃が当たる長さを指定します
    [SerializeField]
    private float attackDistance = 1;
    //物をつかめる長さを指定します
    [SerializeField]
    private float grabDistance = 1;
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
    [SerializeField]
    GameObject playerCamera = null;
    [SerializeField]
    GameObject playerOb = null;

    RobotHPManager hpManager;

    [SerializeField]
    RobotCreate robotCreate = null;

    Vector2 moveInput;
    //事前に宣言しておく変数
    new Rigidbody rigidbody;
    //public Transform grabObjectTransform = null;

    // ロボットの作成地点を指定
    [SerializeField]public Transform plantPoint;

    public Transform grabHead = null;
    public Transform grabBody = null;
    public Transform grabLeg = null;

    // 味方のレイヤーを指定
    const int LayerNumberPlayer = 6;
    // 敵のレイヤーを指定
    const int LayerNumberEnemy = 7;
    // 味方のレイヤーを指定
    const int LayerNumberHead = 8;
    // 敵のレイヤーを指定
    const int LayerNumberBody = 9;
    // 敵のレイヤーを指定
    const int LayerNumberLeg = 10;

    // 頭のレイヤーを指定
    const int LayerHead = 1 << 8;
    // 胴のレイヤーを指定
    const int LayerBody = 1 << 9;
    // 足のレイヤーを指定
    const int LayerLeg = 1 << 10;

    Animator animator;

    float coolTime = 1.5f;
    float lapseTime;

    public bool isAttackable;

    AudioSource audioSource;

    [SerializeField] AudioClip SwingSound = null;
    [SerializeField] AudioClip GrabSound = null;
    [SerializeField] AudioClip AttackSound = null;
    [SerializeField] AudioClip CreateSound = null;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        animator = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        isAttackable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //y軸をゼロに設定。
        transform.position = new Vector3(transform.position.x, 0,transform.position.z);
        //移動
        if (moveInput != Vector2.zero)
        {
            Move();
            animator.SetBool("Walk", true);
        }
        else
        {
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            animator.SetBool("Walk", false);
        }

        Look();

        if (grabHead)
        {
            grabHead.rotation = this.transform.rotation;
            grabHead.position = plantPoint.position;

        }
        if (grabBody)
        {
            grabBody.transform.rotation = this.transform.rotation;
            grabBody.transform.position = plantPoint.position;

        }
        if (grabLeg)
        {
            grabLeg.transform.rotation = this.transform.rotation;
            grabLeg.transform.position = plantPoint.position;

        }

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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isAttackable)
            {
                isAttackable = false;
                animator.SetTrigger("Attack");
                //カメラの中央からRayを飛ばす
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                RaycastHit hit;
                audioSource.PlayOneShot(SwingSound);

                // layerが頭の時
                if (Physics.Raycast(ray, out hit, attackDistance, LayerHead))
                {
                    Attack(hit, hit.transform.gameObject.layer);
                }
                // layerが胴の時
                else if (Physics.Raycast(ray, out hit, attackDistance, LayerBody))
                {
                    Attack(hit, hit.transform.gameObject.layer);
                }
                // layerが足の時
                else if (Physics.Raycast(ray, out hit, attackDistance, LayerLeg))
                {
                    Attack(hit, hit.transform.gameObject.layer);
                }
            }
        }
    }
    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.started) {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            RaycastHit hit;

            // layerが頭の時
            if (Physics.Raycast(ray, out hit, grabDistance, LayerHead))
            {
                Grab(hit, hit.transform.gameObject.layer);
            }
            // layerが胴の時
            else if (Physics.Raycast(ray, out hit, grabDistance, LayerBody))
            {
                Grab(hit, hit.transform.gameObject.layer);
            }
            // layerが足の時
            else if (Physics.Raycast(ray, out hit, grabDistance, LayerLeg))
            {
                Grab(hit, hit.transform.gameObject.layer);
            }
        }
    }

    public void OnDiscard(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            grabHead = null;
            grabBody = null;
            grabLeg = null;

            robotCreate.IsHeadSet = false;
            robotCreate.IsBodySet = false;
            robotCreate.IsLegSet = false;
        }
    }

    private void Move()
    {
        //方向をワールド空間へ変換
        var speed = new Vector3(moveInput.x, 0, moveInput.y);
        speed = transform.TransformDirection(speed);
        //加速度を代入
        var velocity = rigidbody.linearVelocity;
        velocity.x = speed.x * moveSpeed;
        velocity.y = 0;
        velocity.z = speed.z * moveSpeed;
        rigidbody.linearVelocity = velocity;
    }

    private void Look()
    {
        Quaternion cameraRtation = Camera.main.transform.rotation;
        //プレイヤーを回転
        transform.rotation = new Quaternion(0, cameraRtation.y, 0, cameraRtation.w);
        playerOb.transform.rotation = new Quaternion(0, cameraRtation.y, 0, cameraRtation.w);

    }

    // 味方と敵によって攻撃方法を変更
    public void Attack(RaycastHit hit, int layer)
    {
        // HPを管理しているスクリプトを取得
        hpManager = hit.transform.parent.GetComponent<RobotHPManager>();
        // layerが敵の時
        if (hit.transform.parent.gameObject.layer == LayerNumberEnemy)
        {
            StartCoroutine(SwingAttack(hit, layer));
        }
        // layerが味方の時
        else if (hit.transform.parent.gameObject.layer == LayerNumberPlayer)
        {
            SwingHeal();
        }
    }

    IEnumerator SwingAttack(RaycastHit hit, int layer)
    {
        //準備時間まで待つ
        yield return new WaitForSeconds(0.125f);
        hpManager.HitDamegePoint(attackPower, layer);
        //レティクルを表示。
        hitReticle.Show();
        //ヒットエフェクトを表示。
        Instantiate(hitEffect, hit.point, Quaternion.identity);
        audioSource.PlayOneShot(AttackSound);
        //準備時間まで待つ
        yield return new WaitForSeconds(0.875f);
    }

    IEnumerator SwingHeal()
    {
        //準備時間まで待つ
        yield return new WaitForSeconds(0.125f);
        hpManager.HitHeal(attackPower);
        //準備時間まで待つ
        yield return new WaitForSeconds(0.875f);
    }

    public void Grab(RaycastHit hit, int layer)
    {
        // その場に落ちているオブジェクトを指定するため、親のレイヤーが味方か敵なら無視
        if (hit.transform.parent.gameObject.layer == LayerNumberPlayer || hit.transform.parent.gameObject.layer == LayerNumberEnemy)
        {
            return;
        }
        else
        {
            // layerが頭の時
            if (layer == LayerNumberHead)
            {
                if (!robotCreate.IsHeadSet)
                {
                    grabHead = hit.transform;
                    robotCreate.IsHeadSet = true;
                }
                else
                {
                    return;
                }
            }
            // layerが胴の時
            else if (layer == LayerNumberBody)
            {
                if (!robotCreate.IsBodySet)
                {
                    grabBody = hit.transform;
                    robotCreate.IsBodySet = true;
                }
                else
                {
                    return;
                }

            }
            // layerが足の時
            else if (layer == LayerNumberLeg)
            {
                if (!robotCreate.IsLegSet)
                {
                    grabLeg = hit.transform;
                    robotCreate.IsLegSet = true;
                }
                else
                {
                    return;
                }
            }
            audioSource.PlayOneShot(GrabSound);
        }
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
        robotCreate.createObject();
        audioSource.PlayOneShot(CreateSound);
    }

    private void OnApplicationFocus(bool focus)
    {
        // マウスカーソルを非表示に設定
        Cursor.lockState = CursorLockMode.Locked;
    }
}
