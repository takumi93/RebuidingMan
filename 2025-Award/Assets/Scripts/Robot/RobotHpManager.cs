using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotHPManager : MonoBehaviour
{
    [SerializeField]GameObject explosionEffect;     //死んだときのエフェクト
   
    [SerializeField]EnemyCount enemyCount = null;

    [SerializeField] PartsDatabase _partsDatabase = null;

    public int MaxTotalHP { get; private set; }
    public int CurrentTotalHP { get; private set; }

    const int endurance = 3;                        // 耐久値からHPを求めるために使用する値
    
    // 爆発に必要な変数
    float explosionRadius = 10f;                    //爆発の広さ
    
    float explosionPower = 300f;                    //爆発の強さ

    public List<PartHp> Parts = new List<PartHp>();

    private Robot _robot { get; set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 新規追加
        Parts = GetComponentsInChildren<PartHp>().ToList();
        _robot = GetComponent<Robot>();

        MaxTotalHP = 0;
        foreach (var p in Parts)
        {
            p.Init(_partsDatabase);
            MaxTotalHP += p.MaxHP;
        }
        MaxTotalHP = MaxTotalHP / endurance;
        CurrentTotalHP = MaxTotalHP;
    }

    // 全体HPからダメージを減算
    public void HitDamage(int damage)
    {
        CurrentTotalHP = Mathf.Max(CurrentTotalHP - damage, 0);
    }

    // ---- 部位が死んだ通知 ----
    public void OnPartDead(PartHp deadPart)
    {
        DestroyLowestHPPart();
    }

    // ---- 総合HPが0になっても呼ぶ ----
    public void CheckTotalHP()
    {
        if (CurrentTotalHP <= 0)
        {
            DestroyLowestHPPart();
        }
    }

    // ---- 一番HPが低い部位を破壊 ----
    public void DestroyLowestHPPart()
    {
        var target = Parts
            .Where(p => p.CurrentHP >= 0)    // まだ壊れてない部位
            .OrderBy(p => p.CurrentHP)
            .FirstOrDefault();

        if (target == null)
        {
            return;
        }

        // ここで実際の破壊
        Destroy(target.gameObject);
        DestroyRobot();
    }

    // 親を削除して子を残す
    public void DestroyRobot()
    {
        // 子をワールド空間に残す
        foreach (Transform child in transform)
        {
            var childRigidbody = child.GetComponent<Rigidbody>();
            childRigidbody.isKinematic = false;
            childRigidbody.useGravity = true;
            child.SetParent(transform.parent, true);
        }

        // 頭にアニメーションはないため無視
        _robot.body?.Animation.DestoryAnimation();
        _robot.leg?.Animation.DestoryAnimation();

        //破壊時のエフェクト
        {
            Instantiate(explosionEffect, new Vector3(transform.position.x, 1.5f, transform.position.z), transform.rotation);

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

        enemyCount?.EnemyDecrease();
        // 親を削除
        Destroy(gameObject);
    }
}
