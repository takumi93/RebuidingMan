using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    private Animator _animator;

    private WeaponBase _weapon;

    private BodyBase _body;

    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    public void InitBody(BodyBase body)
    {
        Init();

        _weapon = GetComponentInChildren<WeaponBase>();
        _body = body;
    }

    /// <summary>
    /// アニメーションを再生する
    /// </summary>
    /// <param name="name">再生するトリガー名</param>
    public void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }

    public void DestoryAnimation()
    {
        _animator.enabled = false;
    }

    public void OnAttackStart()
    {
        _body?.OnAttackStart();
    }

    public void HitOn()
    {
        _weapon?.HitOn();
    }

    public void HitOff()
    {
        _weapon?.HitOff();
    }

    public void OnAttackEnd()
    {
        _body?.OnAttackEnd();
    }
}
