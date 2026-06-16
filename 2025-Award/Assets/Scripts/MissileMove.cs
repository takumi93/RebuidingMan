using UnityEngine;

public class MissileMove : MonoBehaviour
{
    // 味方のレイヤーを指定
    const int LayerNumberPlayer = 6;
    // 敵のレイヤーを指定
    const int LayerNumberEnemy = 7;
    // 味方の攻撃範囲レイヤーの指定
    int LayerNumberShot = 11;
    // 敵の攻撃範囲レイヤーの指定
    int LayerNumberEnemyShot = 12;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * 0.2f);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
        if (this.transform.gameObject.layer == LayerNumberShot)
        {
            if (other.gameObject.layer == LayerNumberEnemy)
            {
                var Damage = other.gameObject.GetComponent<RobotHPManager>();
                //Damage.HitDamage(30);
                Destroy(this.gameObject);
            }
        }else if (this.transform.gameObject.layer == LayerNumberEnemyShot)
        {
            if (other.gameObject.CompareTag("Ally"))
            {
                var Damage = other.gameObject.GetComponent<RobotHPManager>();
                //Damage.HitDamage(12);
                Destroy(this.gameObject);
            }else if (other.gameObject.CompareTag("Player")){
                var playerHP = other.gameObject.GetComponent<PlayerHP>();
                //playerHP.Damage(20);
                Destroy(this.gameObject);
            }
        }
    }
}
