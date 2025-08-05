using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameOverUI : MonoBehaviour
{// Retry Button が押されたときに発生する UnityEvent
    public UnityEvent onRetryButtonClick;
    // Home Button が押されたときに発生する UnityEvent
    public UnityEvent onHomeButtonClick;

    // Retry Button を指定
    [SerializeField]
    private Button retryButton = null;
    // Home Button を指定
    [SerializeField]
    private Button homeButton = null;
    // 使用するテキストを格納
    //[SerializeField] TextMeshProUGUI Score = null;

    void Awake()
    {
        // UnityEvent を追加
        retryButton.onClick.AddListener(() => { onRetryButtonClick.Invoke(); });
        homeButton.onClick.AddListener(() => { onHomeButtonClick.Invoke(); });

        // 最初は非表示
        Hide();
    }

    // GameOverUIを表示
    public void Show()
    {
        // 子オブジェクトをすべてアクティブ化
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        retryButton.Select();
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