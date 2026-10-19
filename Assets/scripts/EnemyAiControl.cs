using System.Security.Cryptography;
using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAiControl : MonoBehaviour
{
    GameObject player;

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;

    //patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;

    //State Change 
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    bool playerInSight;
    bool playerInAttackRange;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }


    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange)
        {
            Patrol();
        }

        if (playerInSight && !playerInAttackRange)
        {
            Chase();
        }

        if (playerInSight && playerInAttackRange)
        {
            Attack();
        }

        Patrol();
        Chase();
    }

    void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    void Attack()
    {

    }

    void Patrol()
    {
        if (!walkpointSet)
        {
            SearchForDest();
        }

        if (walkpointSet)
        {
            agent.SetDestination(destPoint);

        }

        if (Vector3.Distance(transform.position, destPoint) < 10)
        {
            walkpointSet = false;
        }
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkpointSet = true;
        }
    }
}
