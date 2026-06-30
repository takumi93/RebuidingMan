using UnityEngine;

public class RobotFactory : MonoBehaviour
{
    [SerializeField] private GameObject _robotBasePrefab;
    [SerializeField] private Transform _robotParent;

    private RobotAssembler robotAssembler;

    private void Awake()
    {
        robotAssembler = GetComponent<RobotAssembler>();
    }

    /// <summary>
    /// ロボットが生成可能かを判定する
    /// 生成可能なら生成する
    /// </summary>
    /// <param name="inventory">インベントリ</param>
    /// <returns></returns>
    public bool Create(PlayerInventory inventory)
    {
        PartsData head = inventory.GetPartOfType(PartsType.Head);
        PartsData body = inventory.GetPartOfType(PartsType.Body);
        PartsData leg = inventory.GetPartOfType(PartsType.Leg);

        if(head == null || body == null || leg == null) return false;

        GameObject robot = Instantiate(
            _robotBasePrefab,
            transform.position + transform.forward,
            transform.rotation,
            _robotParent
            );

        robot.name = "Robot";

        Robot robotComponent = robot.GetComponent<Robot>();

        robotAssembler.SetParts(head, body, leg, robot.transform);

        return true;
    }
}
