using UnityEngine;

public class MoveController : MonoBehaviour
{
    // リジッドボディ
    private Rigidbody _rb;

    //プレイヤーの移動速度を指定します
    [SerializeField]
    private float _moveSpeed;

    public void Init()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// プレイヤーの移動
    /// カメラの移動はCinemachineを使用しているため不要
    /// </summary>
    public void Move()
    {
        Vector3 move = transform.TransformDirection(InputManager.Instance.playerInput.Move.normalized);

        // 等速度運動
        var velocity = _rb.linearVelocity;

        // 速度調整
        velocity.x = move.x * _moveSpeed;
        velocity.z = move.z * _moveSpeed;

        // Rigidbodyに速度を反映
        _rb.linearVelocity = velocity;
    }

    /// <summary>
    /// 視点移動（Cinemachineを使ってるためプレイヤーの回転のみ）
    /// </summary>
    public void Look()
    {
        Quaternion cameraRtation = Camera.main.transform.rotation;
        //プレイヤーを回転
        transform.rotation = new Quaternion(0, cameraRtation.y, 0, cameraRtation.w);
    }
}
