using Unity.VisualScripting;
using UnityEngine;

public class RobotHPManager : MonoBehaviour
{
    // 部品ごとのスクリプトを格納する変数
    [SerializeField]public HeadController head;     // 頭

    [SerializeField] public BodyController body;    // 胴

    [SerializeField] public LegController leg;      // 足

    // 部品のデータを格納する変数
    [SerializeField]public PartsData headData;       // 頭

    [SerializeField]public PartsData bodyData;      // 胴

    [SerializeField]public PartsData legData;        // 足
    
    [SerializeField]GameObject explosionEffect;     //死んだときのエフェクト
    RobotAction robotAction;
    
    public int DeleteLayer;                         // 削除するオブジェクト
    
    // ロボットのHPの変数
    int LowCurrentHp = 500;                         // 現在の一番低きHPを管理
    
    const int MinHp = 0;                            // ロボットの最小HPを指定
    
    int TotalHp = 0;                                // ロボットの総HPを指定
    
    int CurrentTotalHp = 0;                         // 現在のロボットのHPを指定
    
    int HeadHp = 0;                                 // ロボットの頭の耐久値を指定
    
    int CurrentHeadHp = 0;                          // 現在のロボットの頭の耐久値を指定
    
    int BodyHp = 0;                                 // ロボットの胴の耐久値を指定
    
    int CurrentBodyHp = 0;                          // 現在のロボットの胴足の耐久値を指定
    
    int LegHp = 0;                                  // ロボットの足の耐久値を指定
    
    int CurrentLegHp = 0;                           // 現在のロボットの足の耐久値を指定
   
    const int endurance = 3;                        // 耐久値からHPを求めるために使用する値
    
    // 爆発に必要な変数
    float explosionRadius = 10f;                    //爆発の広さ
    
    float explosionPower = 300f;                    //爆発の強さ

    // レイヤーを格納する変数
    private int playerLayer;                        // 味方

    private int enemyLayer;                         // 敵

    private int headLayer;                          // 頭

    private int bodyLayer;                          // 胴

    private int legLayer;                           // 足

    // 無敵の判定をするために必要な変数
    const int mutekiTime = 1;                       // 無敵時間

    float coolDownTime = 0;                         // クールタイムを計測する変数

    bool isMuteki = true;                           // 無敵かの判定

    // 敵の数を表示するスクリプトを指定（β版以降なくなるかも）
    // 敵のみに付与するため事前に指定しておけば問題ない
    [SerializeField]EnemyCount enemyCount = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        robotAction = GetComponent<RobotAction>();
        HPCalculate();

        // ステージシーンに登録しているレイヤーを格納
        playerLayer = StageScene.Instance.PlayerLayer;
        enemyLayer = StageScene.Instance.EnemyLayer;
        headLayer = StageScene.Instance.HeadLayer;
        bodyLayer = StageScene.Instance.BodyLayer;
        legLayer = StageScene.Instance.LegLayer;

