using UnityEngine;

public class Bullet : WeaponBase
{
    [SerializeField] private float _speed = 20f;

    /// <summary>
    /// èâä˙âª
    /// </summary>
    public override void Init(Robot owner)
    {
        _owner = owner;
        _body = owner.Body;
        _teamObject = owner.GetComponent<TeamObject>();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit(other);

        Destroy(gameObject);
    }
}