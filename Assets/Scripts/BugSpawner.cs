using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BugSpawner : MonoBehaviour
{
    private List<GameObject> spawnedBugs = new List<GameObject>();

    [SerializeField] private GameObject bugPref;
    [SerializeField] private float spawnRange;
    [SerializeField] private int targetAmount;

    void Start()
    {
        CheckBugs();
    }

    Vector3 FindAPos()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos = new Vector3(transform.position.x + Random.Range(-spawnRange, spawnRange),
                    transform.position.y,
                    transform.position.z + Random.Range(-spawnRange, spawnRange));
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, 0.5f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }   

    public void BugGotCaught(GameObject bug)
    {
        for (int i = 0; i < spawnedBugs.Count; i++)
        {
            if (spawnedBugs[i] == bug)
            {
                spawnedBugs.RemoveAt(i);
                break;
            }
        }
        CheckBugs();
    }

    void CheckBugs()
    {
        int needSpawn = targetAmount - spawnedBugs.Count;
        if (needSpawn < 0) needSpawn = 0;

        for (int i = 0; i < needSpawn; i++)
        {
            GameObject obj = Instantiate(bugPref, FindAPos(), Quaternion.Euler(Vector3.zero));
            spawnedBugs.Add(obj);
            obj.GetComponent<Item>().onPickup.AddListener(BugGotCaught);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
}
