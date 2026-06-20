using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] private Image _hpBarImage;

    /// <summary>
    /// HPバーを更新
    /// </summary>
    /// <param name="currentHp">現在のHP</param>
    /// <param name="maxHp">最大HP</param>
    public void UpdateHp(float currentHp, float maxHp)
    {
        _hpBarImage.fillAmount = currentHp / maxHp;
    }
}
