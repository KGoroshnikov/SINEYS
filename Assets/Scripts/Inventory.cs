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
    public int maxCapacity;
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

    public void RefreshUI()
    {
        for(int i = 0; i < inv.Count; i++)
        {
            inv[i].UIObject.transform.localPosition = i * offsetUI;
        }
    }

    public int GetRemainingCapacity() => remainingCapacity;

    public ItemInv GetNextItem()
    {
        if (inv.Count == 0) return null;

        ItemInv item = inv[inv.Count - 1];
        remainingCapacity += item.weight;
        Destroy(item.UIObject);
        inv.RemoveAt(inv.Count - 1);
        RefreshUI();

        return item;
    }

    public ItemInv GetSpecificItem(int id)
    {
        for(int i = 0; i < inv.Count; i++)
        {
            if (inv[i].id == id)
            {
                ItemInv item = inv[i];
                remainingCapacity += item.weight;
                Destroy(item.UIObject);
                inv.RemoveAt(i);
                RefreshUI();
                return item;
            }
        }
        return null;
    }
}