        // 体の部位ごとに必要な情報を取得
        head = this.transform.GetComponentInChildren<HeadController>();
        body = this.transform.GetComponentInChildren<BodyController>();
        leg = this.transform.GetComponentInChildren<LegController>();
    }

    private void Update()
    {
        if (CurrentTotalHp <= 0 || CurrentHeadHp <= 0 || CurrentBodyHp <= 0 || CurrentLegHp <= 0)
        {
            NoHP();
        }

        // 攻撃を受けたあとの無敵時間
        // isMutekiがfalseなら、直前のフレームからの経過時間を足す
        if (!isMuteki)
        {
            coolDownTime += Time.deltaTime;

            // coolDownTimeがクールタイムを越えたら、isMutekiをtrueに戻して
            // 次に備えて、coolDownTimeを0で初期化
            if (coolDownTime >= mutekiTime)
            {
                isMuteki = true;
                coolDownTime = 0.0f;
            }
        }
    }

    // 子オブジェクトの頭胴足のHPをカウントする
    public void HPCalculate()
    {
        HeadHp = headData.hp;
        BodyHp = bodyData.hp;
        LegHp = legData.hp;

        CurrentHeadHp = HeadHp;
        CurrentBodyHp = BodyHp;
        CurrentLegHp = LegHp;

        // 部位ごとのHPを加算し部位の個数で割ることで全体HPを計算
        TotalHp = (HeadHp + BodyHp + LegHp) / endurance;
        // 現在のHPを更新
        CurrentTotalHp = TotalHp;
    }

    // 解体するときに親から子を外して親を削除
    public void Demolition()
    {
        if (this.transform.gameObject.layer == playerLayer)
        {
            // そのままFor文に入れると子の数が減る度に値が変更されるため現状の子の個数を変数として格納
            var count = this.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                // GetChildを0にしている理由はiにすると親から子を離している設計上
                // countの個数と親の個数に相違が生じて後半にオブジェクトが残るため
                // そのため、親から0番目を対象にしている（最後に親から子を外しているので0番目でも問題ない）

                // 子オブジェクトに付与しているRigidbodyに重力を付与
                this.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                // 子オブジェクトに付与しているRigidbodyに重力の影響を付与
                this.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
                // 子オブジェクトを親オブジェクトから外す
                this.transform.GetChild(0).parent = this.transform.parent;
            }

            // 親オブジェクトを削除
            Destroy(this.transform.gameObject);
        }
    }

    // HPが0になった時の親オブジェクトから外し親オブジェクトを削除
    public void NoHP()
    {
        // そのままFor文に入れると子の数が減る度に値が変更されるため現状の子の個数を変数として格納
        var count = this.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            // HPの低い箇所を削除する
            if(this.transform.GetChild(0).gameObject.layer == DeleteLayer)
            {
                Destroy(this.transform.GetChild(0).gameObject);
            }

            if(this.transform.GetChild(0).gameObject.layer == bodyLayer){
                this.transform.GetChild(0).GetChild(0).GetComponent<Animator>().enabled = false;
                //this.transform.GetChild(0).GetComponent<BodyController>().enabled = false;
            }
            else if (this.transform.GetChild(0).gameObject.layer == legLayer)
            {
                this.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }

            // GetChildを0にしている理由はiにすると親から子を離している設計上
            // countの個数と親の個数に相違が生じて後半にオブジェクトが残るため
            // そのため、親から0番目を対象にしている（最後に親から子を外しているので0番目でも問題ない）

            // 子オブジェクトを親オブジェクトから外す
            this.transform.GetChild(0).parent = this.transform.parent;
        }

        if (head != null ) {
            head.PartsDestroy(false);
        }
        if(body != null)
        {
            body.PartsDestroy(false);
        }
        if(leg != null)
        {
            leg.PartsDestroy(false);
        }

        this.GetComponent<RobotAction>().IsSet = false;

        if (this.transform.gameObject.layer == enemyLayer)
        {
            enemyCount.EnemyDecrease();
        }

        //死亡時のエフェクト
        {
            Instantiate(explosionEffect, new Vector3(transform.position.x,1.5f,transform.position.z), transform.rotation);

            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(explosionPower, new Vector3(transform.position.x, 0, transform.position.z), 4);
                }
            }
        }

        // 親オブジェクトを削除
        Destroy(this.transform.gameObject);
    }

    // 全体HPからダメージを減算
    public void HitDamage(int damage)
    {
        if (isMuteki)
        {
            isMuteki = false;
            CurrentTotalHp = Mathf.Clamp(CurrentTotalHp - damage, MinHp, TotalHp);
            CurrentHeadHp = Mathf.Clamp(CurrentHeadHp - damage / endurance, 0, HeadHp);
            CurrentBodyHp = Mathf.Clamp(CurrentBodyHp - damage / endurance, 0, BodyHp);
            CurrentLegHp = Mathf.Clamp(CurrentLegHp - damage / endurance, 0, LegHp);
        }
    }

    // 部位のHPからダメージを減算
    public void HitDamegePoint(int damage, int layer)
    {
        if (isMuteki)
        {
            isMuteki = false;
            CurrentTotalHp = Mathf.Clamp(CurrentTotalHp - damage, MinHp, TotalHp);
            DeleteLayer = layer;
            if (layer == headLayer)
            {
                CurrentHeadHp = Mathf.Clamp(CurrentHeadHp - damage, 0, HeadHp);
                if (CurrentHeadHp < LowCurrentHp)
                {
                    LowCurrentHp = CurrentHeadHp;
                }
            }
            else if (layer == bodyLayer)
            {
                CurrentBodyHp = Mathf.Clamp(CurrentBodyHp - damage, 0, BodyHp);
                if (CurrentBodyHp < LowCurrentHp)
                {
                    LowCurrentHp = CurrentBodyHp;
                }
            }
            else if (layer == legLayer)
            {
                CurrentLegHp = Mathf.Clamp(CurrentLegHp - damage, 0, LegHp);
                if (CurrentLegHp < LowCurrentHp)
                {
                    LowCurrentHp = CurrentLegHp;
                }
            }
        }
    }
}
