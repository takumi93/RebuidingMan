using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class StageSelectUI : BaseUI
{
    //UnityEvent
    public UnityEvent FirstStageRequested;
    public UnityEvent SecondStageRequested;
    public UnityEvent ThirdStageRequested;
    public UnityEvent ForceStageRequested;

    [Header("ƒ{ƒ^ƒ“")]
    [SerializeField] private Button _firstStageButton = null;
    [SerializeField] private Button _secondStagedButton = null;
    [SerializeField] private Button _thirdStageButton = null;
    [SerializeField] private Button _forceStageButton = null;

    protected override void Awake()
    {
        //UnityEvent
        _firstStageButton.onClick.AddListener(() => { FirstStageRequested.Invoke(); });
        _secondStagedButton.onClick.AddListener(() => { SecondStageRequested.Invoke(); });
        _thirdStageButton.onClick.AddListener(() => { ThirdStageRequested.Invoke(); });
        _forceStageButton.onClick.AddListener(() => { ForceStageRequested.Invoke(); });

        //UI‚Ì”ñ•\Ž¦
        Hide();
    }

    public override void Show()
    {
        base.Show();

        _firstStageButton.Select();
    }
}
