using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float charge;
    public float radius = 3f;
    public ProjectileWeapon projectile;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();    

        if (projectile == ProjectileWeapon.Granade)
        {
            _rigidbody.AddForce(5f * charge * transform.forward, ForceMode.Impulse);
        }
        else
        {
            _rigidbody.AddForce(transform.forward, ForceMode.Impulse);
        }
  
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        if (projectile == ProjectileWeapon.Rocket)
        {
            _rigidbody.AddForce(transform.forward * 2, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}

public enum ProjectileWeapon
{
    Rocket,
    Granade
}
