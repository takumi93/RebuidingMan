using UnityEngine;
public class RookHead : HeadBase 
{ 
    [Header("–h‰qİ’è")]
    [SerializeField] private Transform FixedPosition; 
    // –h‰q’n“_
    [SerializeField] private float defendRadius = 8.0f; 

    // –h‰q”ÍˆÍ
    public override void Init() {
        base.Init(); 
        IsPatrolling = false;
    } 
    
    public override void CreateSetup() { 
        UpdateMaterial();
        var Fix = Instantiate(StageScene.Instance.GuardianPoint, StageScene.Instance.GuardianTransform.transform); 
        Fix.name = "GuardianPoint";
        Fix.transform.position = StageScene.Instance.GuardianTransform.transform.position;
        FixedPosition = Fix.transform;
    }
    
    /// <summary> 
    /// “G‚ğ’Ç”ö‚·‚éˆ— 
    /// </summary> 
    public override void ChaseTarget() 
    { 
        // ƒ^[ƒQƒbƒg‚ª‚¢‚È‚¢
        if (!_robot.Target) 
        { 
            ReturnToPosition(); 
            return; 
        }
        
        // “G‚ª–h‰q”ÍˆÍŠO‚És‚Á‚½‚Æ‚«
        float distance = Vector3.Distance(FixedPosition.position, _robot.Target.position);

        if (distance > defendRadius) 
        { 
            // ˆê’è‹——£ˆÈã—£‚ê‚½‚çƒ^[ƒQƒbƒg’ú‚ß‚é
            _robot.Target = null; 
            _robot.MoveTarget = null; 
            ReturnToPosition();
            return; 
        }

        MoveToTarget(_robot.Target.position);

        // ‹AŠÒ’†
        if (_robot.MoveTarget.HasValue)
        {
            MoveToTarget(_robot.MoveTarget.Value);
        }
    }

    /// <summary> 
    /// Idleó‘Ô‚É‚·‚é‚±‚Æ 
    /// õ“G’†‚É‚·‚é‚±‚Æ 
    /// </summary> 
    /// <param name="category"></param> 
    public override void TrackingTarget()
    {
        // “G”­Œ©
        if (SearchTarget())
        {
            return;
        }

        // –h‰q’n“_‚©‚ç—£‚ê‚Ä‚¢‚½‚ç–ß‚é
        float distance =
            Vector3.Distance(transform.position,
                             FixedPosition.position);

        if (distance > 1.0f)
        {
            _robot.MoveTarget = FixedPosition.position;

            _robot.ChangeState(_robot.StateManager.WalkState);
        }
    }

    /// <summary> 
    /// –h‰q’n“_‚É–ß‚é 
    /// </summary> 
    private void ReturnToPosition() 
    { 
        _area.isStopped = false; 
        _area.destination = FixedPosition.position; 
    } 
}