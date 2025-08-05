using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class StageSelectUI : MonoBehaviour
{
    //UnityEvent
    public UnityEvent OnFirstStageButtonClick;
    public UnityEvent OnSecondStageButtonClick;
    public UnityEvent OnThirdStageButtonClick;
    public UnityEvent OnForceStageButtonClick;

    //ボタンの指定
    [SerializeField] private Button firstStageButton = null;
    [SerializeField] private Button secondStagedButton = null;
    [SerializeField] private Button thirdStageButton = null;
    [SerializeField] private Button forceStageButton = null;

    private void Awake()
    {
        //UnityEvent
        firstStageButton.onClick.AddListener(() => { OnFirstStageButtonClick.Invoke(); });
        secondStagedButton.onClick.AddListener(() => { OnSecondStageButtonClick.Invoke(); });
        thirdStageButton.onClick.AddListener(() => { OnThirdStageButtonClick.Invoke(); });
        forceStageButton.onClick.AddListener(() => { OnForceStageButtonClick.Invoke(); });

        //UIの非表示
        Hide();
    }

    //UIの表示
    public void Show()
    {
        gameObject.SetActive(true);
        firstStageButton.Select();
    }

    //UIの非表示
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
