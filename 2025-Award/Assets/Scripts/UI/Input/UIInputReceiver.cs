using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputReceiver : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private BaseUI _defaultUI = null;

    private UIInputInfo _inputinfo;

    private void Start()
    {
        _inputinfo = _inputManager.uiInput;
    }

    private void LateUpdate()
    {
        // updateの最後に入力をリセットするためLateUpdateに追加
        _inputinfo.Submit = false;
        _inputinfo.Cancel = false;
    }

    /// <summary>
    /// UI移動入力
    /// </summary>
    /// <param name="context"></param>
    public void OnNavigate(InputAction.CallbackContext context)
    {
        _inputinfo.Navigate = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 決定入力
    /// </summary>
    /// <param name="context"></param>
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _inputinfo.Submit = true;
        }
    }

    /// <summary>
    /// 戻る入力
    /// </summary>
    /// <param name="context"></param>
    public void OnCancel(InputAction.CallbackContext context)
    {
        // ボタンを押したとき以外は無視
        if (!context.started) return;

        _inputinfo.Cancel = true;

        BaseUI current = UIManager.Instance.CurrentUI();

        // UIを開いていないとき
        if (current == null)
        {
            UIManager.Instance.Push(_defaultUI);
            return;
        }

        // Cancelで閉じられないUI
        if (!current.CanCloseByChancel)
        {
            return;
        }

        UIManager.Instance.Pop();
    }
}
