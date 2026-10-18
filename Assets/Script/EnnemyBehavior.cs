using UnityEngine;
using UnityEngine.AI;

public class EnnemyBehavior : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    public GameManager manager;
    public GameObject hitSFX;
    public GameObject ShootSFX;
    public GameObject deathSFX;
    public int currentHealth;
    public int maxHealth = 3;
    private float shootTimer = 0f;
    public bool isFleeing = false;
    [Header("Waypoints")]
    public Transform[] waypoints;  // Assign in Inspector
    private int currentWaypointIndex = 0;

    [Header("AI Settings")]
    [SerializeField] private bool alarmed = false;
    public bool Alarmed
    {
        get { return alarmed; }
        set { alarmed = value; }
    }
    [Header("Shoot Settings")]

    public Transform shootPoint;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;

    [Header("Chase Settings")]
    public float stopDistance = 3f; // How close the enemy stops to the player

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Start patrolling if waypoints are set
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if(manager.treasureBoxesCollected >= 3){
            isFleeing = true;
            alarmed = false;
        }
        if (alarmed == true && isFleeing == false)
        {
            ChasePlayer();
        }
        if(alarmed == false && isFleeing == false)
        {
            Patrol();
        }
        if( isFleeing == true)
        {
            Flee();
        }
        Death();
    }

    void OnDestroy()
    {
        if (manager != null)
        {
            GameManager gameManager = manager.GetComponent<GameManager>();
            if(manager != null)
            {
            manager.ObjectDestroyed();
            }
        }
    }
    void Death()
    {
        if (currentHealth <= 0)
            {
                GameObject deathInstance = Instantiate(deathSFX, transform.position, Quaternion.identity);
                Destroy(deathInstance, 2f);
                Destroy(this.gameObject);
                // Add death handling logic here (e.g., respawn, game over screen)
            }
    }

  void ChasePlayer()
{
    float distance = Vector3.Distance(transform.position, target.position);

    // Handle shooting with interval
    shootTimer -= Time.deltaTime;
    if (shootTimer <= 0f && projectilePrefab != null && shootPoint != null)
        {
            
        
        Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        GameObject shootAudioInstance = Instantiate(ShootSFX, shootPoint.position, shootPoint.rotation);
        Destroy(shootAudioInstance, 1f);
        shootTimer = shootInterval; // reset timer
    }

    if (distance > stopDistance)
    {
        // Move toward player
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }
    else
    {
        // Stop moving and look at player
        agent.isStopped = true;

        Vector3 lookDirection = (target.position - transform.position);
        lookDirection.y = 0; // Prevent tilting up/down
        if (lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        agent.isStopped = false;

        // If close to the current waypoint, move to the next one
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    public void Flee()
{
    if (target == null) return;

    
    Vector3 fleeDirection = transform.position - target.position;
    fleeDirection.y = 0; // Keep movement horizontal

    
    
    Vector3 fleeTarget = transform.position + fleeDirection.normalized;

    
    if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, 5f, NavMesh.AllAreas))
    {
        agent.SetDestination(hit.position);
        agent.isStopped = false;
    }  

    
    Quaternion lookRotation = Quaternion.LookRotation(fleeDirection);
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    Debug.Log("Fleeing from player!");
}

   void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            GameObject hitInstance = Instantiate(hitSFX, collision.contacts[0].point, Quaternion.identity);
            Destroy(hitInstance, 1f);
            currentHealth -= 1;
            Debug.Log("Player hit! Current Health: " + currentHealth);
            

            
        }
         if(collision.gameObject.CompareTag("Grenade"))
        {
            Debug.Log("Grenade explosion range.");
            currentHealth = 0;
        }
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alarmed = true;
            Debug.Log("Player detected! Chasing...");
        }
    }
}
