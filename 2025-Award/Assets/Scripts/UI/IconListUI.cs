using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IconListUI : MonoBehaviour
{
    //UnityEvent
    public UnityEvent onResumeButtonClick = null;

    //Button‚ðŽw’è
    //[SerializeField] private Button resumeButton = null;

    void Awake()
    {
        // UnityEvent ‚ð’Ç‰Á
        
        Hide();
    }

    //UI•\Ž¦
    public void Show()
    {
        gameObject.SetActive(true);
        //resumeButton.Select();
    }

    //UI”ñ•\Ž¦
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
