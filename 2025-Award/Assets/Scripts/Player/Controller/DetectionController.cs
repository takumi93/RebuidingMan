using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 視線に使う機能
/// </summary>
public class DetectionController : MonoBehaviour
{
    //HitReticleを指定します。
    [SerializeField]
    HitReticle hitReticle = null;

    [SerializeField] Image Reticle = null;

    // 視線
    private Ray _ray;

    // Rayが当たったオブジェクトを格納する変数を宣言
    private RaycastHit _hit;

    // 拾える距離
    [SerializeField] private float _interactDistance = 10.0f;

    [Header("オブジェクトの識別判定")]
    public bool IsEnemy {  get; private set; }

    public bool IsAlly { get; private set; }

    public bool IsItem { get; private set; }

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
        // Rayを飛ばす位置を指定（メインカメラを指定）
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

        if (_hit.collider == null) return;

        TeamObject enemy = _hit.collider.GetComponentInParent<TeamObject>();

        // 敵味方の判定
        if(enemy != null)
        {
            if(enemy.GetTeamType() == TeamType.Enemy)
            {
                IsEnemy = true;
                //Reticle.color = Color.red;
            }
            else
            {
                IsEnemy = false;
            }

        }

        //Reticle.color = Color.white;

        // アイテム判定
        if(_hit.collider.GetComponentInParent<ItemObject>() != null)
        {
            IsItem = true;
        }
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
