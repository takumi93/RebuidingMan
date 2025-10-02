using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    const float MaxLife = 100.0f;
    public float life = 100.0f;
    [SerializeField]
    private GameObject HitEffect = null;
    [SerializeField]
    private CameraController camController;

    const int mutekiTime = 3;
    float coolDownTime = 0;

    bool isMuteki = true;

    Animator HitAnim;

    public Image hpBar;

    void Start()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = life / MaxLife;
        }
        HitAnim = HitEffect.GetComponent<Animator>();
    }

    private void Update()
    {
        // 攻撃のクールタイム
        // isAttackableがfalseなら、直前のフレームからの経過時間を足す
        if (!isMuteki)
        {
            coolDownTime += Time.deltaTime;

            //lapsetimeがクールタイムを越えたら、isAttackableをtrueに戻して
            //次に備えて、lapseTimeを0で初期化
            if (coolDownTime >= mutekiTime)
            {
                isMuteki = true;
                coolDownTime = 0.0f;
            }
        }
    }

    public void Damage(int damage)
    {
        if (isMuteki)
        {
            isMuteki = false;
            HitAnim.SetTrigger("Hit");
            life -= damage;
            StartCoroutine(camController.CameraShake(5, 0.01f));
            if (hpBar != null)
            {
                hpBar.fillAmount = life / MaxLife;
            }
            if (life <= 0)
            {
                OnDie();
            }
        }
    }

    void OnDie()
    {
        StageScene.Instance.GameOver();
    }
}
