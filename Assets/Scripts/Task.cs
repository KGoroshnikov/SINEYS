using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    public TaskResource[] resources;
    public GameObject next;
    public GameObject iconPref;
    List<GameObject> itemSlots;
    public bool last;

    public GameObject[] spawnPoints;


    public UnityEvent onComplete;
    private void Start()
    {
        itemSlots = new List<GameObject>();
        for (int i = 0; i < resources.Length; i++)
        {
            GameObject icon = Instantiate(iconPref, iconPref.transform.parent);
            resources[i].count = Random.Range(resources[i].ranCount.x, resources[i].ranCount.y);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = "0/"+resources[i].count;
            icon.GetComponent<RawImage>().texture = G.container.icons[resources[i].id];
            itemSlots.Add(icon);
            if (resources[i].count <= 0) icon.SetActive(false);
        }
        G.parasite.currentTask = this;
        Destroy(iconPref);
    }

    public void DisplayUpdate()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{G.parasite.eated[resources[i].id]}/{resources[i].count}";
        }
        if (last && !G.parasite.death)
        {
            if (CheckTask())
            {
                gameObject.SetActive(false);
                G.parasite.Kill();
                
            }
        }
    }

    public void NextTask()
    {
        if (CheckTask() && !last)
        {
            next.SetActive(true);
            gameObject.SetActive(false);
            G.parasite.ResetEated();
            onComplete?.Invoke();
            G.heart.mainArrowT = 0;
            if(spawnPoints.Length > 0)
            {
                for (int i = 0; i < spawnPoints.Length; i++)
                {
                    if (spawnPoints[i].GetComponentInChildren<Car>())
                    {
                        spawnPoints[i].GetComponentInChildren<Car>().SpawnMushrooms();
                    }
                    spawnPoints[i].SetActive(true);
                }
            }
        }
        else G.message.Message("Недостаточно предметов");
    }
    public bool CheckTask()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (G.parasite.eated[resources[i].id] < resources[i].count) return false;
        }
        return true;
    }
}
[System.Serializable]
public class TaskResource
{
    public int id;
    [HideInInspector] public int count;
    public Vector2Int ranCount;
}
