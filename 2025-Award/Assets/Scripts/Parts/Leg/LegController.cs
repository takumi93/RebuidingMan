using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static RobotAction;

public class LegController : MonoBehaviour
{
    [Header("ì∑Ç…òAìÆÇ∑ÇÈRigÇéwíË")]
    [SerializeField] public GameObject LegToBodyRig = null;

    // ë´ÇÃèÓïÒ
    public LegData legData;

    private Animator animator;
    private RobotAction robotAction;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        robotAction = this.GetComponentInParent<RobotAction>();
        if (robotAction == null)
        {
            return;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.parent.CompareTag("Ally") || this.transform.parent.CompareTag("Enemy"))
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

    public void CreateSetup()
    {
        this.transform.GetComponentInChildren<SkinnedMeshRenderer>().material = legData.material;
    }

    public void PartsDestroy(bool isSet)
    {
        if (isSet)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    public LegData OutputData() {
        return legData;
    }
}
