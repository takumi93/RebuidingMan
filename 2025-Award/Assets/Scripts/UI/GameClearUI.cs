using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class GameClearUI : MonoBehaviour
{
    // NextStage Button が押されたときに発生する UnityEvent
    public UnityEvent onNextStageButtonClick;
    // Home Button が押されたときに発生する UnityEvent
    public UnityEvent onHomeButtonClick;

    // NextStage Button を指定
    [SerializeField]
    private Button nextStageButton = null;
    // Home Button を指定
    [SerializeField]
    private Button homeButton = null;
    // 使用するテキストを格納
    //[SerializeField] TextMeshProUGUI Score = null;

    void Awake()
    {
        // UnityEvent を追加
        nextStageButton.onClick.AddListener(() => { onNextStageButtonClick.Invoke(); });
        homeButton.onClick.AddListener(() => { onHomeButtonClick.Invoke(); });

        // 最初は非表示
        Hide();
    }

    // GameClearUIを表示
    public void Show()
    {
        // 子オブジェクトをすべてアクティブ化
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        // 最初に選択されるボタンを指定
        nextStageButton.Select();
    }

    // GameOverUIを非表示
    public void Hide()
    {
        // 子オブジェクトをすべて非アクティブ化
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
