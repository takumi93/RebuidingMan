using UnityEngine;
using UnityEngine.UI;

public class RobotSetUI : MonoBehaviour
{
    [Header("部位ごとにセットされたら表示する画像")]
    // 頭のUIを指定
    [SerializeField] private GameObject _headImage;
    // 胴のUIを指定
    [SerializeField] private GameObject _bodyImage;
    // 足のUIを指定
    [SerializeField] private GameObject _legImage;

    [Header("表示する役割アイコン")]
    // 頭の画像を指定
    [SerializeField] private Image _headIconImage;
    // 胴の画像を指定
    [SerializeField] private Image _bodyIconImage;
    // 足の画像を指定
    [SerializeField] private Image _legIconImage;

    [Header("部位ごとの役割アイコン一覧")]
    // 頭の役割アイコンを指定
    [SerializeField] private Sprite[] _headIcons;
    // 胴の役割アイコンを指定
    [SerializeField] private Sprite[] _bodyIcons;
    // 足の役割アイコンを指定
    [SerializeField] private Sprite[] _legIcons;
    // 役割アイコンを指定
    [SerializeField] private Sprite _iconEmpty;

    /// <summary>
    /// アイコンの変更処理
    /// </summary>
    /// <param name="inventory"></param>
    public void RefreshIcon(PlayerInventory inventory)
    {
        UpdateHead(inventory);
        UpdateBody(inventory);
        UpdateLeg(inventory);
    }

    /// <summary>
    /// 頭アイコンの変更処理
    /// </summary>
    private void UpdateHead(PlayerInventory inventory)
    {
        // インベントリに頭オブジェクトがないとき
        if (!inventory.HasHead)
        {
            _headImage.SetActive(false);
            _headIconImage.sprite = _iconEmpty;
            return;
        }

        _headImage.SetActive(true);

        // 頭オブジェクトの種類によって画像を入れ替える
        if(inventory.GetPartOfType(PartsType.Head) is HeadData headData)
        {
            _headIconImage.sprite = _headIcons[(int)headData.headType];
        }
    }

    /// <summary>
    /// 体アイコンの変更処理
    /// </summary>
    private void UpdateBody(PlayerInventory inventory)
    {
        // インベントリに体オブジェクトがないとき
        if (!inventory.HasBody)
        {
            _bodyImage.SetActive(false);
            _bodyIconImage.sprite = _iconEmpty;
            return;
        }

        _bodyImage.SetActive(true);

        // 体オブジェクトの種類によって画像を入れ替える
        if (inventory.GetPartOfType(PartsType.Body) is BodyData bodyData)
        {
            _bodyIconImage.sprite = _bodyIcons[(int)bodyData.BodyType];
        }
    }

    /// <summary>
    /// 足アイコンの変更処理
    /// </summary>
    private void UpdateLeg(PlayerInventory inventory)
    {
        // インベントリに足オブジェクトがないとき
        if (!inventory.HasLeg)
        {
            _legImage.SetActive(false);
            _legIconImage.sprite = _iconEmpty;
            return;
        }

        _legImage.SetActive(true);

        // 足オブジェクトの種類によって画像を入れ替える
        if (inventory.GetPartOfType(PartsType.Leg) is LegData legData)
        {
            _legIconImage.sprite = _legIcons[(int)legData.legType];
        }
    }
}
