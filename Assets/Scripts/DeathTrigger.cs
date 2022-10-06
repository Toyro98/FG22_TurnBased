using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            player.playerManager.RemovePlayer(player.index);
            Destroy(player.gameObject);
        }
    }
}
