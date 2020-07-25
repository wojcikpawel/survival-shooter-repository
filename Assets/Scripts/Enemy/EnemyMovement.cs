using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    // This element is not "public" because the enemies are instantiated during gameplay, so the reference to the player needs to be established at runtime.
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent>();
    }


    void Update ()
    {
        // Update() is being used here because the enemies are being controlled using NavMesh and not physics.
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            nav.SetDestination (player.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
