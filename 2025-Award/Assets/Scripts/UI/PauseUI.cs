using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PauseUI : MonoBehaviour
{
    //UnityEvent
    public UnityEvent onResumeButtonClick = null;
    public UnityEvent onRetryButtonClick = null;
    public UnityEvent onOptionButtonClick = null;
    public UnityEvent onTitleButtonClick = null;

    //Button‚ðŽw’è
    [SerializeField]
    private Button resumeButton = null;
    [SerializeField]
    private Button retryButton = null;
    [SerializeField]
    private Button optionButton = null;
    [SerializeField]
    private Button titleButton = null;

    void Awake()
    {
        // UnityEvent ‚ð’Ç‰Á
        resumeButton.onClick.AddListener(() => { onResumeButtonClick.Invoke(); });
        retryButton.onClick.AddListener(() => { onRetryButtonClick.Invoke(); });
        optionButton.onClick.AddListener(() => { onOptionButtonClick.Invoke(); });
        titleButton.onClick.AddListener(() => { onTitleButtonClick.Invoke(); });

        Hide();
    }

    //‚±‚ÌUI‚ð•\Ž¦‚µ‚Ü‚·
    public void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    //‚±‚ÌUI‚ð”ñ•\Ž¦‚É‚µ‚Ü‚·
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
