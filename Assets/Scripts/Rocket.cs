using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 25, ForceMode.Impulse);    
        Destroy(gameObject, 5f);
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
