using UnityEngine;
using UnityEngine.AI;

public class BoxCreature : MonoBehaviour
{
    [SerializeField] private float walkRange;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    [SerializeField] private Item item;

    [SerializeField] private Vector2 hideTime;
    private bool hiding = false;

    void Start()
    {
        FindATarget();
    }

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

        if (!agent.pathPending && agent.remainingDistance <= .5f && !hiding)
        {
            item.canBeUsed = false;
            hiding = true;
            animator.SetTrigger("Hide");
            Invoke("StopHiding", Random.Range(hideTime.x, hideTime.y));
        }
    }

    void StopHiding()
    {
        item.canBeUsed = true;
        hiding = false;
        FindATarget();
        animator.SetTrigger("Walk");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkRange);
    }
}
