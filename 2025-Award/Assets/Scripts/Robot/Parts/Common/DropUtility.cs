using UnityEngine;

public static class DropUtility
{
    /// <summary>
    /// ロボットの部品をアイテム化する時の処理
    /// </summary>
    /// <param name="obj"></param>
    public static void SetupDroppedPart(GameObject obj)
    {
        if(obj.TryGetComponent<Animator>(out var animator))
        {
            animator.enabled = false;
        }

        if(obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        int itemLayer = LayerMask.NameToLayer("Item");

        SetLayerRecursively(obj, itemLayer);
    }

    /// <summary>
    /// オブジェクトのレイヤーを変更する
    /// </summary>
    /// <param name="obj">変更するオブジェクト</param>
    /// <param name="layer">変更先レイヤー</param>
    private static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
