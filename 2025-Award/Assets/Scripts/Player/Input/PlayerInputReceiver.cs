using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReceiver : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager = null;

    private PlayerInputInfo _inputInfo;

    [SerializeField] private BaseUI _defaultUI = null;

    private void Start()
    {
        _inputInfo = _inputManager.playerInput;
    }

    private void LateUpdate()
    {
        // updateの最後に入力をリセットするためLateUpdateに追加
        _inputInfo.IsAttack = false;
        _inputInfo.Interact = false;
        _inputInfo.Create = false;
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// 移動の入力があった際に値を取得する
    /// </summary>
    /// <param name="context">コントローラーまたはキーの入力値</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            // 入力がなくなったとき（スティックやキーを離した）
            _inputInfo.Move = Vector3.zero;
            _inputInfo.IsMoving = false;
            return;
        }

        var value = context.ReadValue<Vector2>();
        _inputInfo.Move = new Vector3(value.x, 0.0f, value.y);

        // 歩いているかどうかは magnitude で判断
        _inputInfo.IsMoving = _inputInfo.Move.sqrMagnitude > 0.01f;
    }

    /// <summary>
    /// Inputsystemに登録する用の関数
    /// 視点移動の入力があった際に値を取得する
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();
        _inputInfo.Look = new Vector3(value.x, value.y, 0.0f);
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// 攻撃の入力があった際にbool値を変更する
    /// </summary>
    /// <param name="context"></param>
    public void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log($"Fire : {context.phase}");
        if (context.started)
        {
            Debug.Log("Attack Start");
            _inputInfo.IsAttack = true;
        }
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// キャラのアクションが起きたときに値をTrueに変更する
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _inputInfo.Interact = true;
        }
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// キャラのアクションが起きたときに値をTrueに変更する
    /// </summary>
    /// <param name="context"></param>
    public void OnCreate(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _inputInfo.Create = true;
        }
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// Pauseにするアクションが起きたときに実行する
    /// </summary>
    /// <param name="context"></param>
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        if(UIManager.Instance.CurrentUI() == null)
        {
            UIManager.Instance.Push(_defaultUI);

            return;
        }
    }

    public void OnDiscard(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }
}
