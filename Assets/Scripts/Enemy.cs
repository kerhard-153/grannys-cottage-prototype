using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    [SerializeField] float damage = 10f;

    private IObjectPool<Enemy> enemyPool;

    private bool isDead = false;

    public void SetPool(IObjectPool<Enemy> pool)
    {
        enemyPool = pool;
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
       {    
           navMeshAgent.SetDestination(player.position);
       }
    }

    public void HandleHit(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (isDead) return;

        isDead = true;

        if (Spawner.Instance != null)
            Spawner.Instance.aliveEnemies--;

        if (enemyPool != null)
            enemyPool.Release(this);
        else
            gameObject.SetActive(false);
    }

    public void ResetEnemy()
    {
        isDead = false;
    }
}
