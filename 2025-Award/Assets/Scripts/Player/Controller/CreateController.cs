using UnityEngine;

public class CreateController : MonoBehaviour
{
    [SerializeField] private RobotFactory _robotFactory;

    // ÉvÉåÉCÉÑÅ[
    private Player _player;

    private PlayerInventory _inventory;

    /// <summary>
    /// èâä˙âª
    /// </summary>
    /// <param name="player"></param>
    public void Init(Player player)
    {
        _player = player;
        _inventory = player.Inventory;
    }

    public void CreateRobot()
    {
        if (_inventory == null) return;

        bool success = _robotFactory.Create(_inventory);

        if (success)
        {
            _inventory.ClearParts();
        }
    }
}
