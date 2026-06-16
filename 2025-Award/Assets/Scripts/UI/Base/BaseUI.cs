using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseUI : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _canvasGroup;

    [SerializeField] protected bool _canCloseByCancel = true;

    public bool CanCloseByChancel => _canCloseByCancel;

    protected UIManager _uiManager;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        _uiManager = UIManager.Instance;
    }

    /// <summary>
    /// UIの表示
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// UIの非表示
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// このUIが選択状態の時の処理
    /// </summary>
    public virtual void OnFocus()
    {
        _canvasGroup.interactable = true;

        _canvasGroup.blocksRaycasts = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// このUIが非選択状態になった時の処理
    /// </summary>
    public virtual void OnUnFocus()
    {
        _canvasGroup.interactable = false;

        _canvasGroup.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(null);

        Cursor.visible = false;
    }
}
