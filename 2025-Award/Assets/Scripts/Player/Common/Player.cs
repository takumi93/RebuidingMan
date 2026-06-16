using UnityEngine;

public class Player : MonoBehaviour
{
    // ステートマシン
    private PlayerStateManager _stateManager;

    public PlayerInputInfo PlayerInputInfo {  get; private set; }

    // 最後に攻撃してきた敵
    public GameObject LastAttacker { get; set; }

    [Header("コントローラー")]
    public AttackController Attack {  get; private set; }

    public MoveController Move { get; private set; }

    public InteractController Interact { get; private set; }

    public CreateController Create { get; private set; }

    public DetectionController Detection { get; private set; }

    public PlayerAnimation Animation { get; private set; }

    public PlayerHP HpManager { get; private set; }

    public AudioController Sound { get; private set; }

    public PlayerInventory Inventory { get; private set; }

    private void Awake()
    {
        // コントローラー登録
        Detection = GetComponent<DetectionController>();
        Attack = GetComponent<AttackController>();
        Move = GetComponent<MoveController>();
        Interact = GetComponent<InteractController>();
        Create = GetComponent<CreateController>();
        Sound = GetComponent<AudioController>();

        Animation = GetComponentInChildren<PlayerAnimation>(true);

        Inventory = GetComponent<PlayerInventory>();

        Move.Init();
        Interact.Init(this);
        Create.Init(this);
    }

    private void Start()
    {
        PlayerInputInfo = InputManager.Instance.playerInput;

        _stateManager = new PlayerStateManager(this);
    }

    /// <summary>
    /// PlayerControllerに送るUpdateの処理
    /// </summary>
    public void Tick()
    {
        // 操作対象がプレイヤー以外は無視
        if (!InputManager.Instance.IsPlayerInput) return;

        _stateManager.CurrentState.Update();
        Move.Look();
        Detection.Tick();
    }

    /// <summary>
    /// ラッパー関数として登録
    /// プレイヤーの状態遷移をする処理
    /// </summary>
    /// <param name="newstate"></param>
    public void ChangeState(PlayerState newstate)
    {
        _stateManager.ChangeState(newstate);
    }
}
