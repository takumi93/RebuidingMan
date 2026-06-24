using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private CameraController _camController;

    private Player _player;

    // HPê›íË
    private const float _maxHp = 100.0f;

    private float _currentHp;

    public float MaxHp => _maxHp;
    public float CurrentHp => _currentHp;

    private const int _mutekiTime = 1;
    private float _coolDownTime = 0;

    bool _isInvincible = false;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        _currentHp = _maxHp;

        _player.UI.InitializeHp(_currentHp, _maxHp);
    }

    private void Update()
    {
        if (_isInvincible)
        {
            _coolDownTime += Time.deltaTime;

            if (_coolDownTime >= _mutekiTime)
            {
                _isInvincible = false;
                _coolDownTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// É_ÉÅÅ[ÉWÇÃèàóù
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="robot"></param>
    public void Damage(int damage, GameObject robot)
    {
        if (!_isInvincible)
        {
            _isInvincible = true;

            _currentHp = Mathf.Clamp(_currentHp - damage, 0, _maxHp);

            _player.UI.OnDamage(_currentHp, _maxHp);

            _player.LastAttacker = robot;

            StartCoroutine(_camController.CameraShake(5, 0.01f));

            if (_currentHp <= 0)
            {
                StageScene.Instance.GameOver();
            }
        }
    }
}
