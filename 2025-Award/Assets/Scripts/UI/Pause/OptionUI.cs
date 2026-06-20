using UnityEngine;
using UnityEngine.UI;

public class OptionUI : BaseUI
{
    [Header("ボタン")]
    [SerializeField] private Button _resumeButton = null;
    [SerializeField] private Button _soundButton = null;
    [SerializeField] private Button _operationButton = null;
    [SerializeField] private Button _iconListButton = null;

    [Header("UI")]
    [SerializeField] private BaseUI _soundUI = null;
    [SerializeField] private BaseUI _operationUI = null;
    [SerializeField] private BaseUI _iconListUI = null;


    protected override void Awake()
    {
        // UnityEvent
        _resumeButton.onClick.AddListener(() => { UIManager.Instance.Pop(); });
        _soundButton.onClick.AddListener(() => { UIManager.Instance.Push(_soundUI); });
        _operationButton.onClick.AddListener(() => { UIManager.Instance.Push(_operationUI); });
        _iconListButton.onClick.AddListener(() => { UIManager.Instance.Push(_iconListUI); });

        // UI非表示
        Hide();
    }

    public override void Show()
    {
        base.Show();

        _soundButton.Select();
    }

    // サウンド、操作説明表示時の裏側,イントロアニメーション中でボタンが反応しないようにする（非表示時は反応する）
    public void ToggleButtonEnabled()
    {
        if (_soundButton.enabled == false)
        {
            _soundButton.enabled = true;
            _operationButton.enabled = true;
            _resumeButton.enabled = true;
        }
        else
        {
            _soundButton.enabled = false;
            _operationButton.enabled = false;
            _resumeButton.enabled = false;
        }
    }
}