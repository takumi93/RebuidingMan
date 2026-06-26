using UnityEngine;

public class KnightHead : HeadBase
{
    [Header("Œì‰qAI‚Ìİ’è")]
    // Œì‰q‘ÎÛ
    [SerializeField] private GameObject _escortTarget;

    // ’Êí‚ÌŒì‰q‹——£
    [SerializeField] private float _escortDistance = 3.0f; 
    
    // “G‘Î‚ÌŒì‰q‚©‚ç—£‚ê‚ç‚ê‚éÅ‘å‹——£
    [SerializeField] private float _maxProtectDistance = 10f;

    public override void Init() 
    { 
        base.Init(); 
    }

    public override void CreateSetup() { 
        FindEscortTarget();
        UpdateMaterial();
    }

    public override void ChaseTarget() 
    { 
        if (!_robot.MoveTarget.HasValue) return; 
        MoveToTarget(_robot.MoveTarget.Value); 
    }

    /// <summary> 
    /// Idleó‘Ô‚É‚·‚é‚±‚Æ 
    /// õ“G’†‚É‚·‚é‚±‚Æ 
    /// “G‚Ì‚ÍŒì‰q‘ÎÛ‚ğ’T‚µˆê”Ô‹ß‚¢“G‚ğŒì‰q‘ÎÛ‚Æ‚·‚é 
    /// –¡•û‚Ì‚ÍƒvƒŒƒCƒ„[‚ğŒì‰q‘ÎÛ‚Æ‚·‚é 
    /// </summary> 
    public override void TrackingTarget() 
    {
        // Œì‰q‘ÎÛ‚ª‚¢‚È‚¢‚Æ‚«
        if (_escortTarget == null) 
        { 
            FindEscortTarget();
            return;
        } 
        
        Robot escortRobot = _escortTarget.GetComponent<Robot>(); 
        
        // Œì‰q‘ÎÛ‚ªÅŒã‚ÉUŒ‚‚µ‚½“G‚ª‚¢‚½‚Æ‚«
        if (escortRobot?.LastAttacker != null)
        {
            ProtectEscort(escortRobot.LastAttacker); 
            return; 
        } 
        FollowEscort(); 
    } 
    
    private void ProtectEscort(Robot attacker) 
    { 
        if (attacker == null) 
        { 
            FollowEscort();
            return; 
        } 
        
        float escortDistance = Vector3.Distance(transform.position, _escortTarget.transform.position);
        
        if (escortDistance > _maxProtectDistance) 
        { 
            _robot.MoveTarget = null;
            FollowEscort();
            return;
        } 
        
        _robot.MoveTarget = _robot.LastAttacker.transform.position; 
        _robot.ChangeState(_robot.StateManager.WalkState); 
    } 
    
    /// <summary> 
    /// Œì‰q‘ÎÛ‚ğ’Ç”ö 
    /// </summary> 
    private void FollowEscort() 
    { 
        if (_escortTarget == null) return; 
        float distance = Vector3.Distance(transform.position, _escortTarget.transform.position); 
        // ‰“‚¢‚È‚ç’Ç”ö
        if(distance > _escortDistance) 
        { 
            _robot.MoveTarget = _escortTarget.transform.position; 
            _robot.ChangeState(_robot.StateManager.WalkState); 
        } 
        else 
        { 
            _robot.MoveTarget = null;
            _robot.ChangeState(_robot.StateManager.IdleState);
        } 
    } 
    
    /// <summary>
    /// Œì‰q‘ÎÛ‚ğ’T‚· 
    /// </summary> 
    private void FindEscortTarget()
    { 
        // w‰c‚ªƒvƒŒƒCƒ„[w‰c‚Ì
        if(_robot.TeamType == TeamType.Player) 
        { 
            GameObject player = GameObject.FindGameObjectWithTag("Player"); 
            if (player != null) 
            { 
                _escortTarget = player; 
            } 
        } 
        else 
        { 
            Robot ally = RobotManager.Instance.GetNearestAlly(_robot);
            if (ally != null) 
            { 
                _escortTarget = ally.gameObject;
            } 
        } 
    } 
}
