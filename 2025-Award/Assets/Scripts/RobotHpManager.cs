using Unity.VisualScripting;
using UnityEngine;

public class RobotHPManager : MonoBehaviour
{
    // ロボットの詳細データを指定
    [SerializeField]
    public RobotData robotData;
    //死んだときのエフェクト
    [SerializeField]
    GameObject explosionEffect;
    RobotAction robotAction;
    // 現在の一番低きHPを管理
    int hpLowCurrent = 500;
    // 削除するオブジェクト
    public int DeleteLayer;
    // ロボットの最小HPを指定
    const int HpMin = 0;
    // ロボットの総HPを指定
    int HpTotal = 0;
    // 現在のロボットのHPを指定
    int HpCurrentTotal = 0;
    // ロボットの頭の耐久値を指定
    int HpHead = 0;
    // 現在のロボットの頭の耐久値を指定
    int HpCurrentHead = 0;
    // ロボットの胴の耐久値を指定
    int HpBody = 0;
    // 現在のロボットの胴足の耐久値を指定
    int HpCurrentBody = 0;
    // ロボットの足の耐久値を指定
    int HpLeg = 0;
    // 現在のロボットの足の耐久値を指定
    int HpCurrentLeg = 0;
    // 耐久値からHPを求めるために使用する値
    const int endurance = 3;
    //爆発の広さ
    float explosionRadius = 10f;
    //爆発の強さ
    float explosionPower = 300f;

    // 味方のレイヤーを指定
    const int LayerNumberPlayer = 6;
    // 敵のレイヤーを指定
    const int LayerNumberEnemy = 7;
    // 頭のレイヤーを指定
    const int LayerHead = 8;
    // 胴のレイヤーを指定
    const int LayerBody = 9;
    // 足のレイヤーを指定
    const int LayerLeg = 10;

    const int mutekiTime = 1;
    float coolDownTime = 0;

    bool isMuteki = true;

