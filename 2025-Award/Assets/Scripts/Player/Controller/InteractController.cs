using UnityEngine;

public class InteractController : MonoBehaviour
{
    // プレイヤー
    private Player _player;

    public void Init(Player player)
    {
        _player = player;
    }

    /// <summary>
    /// インタラクトの処理
    /// </summary>
    public void Interact(RaycastHit hit)
    {
        // 識別したオブジェクトがアイテム以外なら無視
        if (!_player.Detection.IsItem) return;

        // インベントリに追加する
        if (_player.Inventory.AddPart(hit.transform.gameObject))
        {
            Destroy(hit.transform.gameObject); // 拾ったら消す
            _player.Sound.PlayGrab();
        }
    }
}
