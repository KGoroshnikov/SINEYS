using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInv
{
    public ItemInv(int _id, GameObject _obj, int _weight, GameObject _uiobj)
    {
        id = _id;
        obj = _obj;
        weight = _weight;
        UIObject = _uiobj;
    }
    public int id;
    public GameObject obj;
    public int weight;
    public GameObject UIObject;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity;
    [SerializeField] private int remainingCapacity;
    [SerializeField] private List<ItemInv> inv = new List<ItemInv>();

    [SerializeField] private GameObject[] uiPrefs;
    [SerializeField] private Transform spawnUIPos;
    [SerializeField] private Vector3 offsetUI;

    void Awake()
    {
        G.inventory = this;
        remainingCapacity = maxCapacity;
    }

    public void AddItem(int id, GameObject obj, int weight)
    {
        GameObject uiicon = Instantiate(uiPrefs[id], spawnUIPos.position, spawnUIPos.rotation);
        uiicon.transform.SetParent(spawnUIPos);
        uiicon.transform.localPosition = offsetUI * inv.Count;
        uiicon.transform.localEulerAngles = Vector3.zero;

        remainingCapacity -= weight;
        inv.Add(new ItemInv(id, obj, weight, uiicon));
    }

    public int GetRemainingCapacity() => remainingCapacity;

    public ItemInv GetNextItem()
    {
        if (inv.Count == 0) return null;

        ItemInv item = inv[inv.Count - 1];
        remainingCapacity += item.weight;
        inv.RemoveAt(inv.Count - 1);

        return item;
    }
}
