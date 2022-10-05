using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class Projectile : MonoBehaviour
{
    public float charge;
    public float radius = 3f;
    public ProjectileWeapon projectile;
    public Rigidbody rb;
    private int _damage;

    private void Start()
    {
        _damage = Random.Range(5, 16);

        if (projectile == ProjectileWeapon.Granade)
        {
            rb.AddForce(charge * transform.forward, ForceMode.Impulse);
        }
  
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (projectile == ProjectileWeapon.Rocket)
        {
            rb.AddForce(transform.forward, ForceMode.Impulse);
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
