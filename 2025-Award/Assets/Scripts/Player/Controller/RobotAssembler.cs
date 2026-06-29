using UnityEngine;

public class RobotAssembler : MonoBehaviour
{
    /// <summary>
    /// ロボットの部品を生成して指定したオブジェクトに入れる
    /// </summary>
    /// <param name="head"></param>
    /// <param name="body"></param>
    /// <param name="leg"></param>
    /// <param name="parent"></param>
    public void SetParts(PartsData head, PartsData body, PartsData leg, Transform parent)
    {
        var headObj = CreatePart(head.Prefab, parent, head.PartName);
        var bodyObj = CreatePart(body.Prefab, parent, body.PartName);
        var legObj = CreatePart(leg.Prefab, parent, leg.PartName);

        headObj.GetComponent<HeadBase>().SetData((HeadData)head);
        bodyObj.GetComponent<BodyBase>().SetData((BodyData)body);
        legObj.GetComponent<LegBase>().SetData((LegData)leg);

        headObj.GetComponent<HeadBase>().CreateSetup() ;
        bodyObj.GetComponent <BodyBase>().CreateSetup();
        legObj.GetComponent <LegBase>().CreateSetup();
    }

    /// <summary>
    /// 部品を生成する
    /// </summary>
    /// <param name="prefab">生成する部品</param>
    /// <param name="slot">生成する場所</param>
    /// <param name="partName">部品の名前</param>
    /// <returns>生成した部品</returns>
    private GameObject CreatePart(GameObject prefab, Transform slot, string partName)
    {
        if (prefab == null) return null;

        GameObject obj = Instantiate(prefab, slot);

        obj.name = partName;

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        return obj;
    }
}