    // 敵の数を表示するスクリプトを指定（β版以降なくなるかも）
    // 敵のみに付与するため事前に指定しておけば問題ない
    [SerializeField]EnemyCount enemyCount = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        robotAction = GetComponent<RobotAction>();
        HPCalculate();
    }

    private void Update()
    {
        if (HpCurrentTotal <= 0 || HpCurrentHead <= 0 || HpCurrentBody <= 0 || HpCurrentLeg <= 0)
        {
            NoHP();
        }

        // 攻撃のクールタイム
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
        // 頭のデータリストの個数を取得
        int headDataCount = robotData.HeadDataList.Count;
        // 胴のデータリストの個数を取得
        int bodyDataCount = robotData.BodyDataList.Count;
        // 足のデータリストの個数を取得
        int legDataCount = robotData.LegDataList.Count;

        // 子オブジェクトを参照
        for (int i = 0; i < transform.childCount; i++)
        {
            if(this.transform.GetChild(i).gameObject.layer == LayerHead)
            {
                // 頭のデータリストを参照
                for (int j = 0; j < headDataCount; j++)
                {
                    // 子オブジェクトとデータリストが一緒の時
                    if (this.transform.GetChild(i).name == robotData.HeadDataList[j].Head.name)
                    {
                        // 頭のHPを取得
                        HpHead = robotData.HeadDataList[j].HeadHP;
                        HpCurrentHead = HpHead;
                        robotAction.CategoryHead = robotData.HeadDataList[j].HeadCategory;
                        break;
                    }
                }
            }
            
            if (this.transform.GetChild(i).gameObject.layer == LayerBody)
            {
                // 胴のデータリストを参照
                for (int j = 0; j < bodyDataCount; j++)
                {
                    // 子オブジェクトとデータリストが一緒の時
                    if (this.transform.GetChild(i).name == robotData.BodyDataList[j].Body.name)
                    {
                        // 胴のHPを取得
                        HpBody = robotData.BodyDataList[j].BodyHP;
                        HpCurrentBody = HpBody;
                        robotAction.CategoryBody = robotData.BodyDataList[j].BodyCategory;
                        break;
                    }
                }
                this.transform.GetChild(i).GetChild(0).GetComponent<Animator>().enabled = true;
            }
            if(this.transform.GetChild(i).gameObject.layer == LayerLeg)
            {
                // 足のデータリストを参照
                for (int j = 0; j < legDataCount; j++)
                {
                    // 子オブジェクトとデータリストが一緒の時
                    if (this.transform.GetChild(i).name == robotData.LegDataList[j].Leg.name)
                    {
                        // 足のHPを取得
                        HpLeg = robotData.LegDataList[j].LegHP;
                        HpCurrentLeg = HpLeg;
                        robotAction.CategoryLeg = robotData.LegDataList[j].LegCategory;
                        break;
                    }
                }
                this.transform.GetChild(i).GetComponent<Animator>().enabled = true;
            }
        }
        // 部位ごとのHPを加算し部位の個数で割ることで全体HPを計算
        HpTotal = (HpHead + HpBody + HpLeg) / endurance;
        // 現在のHPを更新
        HpCurrentTotal = HpTotal;
        
    }

    // 解体するときに親から子を外して親を削除
    public void Demolition()
    {
        if (this.transform.gameObject.layer == LayerNumberPlayer)
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

            if(this.transform.GetChild(0).gameObject.layer == LayerBody){
                this.transform.GetChild(0).GetChild(0).GetComponent<Animator>().enabled = false;
                this.transform.GetChild(0).GetComponent<RobotAttackMethod>().enabled = false;
            }
            else if (this.transform.GetChild(0).gameObject.layer == LayerLeg)
            {
                this.transform.GetChild(0).GetComponent<Animator>().enabled = false;
            }

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

        if (this.transform.gameObject.layer == LayerNumberEnemy)
        {
            enemyCount.EnemyDecrease();
            // 親オブジェクトを削除
            Destroy(this.transform.gameObject);
        }
        else if (this.transform.gameObject.layer == LayerNumberPlayer)
        {
            // 親オブジェクトを削除
            Destroy(this.transform.gameObject);
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
    }

    // 全体HPからダメージを減算
    public void HitDamage(int damage)
    {
        if (isMuteki)
        {
            isMuteki = false;
            HpCurrentTotal = Mathf.Clamp(HpCurrentTotal - damage, HpMin, HpTotal);
            HpCurrentHead = Mathf.Clamp(HpCurrentHead - damage / endurance, 0, HpHead);
            HpCurrentBody = Mathf.Clamp(HpCurrentBody - damage / endurance, 0, HpBody);
            HpCurrentLeg = Mathf.Clamp(HpCurrentLeg - damage / endurance, 0, HpLeg);
        }
    }

    // 部位のHPからダメージを減算
    public void HitDamegePoint(int damage, int layer)
    {
        if (isMuteki)
        {
            isMuteki = false;
            HpCurrentTotal = Mathf.Clamp(HpCurrentTotal - damage, HpMin, HpTotal);
            if (LayerHead == layer)
            {
                HpCurrentHead = Mathf.Clamp(HpCurrentHead - damage, 0, HpHead);
                if (HpCurrentHead < hpLowCurrent)
                {
                    hpLowCurrent = HpCurrentHead;
                    DeleteLayer = LayerHead;
                }
            }
            else if (LayerBody == layer)
            {
                HpCurrentBody = Mathf.Clamp(HpCurrentBody - damage, 0, HpBody);
                if (HpCurrentBody < hpLowCurrent)
                {
                    hpLowCurrent = HpCurrentBody;
                    DeleteLayer = LayerBody;
                }
            }
            else if (LayerLeg == layer)
            {
                HpCurrentLeg = Mathf.Clamp(HpCurrentLeg - damage, 0, HpLeg);
                if (HpCurrentLeg < hpLowCurrent)
                {
                    hpLowCurrent = HpCurrentLeg;
                    DeleteLayer = LayerLeg;
                }
            }
        }
    }

    // 全体HPからダメージ分、回復
    public void HitHeal(int damage)
    {
        HpCurrentTotal = Mathf.Clamp(HpCurrentTotal + damage, HpMin, HpTotal);
        HpCurrentHead = Mathf.Clamp(HpCurrentHead + damage / endurance, 0, HpHead);
        HpCurrentBody = Mathf.Clamp(HpCurrentBody + damage / endurance, 0, HpBody);
        HpCurrentLeg = Mathf.Clamp(HpCurrentLeg + damage / endurance, 0, HpLeg);
        //HpLeg -= damage;
        //HpTotal += damage;
    }
}
