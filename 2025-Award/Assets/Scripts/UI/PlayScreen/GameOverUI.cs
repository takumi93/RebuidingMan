using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameOverUI : BaseUI
{
    // Retry Button ‚ª‰Ÿ‚³‚ê‚½‚Æ‚«‚É”­¶‚·‚é UnityEvent
    public UnityEvent RetryRequested;
    // Home Button ‚ª‰Ÿ‚³‚ê‚½‚Æ‚«‚É”­¶‚·‚é UnityEvent
    public UnityEvent HomeRequested;

    // Retry Button ‚ðŽw’è
    [SerializeField]
    private Button _retryButton = null;
    // Home Button ‚ðŽw’è
    [SerializeField]
    private Button _homeButton = null;

    protected override void Awake()
    {
        // UnityEvent
        _retryButton.onClick.AddListener(() => { RetryRequested.Invoke(); });
        _homeButton.onClick.AddListener(() => { HomeRequested.Invoke(); });

        Hide();
    }

    public override void Show()
    {
        Debug.Log("GameOver Open");
        base.Show();

        Time.timeScale = 0;

        _retryButton.Select();
    }

    public override void Hide()
    {
        Time.timeScale = 1;

        base.Hide();
    }

    public override void OnOpen()
    {
        InputManager.Instance.EnableUIInput();
        Time.timeScale = 0;
    }

    public override void OnClose()
    {
        InputManager.Instance.EnablePlayerInput();

        Time.timeScale = 1;
    }
}