using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class GameClearUI : BaseUI
{
    // NextStage Button ‚ª‰Ÿ‚³‚ê‚½‚Æ‚«‚É”­¶‚·‚é UnityEvent
    public UnityEvent NextStageRequested;
    // Home Button ‚ª‰Ÿ‚³‚ê‚½‚Æ‚«‚É”­¶‚·‚é UnityEvent
    public UnityEvent HomeRequested;

    // NextStage Button ‚ðŽw’è
    [SerializeField]
    private Button _nextStageButton = null;
    // Home Button ‚ðŽw’è
    [SerializeField]
    private Button _homeButton = null;

    protected override void Awake()
    {
        // UnityEvent
        _nextStageButton.onClick.AddListener(() => { NextStageRequested.Invoke(); });
        _homeButton.onClick.AddListener(() => { HomeRequested.Invoke(); });

        Hide();
    }

    public override void Show()
    {
        base.Show();

        Time.timeScale = 0;

        _nextStageButton.Select();
    }

    public override void Hide()
    {
        Time.timeScale = 1;

        base.Hide();
    }
}
