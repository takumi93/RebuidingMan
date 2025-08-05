using UnityEngine;

public class RobotLegMethod : MonoBehaviour
{
    private Animator animator;
    private RobotAction robotAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.CompareTag("Ally") || this.transform.parent.CompareTag("Enemy"))
        {
            robotAction = transform.parent.GetComponent<RobotAction>();
            if (robotAction.CategoryHead == 0)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                if (robotAction.robotState == RobotAction.RobotState.Idle || robotAction.robotState == RobotAction.RobotState.Attack)
                {
                    animator.SetBool("Walk", false);
                }
                else
                {
                    animator.SetBool("Walk", true);
                }
            }
        }
            
    }
}
