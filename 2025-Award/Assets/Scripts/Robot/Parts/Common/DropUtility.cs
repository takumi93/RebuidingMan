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

        if(!obj.TryGetComponent<ItemObject>(out var _))
        {
            obj.AddComponent<ItemObject>();
        }
    }
}
