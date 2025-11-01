using UnityEngine;
using UnityEngine.AI;

public class Bug : MonoBehaviour
{
    [SerializeField] private float walkRange;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    void FindATarget()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-walkRange, walkRange),
                    transform.position.y,
                    transform.position.z + Random.Range(-walkRange, walkRange));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 0.5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }   

    public void DisableBug()
    {
        animator.enabled = false;
        agent.enabled = false;
    }

    void FixedUpdate()
    {
        if (!agent.enabled) return;

        if (!agent.pathPending && agent.remainingDistance <= .5f) FindATarget();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkRange);
    }
}
