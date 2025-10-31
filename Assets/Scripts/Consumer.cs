using UnityEngine;

public class Consumer : MonoBehaviour, IUsable
{
    [SerializeField] private Transform eatPoint;

    public void Use()
    {
        ItemInv item = G.inventory.GetNextItem();
        if (item == null)
        {
            return;
        }
        item.obj.SetActive(true);
        item.obj.transform.position = G.rigidcontroller.transform.position;

        MoveObjects.Instance.AddObjectToMove(item.obj, eatPoint.position,
                Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 0.2f, EatObject);
    }

    public void EatObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
