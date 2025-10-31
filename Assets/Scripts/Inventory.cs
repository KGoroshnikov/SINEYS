using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInv
{
    public ItemInv(int _id, GameObject _obj)
    {
        id = _id;
        obj = _obj;
    }
    public int id;
    public GameObject obj;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemInv> inv = new List<ItemInv>();

    void Awake()
    {
        G.inventory = this;
    }

    public void AddItem(int id, GameObject obj)
    {
        inv.Add(new ItemInv(id, obj));
    }

    public ItemInv GetNextItem()
    {
        if (inv.Count == 0) return null;

        ItemInv item = inv[inv.Count - 1];
        inv.RemoveAt(inv.Count - 1);

        return item;
    }
}
