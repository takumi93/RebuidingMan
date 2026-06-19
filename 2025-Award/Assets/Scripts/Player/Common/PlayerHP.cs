using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private Player _player;

    private const float _maxLife = 100.0f;
    public float CurrentLife = 100.0f;

    [SerializeField] private CameraController _camController;

    // ヒットエフェクトを表示するUI
    [SerializeField] private HitEffectUI _hitEffect;

    private const int _mutekiTime = 1;
    private float _coolDownTime = 0;

    bool _isMuteki = true;

    public Image hpBar;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = CurrentLife / _maxLife;
        }
    }

    private void Update()
    {
        if (!_isMuteki)
        {
            _coolDownTime += Time.deltaTime;

            if (_coolDownTime >= _mutekiTime)
            {
                _isMuteki = true;
                _coolDownTime = 0.0f;
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
        if (_isMuteki)
        {
            _isMuteki = false;

            CurrentLife -= damage;

            _player.LastAttacker = robot;

            _hitEffect.Play();

            StartCoroutine(_camController.CameraShake(5, 0.01f));
            if (hpBar != null)
            {
                hpBar.fillAmount = CurrentLife / _maxLife;
            }
            if (CurrentLife <= 0)
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
