using UnityEngine;

public class Consumer : MonoBehaviour, IUsable
{
    [SerializeField] private Transform eatPoint;
    public AudioClip insertSFX;

    public void Awake()
    {
        G.consumer = this;
    }

    public void Use()
    {
        ItemInv item = G.inventory.GetNextItem();
        if (item == null)
        {
            G.message.Message("Нет предметов");
            return;
        }
        item.obj.SetActive(true);
        item.obj.transform.position = G.rigidcontroller.transform.position;
        G.container.AddItem(item.id);
        G.container.ResourceDisplayUpdate();
        MoveObjects.Instance.AddObjectToMove(item.obj, eatPoint.position,
                Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 0.2f, EatObject);
        Delay.InvokeDelayed(() => GetComponent<Shaker>().ShakeIt(0.5f),0.25f);
        G.CreateSFX(insertSFX,1,Random.Range(0.9f,1.1f));
    }

    public void EatObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
