using System.Collections.Generic;
using UnityEngine;

public class LightersSpawn : MonoBehaviour
{
    [SerializeField] private Vector3 boxSizes;
    [SerializeField] private GameObject lighterPref;
    [SerializeField] private int amount;

    class Lighter
    {
        public Lighter(float _spd, GameObject _obj)
        {
            speed = _spd;
            obj = _obj;
        }
        public float speed;
        public Vector3 target = Vector3.zero;
        public GameObject obj;
    }

    private List<Lighter> currentSpawned = new List<Lighter>();

    [SerializeField] private Vector2 speed;

    void Start()
    {
        RefreshLighters();
    }

    Vector3 GetRandomPosInBox()
    {
        Vector3 rPos = new Vector3(Random.Range(-boxSizes.x, boxSizes.x), Random.Range(-boxSizes.y, boxSizes.y), Random.Range(-boxSizes.z, boxSizes.z)) / 2;
        return transform.position + rPos;
    }

    public void LigherGotCaught(GameObject lighter)
    {
        for (int i = 0; i < currentSpawned.Count; i++)
        {
            if (currentSpawned[i].obj == lighter)
            {
                currentSpawned.RemoveAt(i);
                break;
            }
        }
        RefreshLighters();
    }

    void RefreshLighters()
    {
        int needSpawn = amount - currentSpawned.Count;
        if (needSpawn < 0) needSpawn = 0;

        for (int i = 0; i < needSpawn; i++)
        {
            GameObject newLight = Instantiate(lighterPref, GetRandomPosInBox(), Quaternion.Euler(Vector3.zero));
            newLight.GetComponent<Animator>().SetFloat("IdleSpeed", Random.Range(0.7f, 1.3f));
            currentSpawned.Add(new Lighter(Random.Range(speed.x, speed.y), newLight));
            newLight.GetComponent<Item>().onPickup.AddListener(LigherGotCaught);
        }
    }

    void FixedUpdate()
    {
        for(int i = 0; i < currentSpawned.Count; i++)
        {
            if (currentSpawned == null) continue;
            if (Vector3.Distance(currentSpawned[i].obj.transform.position, currentSpawned[i].target) <= 0.1f || currentSpawned[i].target == Vector3.zero)
                currentSpawned[i].target = GetRandomPosInBox();

            currentSpawned[i].obj.transform.position = Vector3.MoveTowards(
                    currentSpawned[i].obj.transform.position, currentSpawned[i].target, currentSpawned[i].speed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSizes);
    }
}
