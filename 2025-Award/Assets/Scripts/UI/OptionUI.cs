using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class OptionUI : MonoBehaviour
{
    // UnityEvent
    public UnityEvent onResumeButtonClick = null;
    public UnityEvent onSoundButtonClick = null;
    public UnityEvent onOperatingButtonClick = null;

    public UnityEvent onIconListButtonClick;

    // Buttonを指定
    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button soundButton = null;
    [SerializeField] private Button operatingButton = null;
    [SerializeField] private Button IconListButton = null;

    private void Awake()
    {
        // UnityEvent
        resumeButton.onClick.AddListener(() => { onResumeButtonClick.Invoke(); });
        soundButton.onClick.AddListener(() => { onSoundButtonClick.Invoke(); });
        operatingButton.onClick.AddListener(() => { onOperatingButtonClick.Invoke(); });
        IconListButton.onClick.AddListener(() => { onIconListButtonClick.Invoke(); });

        // UI非表示
        Hide();
    }

    // UI表示
    public void Show()
    {
        gameObject.SetActive(true);
        soundButton.Select();
    }
    // UI非表示
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // サウンド、操作説明表示時の裏側,イントロアニメーション中でボタンが反応しないようにする（非表示時は反応する）
    public void ToggleButtonEnabled()
    {
        if (soundButton.enabled == false)
        {
            soundButton.enabled = true;
            operatingButton.enabled = true;
            resumeButton.enabled = true;
        }
        else
        {
            soundButton.enabled = false;
            operatingButton.enabled = false;
            resumeButton.enabled = false;
        }
    }
}