using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineFollow _follow;

    private CinemachineInputAxisController _inputAxis;

    private void Awake()
    {
        _follow = GetComponent<CinemachineFollow>();
        _inputAxis = GetComponent<CinemachineInputAxisController>();
    }

    /// <summary>
    /// ƒJƒƒ‰‚Ì“ü—Í‚ğON‚É‚·‚é
    /// </summary>
    public void EnableCameraInput()
    {
        _inputAxis.enabled = true;
    }

    /// <summary>
    /// ƒJƒƒ‰‚Ì“ü—Í‚ğOFF‚É‚·‚é
    /// </summary>
    public void DisableCameraInput()
    {
        _inputAxis.enabled = false;
    }

    /// <summary>
    /// ƒJƒƒ‰‚ğ—h‚ç‚·ˆ—
    /// </summary>
    /// <param name="time"></param>
    /// <param name="speed"></param>
    /// <param name="minRange"></param>
    /// <param name="maxRange"></param>
    /// <returns></returns>
    public IEnumerator CameraShake(int time, float speed = 0.01f,float minRange = -0.2f,float maxRange = 0.2f)
    {
        for (int i = 0; i <= time; i++)
        {
            float x = Random.Range(minRange,maxRange);
            float y = Random.Range(minRange,maxRange);

            var position = _follow.FollowOffset;

            _follow.FollowOffset.x += x;
            _follow.FollowOffset.y += y;
            yield return new WaitForSeconds(speed);

            _follow.FollowOffset = position;

        }
    }
}
