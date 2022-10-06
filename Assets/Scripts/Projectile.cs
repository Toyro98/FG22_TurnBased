using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class Projectile : MonoBehaviour
{
    public float charge;
    public float radius = 3f;
    public ProjectileWeapon projectile;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private int _damage;

    private void Start()
    {
        _damage = Random.Range(5, 16);

        if (projectile == ProjectileWeapon.Granade)
        {
            _damage *= 2;
            _rigidbody.AddForce(charge * 2f * transform.forward, ForceMode.VelocityChange);
        }
  
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (projectile == ProjectileWeapon.Rocket)
        {
            _rigidbody.AddForce(transform.forward, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out var player))
            {
                player.TakeDamage(_damage);
            }
        }

        Destroy(gameObject);
    }
}

public enum ProjectileWeapon
{
    Rocket,
    Granade
}
