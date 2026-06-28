using UnityEngine;

/// <summary>
/// 視線に使う機能
/// </summary>
public class DetectionController : MonoBehaviour
{
    // プレイヤー
    private Player _player;

    // 視線
    private Ray _ray;

    // Rayが当たったオブジェクトを格納する変数を宣言
    private RaycastHit _hit;

    // 拾える距離
    [SerializeField] private float _interactDistance = 10.0f;

    private int _itemLayer;

    [Header("オブジェクトの識別判定")]
    public bool IsEnemy {  get; private set; }

    public bool IsAlly { get; private set; }

    public bool IsItem { get; private set; }

    public void Init(Player player)
    {
        _player = player;
        _itemLayer = LayerMask.NameToLayer("Item");
    }

    /// <summary>
    /// プレイヤーで使用する更新処理をまとめたもの
    /// </summary>
    public void Tick()
    {
        SetUpViewPoint();

        Recognition();
    }

    /// <summary>
    /// 視点の先にあるオブジェクトを取得
    /// </summary>
    public RaycastHit SetUpViewPoint()
    {
        // Rayを飛ばす位置を指定（画面の中心を指定）
        _ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f,Screen.height * 0.5f));

        // Rayがオブジェクトに当たった時hitに値を入れる
        // アイテム以外にも参照できるようにLayer指定は行わない
        Physics.Raycast(_ray, out _hit, _interactDistance);

        return _hit;
    }

    /// <summary>
    /// SetUpViewPointで取得したhitにある物が敵であるか識別する
    /// その後レティクルの色を変更する
    /// </summary>
    public void Recognition()
    {
        IsEnemy = false;
        IsAlly = false;
        IsItem = false;

        if (_hit.collider == null)
        {
            _player.UI.SetReticleNormal();
            return;
        }

        TeamObject enemy = _hit.collider.GetComponentInParent<TeamObject>();

        // 敵味方の判定
        if(enemy != null)
        {
            if(enemy.GetTeamType() == TeamType.Enemy)
            {
                IsEnemy = true;
                _player.UI.SetReticleEnemy();

                return;
            }
        }

        // アイテム判定
        if(_hit.collider.gameObject.layer == _itemLayer)
        {
            IsItem = true;
            _player.UI.SetReticleItem();

            return;
        }

        _player.UI.SetReticleNormal();
    }

    /// <summary>
    /// 視点の先にあるオブジェクトを取得したものを返す
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetHit()
    {
        return _hit;
    }
}
