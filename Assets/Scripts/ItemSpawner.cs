using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ItemToSpawn
    {
        public Vector2 randAmount;
        public GameObject itemPref;
    }

    [System.Serializable]
    public class LvlTier
    {
        public List<ItemToSpawn> itemsToSpawn;
    }

    [SerializeField] private List<LvlTier> allTiers;

    [SerializeField] private List<Storage> allStorages;

    [SerializeField] private Animator animator;

    void Start()
    {
        Invoke("RefreshStorages", 3);
    }

    void RefreshStorages()
    {
        animator.SetTrigger("Swap");
    }

    public void RespawnItems()
    {
        
    }
}
