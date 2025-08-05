using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CinemachineFollow follow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        follow = GetComponent<CinemachineFollow>();
    }

    public IEnumerator CameraShake(int time, float speed = 0.01f,float minRange = -0.2f,float maxRange = 0.2f)
    {
        for (int i = 0; i <= time; i++)
        {
            float x = Random.Range(minRange,maxRange);
            float y = Random.Range(minRange,maxRange);

            var position = follow.FollowOffset;

            follow.FollowOffset.x += x;
            follow.FollowOffset.y += y;
            yield return new WaitForSeconds(speed);

            follow.FollowOffset = position;

        }
    }
}
