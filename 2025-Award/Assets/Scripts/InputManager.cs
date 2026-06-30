using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }

    public PlayerInputInfo playerInput {  get; private set; }

    public UIInputInfo uiInput { get; private set; }

    [SerializeField] private CameraController _cameraController = null;

    [Header("InputSystem")]
    // InputSystem
    [SerializeField] private PlayerInput _playerInputSystem;

    // プレイヤーのActionMap
    private InputActionMap _playerMap;

    // UIのActionMap
    private InputActionMap _uiMap;

    // 操作対象がプレイヤーかを判定
    public bool IsPlayerInput { get; private set; }

    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInputInfo();

        uiInput = new UIInputInfo();

        _playerMap = _playerInputSystem.actions.FindActionMap("Player");
        _uiMap = _playerInputSystem.actions.FindActionMap("UI");

    }

    /// <summary>
    /// プレイヤーの操作を禁止する
    /// </summary>
    public void DisablePlayerInput()
    {
        _playerMap.Disable();

        _cameraController.DisableCameraInput();

        playerInput.Move = Vector3.zero;
        playerInput.Look = Vector3.zero;

        IsPlayerInput = false;
    }

    /// <summary>
    /// 操作をプレイヤーに切り替える
    /// </summary>
    public void EnablePlayerInput()
    {
        //_uiMap.Disable();

        //_playerMap.Enable();

        _playerInputSystem.SwitchCurrentActionMap("Player");

        IsPlayerInput = true;

        _cameraController.EnableCameraInput();

        playerInput.Move = Vector3.zero;
        playerInput.Look = Vector3.zero;
    }

    /// <summary>
    /// 操作をUIに切り替える
    /// </summary>
    public void EnableUIInput()
    {
        //_playerMap.Disable();

        //_uiMap.Enable();

        _playerInputSystem.SwitchCurrentActionMap("UI");

        Debug.Log($"PlayerMap:{_playerMap.enabled}");
        Debug.Log($"UIMap:{_uiMap.enabled}");
        Debug.Log($"Current:{_playerInputSystem.currentActionMap?.name}");

        IsPlayerInput = false;

        _cameraController?.DisableCameraInput();
    }
}
