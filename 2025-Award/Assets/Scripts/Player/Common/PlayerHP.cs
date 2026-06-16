using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private Player _player;

    const float MaxLife = 100.0f;
    public float currentLife = 100.0f;
    [SerializeField]
    private GameObject HitEffect = null;
    [SerializeField]
    private CameraController camController;

    const int mutekiTime = 1;
    float coolDownTime = 0;

    bool isMuteki = true;

    Animator HitAnim;

    public Image hpBar;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentLife / MaxLife;
        }
        if (HitEffect != null)
        {
            HitAnim = HitEffect.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (!isMuteki)
        {
            coolDownTime += Time.deltaTime;

            if (coolDownTime >= mutekiTime)
            {
                isMuteki = true;
                coolDownTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// ダメージの処理
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="robot"></param>
    public void Damage(int damage, GameObject robot)
    {
        if (isMuteki)
        {
            isMuteki = false;
            HitAnim?.SetTrigger("Hit");
            currentLife -= damage;

            _player.LastAttacker = robot;

            StartCoroutine(camController.CameraShake(5, 0.01f));
            if (hpBar != null)
            {
                hpBar.fillAmount = currentLife / MaxLife;
            }
            if (currentLife <= 0)
            {
                OnDie();
            }
        }
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    void OnDie()
    {
        StageScene.Instance.GameOver();
    }
}
