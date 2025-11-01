using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int upgradeId;
    public float value;
    public GameObject nextUpgrade;
    public GameObject iconPref;
    public Resource[] resources;

    private void Start()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            GameObject icon = Instantiate(iconPref,iconPref.transform.parent);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = resources[i].count + "x";
            icon.GetComponent<RawImage>().texture = G.container.icons[resources[i].id];
        }
        Destroy(iconPref);
    }

    public void BuyUpgrade()
    {
        if (CheckResources())
        {
            switch (upgradeId)
            {
                case 0:
                    G.rigidcontroller.movementSettings.JumpForce = value;
                    break;
            }
            gameObject.SetActive(false);
            nextUpgrade.SetActive(true);
            for (int i = 0; i < resources.Length; i++)
            {
                G.container.itemsAmount[resources[i].id] -= resources[i].count;
            }
            G.container.ResourceDisplayUpdate();
        }
    }
    public bool CheckResources()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            if (G.container.itemsAmount[resources[i].id] < resources[i].count)
            {
                G.message.Message("Недостаточно ресурсов");
                return false;
            }
        }
        return true;
    }
    
}
[System.Serializable]
public class Resource
{
    public int id;
    public int count;
}
