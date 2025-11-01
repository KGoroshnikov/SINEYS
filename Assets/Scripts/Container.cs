using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    public int[] itemsAmount;
    public Texture[] icons;
    public GameObject iconPref;
    List<GameObject> itemSlots;
    private void Awake()
    {
        G.container = this;
        itemsAmount = new int[6];
    }

    public void AddItem(int id)
    {
        itemsAmount[id]++;
    }

    private void Start()
    {
        itemSlots = new List<GameObject>();
        for (int i = 0; i < itemsAmount.Length; i++)
        {
            GameObject icon = Instantiate(iconPref, iconPref.transform.parent);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = itemsAmount[i] + "x";
            icon.GetComponent<RawImage>().texture = icons[i];
            if(itemsAmount[i]<= 0)
                icon.SetActive(false);
            itemSlots.Add(icon);
        }
        Destroy(iconPref);
    }
    [Button]
    public void ResourceDisplayUpdate()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = itemsAmount[i] + "x";
            if (itemsAmount[i] <= 0) itemSlots[i].SetActive(false);
            else itemSlots[i].SetActive(true);
        }
    }
}