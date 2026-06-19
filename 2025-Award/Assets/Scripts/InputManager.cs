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

    // ƒvƒŒƒCƒ„[‚ÌActionMap
    private InputActionMap _playerMap;

    // UI‚ÌActionMap
    private InputActionMap _uiMap;

    // ‘€ì‘ÎÛ‚ªƒvƒŒƒCƒ„[‚©‚ğ”»’è
    public bool IsPlayerInput { get; private set; }

    private void Awake()
    {
        Instance = this;

        playerInput = new PlayerInputInfo();

        uiInput = new UIInputInfo();

        _playerMap = _playerInputSystem.actions.FindActionMap("Player");
        _uiMap = _playerInputSystem.actions.FindActionMap("UI");

    }

    private void Start()
    {
        //// inputSystem‚É“o˜^‚µ‚Ä‚¢‚éPlayer‚ğ’T‚·
        //_playerMap = _playerInputSystem.actions.FindActionMap("Player");

        //// inputSystem‚É“o˜^‚µ‚Ä‚¢‚éUI‚ğ’T‚·
        //_uiMap = _playerInputSystem.actions.FindActionMap("UI");
    }

    /// <summary>
    /// ‘€ì‚ğƒvƒŒƒCƒ„[‚ÉØ‚è‘Ö‚¦‚é
    /// </summary>
    public void EnablePlayerInput()
    {
        _uiMap.Disable();

        _playerMap.Enable();

        IsPlayerInput = true;

        _cameraController.EnableCameraInput();

        playerInput.Move = Vector3.zero;
        playerInput.Look = Vector3.zero;
    }

    /// <summary>
    /// ‘€ì‚ğUI‚ÉØ‚è‘Ö‚¦‚é
    /// </summary>
    public void EnableUIInput()
    {
        _playerMap.Disable();

        _uiMap.Enable();

        IsPlayerInput = false;

        _cameraController.DisableCameraInput();
    }
}
