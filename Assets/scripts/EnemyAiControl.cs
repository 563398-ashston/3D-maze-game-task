using UnityEngine;
using UnityEngine.AI;

public class EnemyAiControl : MonoBehaviour
{
    [SerializeField] GameObject player;

    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer, playerLayer;


    //patrol
    Vector3 destPoint;
    bool walkpointSet;
    [SerializeField] float range;
    [SerializeField] float patrolSpeed = 6;
    [SerializeField] float chaseSpeed = 10;


    //State Change 
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;
    bool playerInSight;
    bool playerInAttackRange;

    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;

        /*
        Debug.Log("Agent null: " + (agent == null));
        Debug.Log("Player null: " + (player == null));
        */
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
    }

    void Chase()
    {
        agent.speed = chaseSpeed;
        Debug.Log("Chase speed: " + agent.speed);

        agent.SetDestination(player.transform.position);
    }

    void Attack()
    {

    }

    void Patrol()
    {
        agent.speed = patrolSpeed;
        Debug.Log("Patrol speed: " + agent.speed);

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
