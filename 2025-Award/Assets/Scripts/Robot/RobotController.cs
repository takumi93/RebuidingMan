using UnityEngine;

public class RobotController : MonoBehaviour
{
    private Robot _robot;
    
    void Awake()
    {
        _robot = GetComponent<Robot>();
    }

    void Update()
    {
        _robot.Tick();
    }
}
