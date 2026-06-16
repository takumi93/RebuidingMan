using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _player.Tick();
    }

    private void FixedUpdate()
    {
        _player.Move.Move();
    }
}
