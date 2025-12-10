using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private InputInfo _inputInfo;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _inputInfo = new InputInfo();
    }

    void Update()
    {
        _player.Tick(_inputInfo);
        _player.Move(_inputInfo);
        InputClear();
    }

    private void FixedUpdate()
    {
        // これだけ移動処理のためFixedUpdateにて処理
        //_player.Move(_inputInfo);
    }

    private void InputClear()
    {
        _inputInfo.IsAttack = false;
        _inputInfo.Interact = false;
        _inputInfo.Create = false;
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// 移動の入力があった際に値を取得する
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<Vector2>();

        // 入力は常に読み込む
        _inputInfo.Move = new Vector3(value.x, 0.0f, value.y);

        // 歩いているかどうかは magnitude で判断
        _inputInfo.IsWalk = _inputInfo.Move.sqrMagnitude > 0.01f;
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
        if(context.started)
        {
            _inputInfo.IsAttack = true;
        }
    }

    /// <summary>
    /// InputSystemに登録する用の関数
    /// キャラのアクションが起きたときに値をTrueに変更する
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _inputInfo.Interact = true;
        }
        if (context.canceled)
        {
            _inputInfo.Interact = false;
        }
    }

    public void OnCreate(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _inputInfo.Create = true;
            return;
        }
        _inputInfo.Create = false;
    }

    public void OnDiscard(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }
}
