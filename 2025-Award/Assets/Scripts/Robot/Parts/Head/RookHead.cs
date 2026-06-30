using UnityEngine;

public class RookHead : HeadBase 
{ 
    [Header("–h‰qİ’è")]
    [SerializeField] private Transform FixedPosition; 
    // –h‰q’n“_
    [SerializeField] private float defendRadius = 15.0f; 

    // –h‰q”ÍˆÍ
    public override void Init(Robot robot) 
    {
        base.Init(robot); 
        IsPatrolling = false;
    } 
    
    public override void CreateSetup()
    { 
        UpdateMaterial(HeadData);

        // –h‰q’n“_‚Ìİ’è
        var Fix = Instantiate(
            StageScene.Instance.GuardianPoint,
            transform.position,
            Quaternion.identity,
            StageScene.Instance.GuardianTransform.transform); 
        Fix.name = "GuardianPoint";
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
            _robot.MoveTarget = FixedPosition.position;
            return; 
        }
        
        // “G‚ª–h‰q”ÍˆÍŠO‚És‚Á‚½‚Æ‚«
        float distance = Vector3.Distance(FixedPosition.position, _robot.Target.position);

        if (distance > defendRadius) 
        { 
            // ˆê’è‹——£ˆÈã—£‚ê‚½‚çƒ^[ƒQƒbƒg’ú‚ß‚é
            _robot.Target = null; 
            _robot.MoveTarget = FixedPosition.position; 
            ReturnToPosition();
            return; 
        }

        _robot.MoveTarget = _robot.Target.position;
    }

    /// <summary> 
    /// Idleó‘Ô‚É‚·‚é‚±‚Æ 
    /// õ“G’†‚É‚·‚é‚±‚Æ 
    /// </summary> 
    /// <param name="category"></param> 
    public override void TrackingTarget()
    {
        // “G”­Œ©
        if (SearchTarget()) return;

        // –h‰q’n“_‚©‚ç—£‚ê‚Ä‚¢‚½‚ç–ß‚é
        float distance = Vector3.Distance(transform.position, FixedPosition.position);

        if (distance > 2.0f)
        {
            _robot.MoveTarget = FixedPosition.position;
        }
        else
        {
            _robot.MoveTarget = null;

            _robot.transform.rotation = Quaternion.RotateTowards(
                    _robot.transform.rotation,
                    FixedPosition.rotation,
                    180f * Time.deltaTime
                );
        }
    }

    /// <summary> 
    /// –h‰q’n“_‚É–ß‚é 
    /// </summary> 
    private void ReturnToPosition() 
    {
        _robot.MoveTarget = FixedPosition.position; 
    } 
}