using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour, IUsable
{
    [SerializeField] private int ID;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] private int weight;
    public UnityEvent<GameObject> onPickup;

    public void Use()
    {
        if (G.inventory.GetRemainingCapacity() < weight)
        {
            G.message.Message("ИНВЕНТАРЬ ПОЛОН");
            return;
        }
        if (rb != null)
            rb.isKinematic = true;
        if (onPickup != null)
            onPickup.Invoke(gameObject);
        if (col != null)
            col.enabled = false;
        MoveObjects.Instance.AddObjectToMove(gameObject, G.rigidcontroller.transform.position, Quaternion.identity, 0.3f, CollectObject);
    }

    public void CollectObject(GameObject obj)
    {
        G.inventory.AddItem(ID, gameObject, weight);
        gameObject.SetActive(false);
    }
}
