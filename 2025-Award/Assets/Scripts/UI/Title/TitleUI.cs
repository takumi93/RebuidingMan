using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TitleUI : BaseUI
{
    //UnityEvent
    public UnityEvent onStageSelectButtonClick = null;
    public UnityEvent onOptionButtonClick = null;
    public UnityEvent onExitButtonClick = null;

    //Buttonを指定
    [SerializeField]
    private Button stageSelectButton = null;
    [SerializeField]
    private Button optionButton = null;
    [SerializeField]
    private Button exitButton = null;

    // ステージセレクト画面
    [SerializeField] private BaseUI _stageSelectUI = null;
    // オプション画面
    [SerializeField] private BaseUI _optionUI = null;


    protected override void Awake()
    {
        // UnityEvent を追加
        stageSelectButton.onClick.AddListener(() => { UIManager.Instance.Push(_stageSelectUI); });
        optionButton.onClick.AddListener(() => { UIManager.Instance.Push(_optionUI); });
        exitButton.onClick.AddListener(() => { onExitButtonClick.Invoke(); });
    }

    //このUIを表示します
    public override void Show()
    {
        base.Show();
        
        stageSelectButton.Select();
    }

    public override void OnOpen()
    {
        InputManager.Instance.EnableUIInput();
    }
}
