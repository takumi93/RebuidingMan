using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PauseUI : BaseUI
{
    //UnityEvent
    public UnityEvent RetryRequested = null;
    public UnityEvent TitleRequested = null;

    //Buttonを指定
    [SerializeField]
    private Button _resumeButton = null;
    [SerializeField]
    private Button _retryButton = null;
    [SerializeField]
    private Button _optionButton = null;
    [SerializeField]
    private Button _titleButton = null;

    // オプション画面
    [SerializeField] private BaseUI _optionUI = null;

    protected override void Awake()
    {
        base.Awake();

        // UnityEvent を追加
        _resumeButton.onClick.AddListener(() => { UIManager.Instance.Pop(); });
        _retryButton.onClick.AddListener(() => { RetryRequested.Invoke(); });
        _optionButton.onClick.AddListener(() => { UIManager.Instance.Push(_optionUI); });
        _titleButton.onClick.AddListener(() => { TitleRequested.Invoke(); });

        base.Hide();
    }

    public override void Show()
    {
        base.Show();

        InputManager.Instance.EnableUIInput();

        Time.timeScale = 0;

        _resumeButton.Select();
    }

    public override void Hide()
    {
        InputManager.Instance.EnablePlayerInput();

        Time.timeScale = 1;

        base.Hide();
    }
}
