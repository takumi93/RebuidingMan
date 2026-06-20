using UnityEngine;

// 常時表示させるためBaseUIは継承させない
public class PlayUI : MonoBehaviour
{
    [SerializeField] private HitEffectUI _hitEffectUI;

    [SerializeField] private HPBarUI _hpBarUI;

    [SerializeField] private RobotSetUI _robotSetUI;

    [SerializeField] private EnemyCount _enemyCount;

    [SerializeField] private ReticleUI _reticleUI;

    [SerializeField] private StageFadeUI _stageFadeUI;

    /// <summary>
    /// HPの初期化
    /// </summary>
    /// <param name="currentHp"></param>
    /// <param name="maxHp"></param>
    public void InitializeHp(float currentHp, float maxHp)
    {
        _hpBarUI.UpdateHp(currentHp, maxHp);
    }

    /// <summary>
    /// アイコンの更新
    /// </summary>
    /// <param name="inventory"></param>
    public void RefreshParts(PlayerInventory inventory)
    {
        _robotSetUI.RefreshIcon(inventory);
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="currentHP"></param>
    /// <param name="maxHp"></param>
    public void OnDamage(float currentHP, float maxHp)
    {
        _hpBarUI.UpdateHp(currentHP, maxHp);
        _hitEffectUI.PlayHitEffect();
    }

    /// <summary>
    /// 敵の増加
    /// </summary>
    public void EnemyIncrease()
    {
        _enemyCount.EnemyIncrease();
    }

    /// <summary>
    /// 敵の減少
    /// </summary>
    public void EnemyDecrease()
    {
        _enemyCount.EnemyDecrease();
    }

    /// <summary>
    /// 通常の時はレティクルの色を白色にする
    /// </summary>
    public void SetReticleNormal()
    {
        _reticleUI.SetReticleColor(Color.white);
    }

    /// <summary>
    /// 敵の時はレティクルの色を赤色にする
    /// </summary>
    public void SetReticleEnemy()
    {
        _reticleUI.SetReticleColor(Color.red);
    }

    /// <summary>
    /// アイテムの時はレティクルの色を黄色にする
    /// </summary>
    public void SetReticleItem()
    {
        _reticleUI.SetReticleColor(Color.yellow);
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void FadeIn()
    {
        _stageFadeUI.FadeIn();
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void FadeOut()
    {
        _stageFadeUI.FadeOut();
    }
}
