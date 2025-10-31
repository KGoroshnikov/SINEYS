using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float refreshPeriod; // sec

    [System.Serializable]
    public class ItemToSpawn
    {
        public Vector2Int randAmount;
        public GameObject itemPref;
    }

    [System.Serializable]
    public class LvlTier
    {
        public List<ItemToSpawn> itemsToSpawn;
    }

    [SerializeField] private List<LvlTier> allTiers;

    [SerializeField] private int currentTier;

    [SerializeField] private List<Storage> allStorages;
    private List<GameObject> spawnedItems = new List<GameObject>();

    [SerializeField] private GameObject[] storagesPrefs;
    

    [SerializeField] private Animator animator;

    void Start()
    {
        InvokeRepeating("RefreshStorages", 3, refreshPeriod);
    }

    void RefreshStorages()
    {
        animator.SetTrigger("Swap");
    }

    // called from anim
    public void RespawnItems() // we can optimize it with pools instead of instantiate
    {
        // respawn storages
        List<Storage> newStorages = new List<Storage>();
        List<Transform> availPosesForItems = new List<Transform>();
        for (int i = 0; i < allStorages.Count; i++)
        {
            GameObject newStorate = Instantiate(storagesPrefs[Random.Range(0, storagesPrefs.Length)], allStorages[i].transform.position, allStorages[i].transform.rotation);
            newStorate.transform.SetParent(allStorages[i].transform.parent);
            Destroy(allStorages[i].gameObject);
            Storage st = newStorate.GetComponent<Storage>();
            newStorages.Add(st);
            availPosesForItems.AddRange(st.GetPositions());
        }
        allStorages.Clear();
        allStorages = newStorages;

        // destroy old items
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] == null) continue;
            Destroy(spawnedItems[i]);
        }
        spawnedItems.Clear();

        // spawn new items
        int objectsToSpawn = allTiers[currentTier].itemsToSpawn.Count;

        for (int i = 0; i < objectsToSpawn; i++)
        {
            ItemToSpawn currentItem = allTiers[currentTier].itemsToSpawn[i];
            int amountOfObject = Random.Range(currentItem.randAmount.x, currentItem.randAmount.y);
            for (int j = 0; j < amountOfObject; j++)
            {
                int idxSpawn = Random.Range(0, availPosesForItems.Count);
                GameObject spawnedObj = Instantiate(currentItem.itemPref,
                                        availPosesForItems[idxSpawn].position,
                                        Quaternion.Euler(Vector3.zero));
                spawnedObj.transform.SetParent(allStorages[0].transform.parent);
                spawnedItems.Add(spawnedObj);

                availPosesForItems.RemoveAt(idxSpawn);
            }
        }

    }
    
    // called from anim
    public void SpawnEnded()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] == null) continue;
            spawnedItems[i].transform.SetParent(null);
        }
    }
}
