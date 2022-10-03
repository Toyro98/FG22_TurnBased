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

    private void Start()
    {
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
        bool playerTookDamage = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IDamageable>(out var test))
            {
                playerTookDamage = true;
                Debug.Log("Distance:" + Vector3.Distance(transform.position, collider.transform.position));
                test.TakeDamage(Random.Range(5, 16));
            }
        }

        if (!playerTookDamage)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.UpdateGameState(GameState.PlayerSwitch);
    }
}

public enum ProjectileWeapon
{
    Rocket,
    Granade
}
