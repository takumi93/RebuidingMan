using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TitleUI : BaseUI
{
    //UnityEvent
    public UnityEvent onStageSelectButtonClick = null;
    public UnityEvent onOptionButtonClick = null;
    public UnityEvent onExitButtonClick = null;

    //Button‚ðŽw’è
    [SerializeField]
    private Button stageSelectButton = null;
    [SerializeField]
    private Button optionButton = null;
    [SerializeField]
    private Button exitButton = null;

    protected override void Awake()
    {
        // UnityEvent ‚ð’Ç‰Á
        stageSelectButton.onClick.AddListener(() => { onStageSelectButtonClick.Invoke(); });
        optionButton.onClick.AddListener(() => { onOptionButtonClick.Invoke(); });
        exitButton.onClick.AddListener(() => { onExitButtonClick.Invoke(); });
    }

    //‚±‚ÌUI‚ð•\Ž¦‚µ‚Ü‚·
    public override void Show()
    {
        base.Show();
        
        stageSelectButton.Select();
    }
}
