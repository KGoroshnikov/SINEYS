using UnityEngine;

public class Item : MonoBehaviour, IUsable
{
    [SerializeField] private int ID;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    public void Use()
    {
        rb.isKinematic = true;
        col.enabled = false;
        MoveObjects.Instance.AddObjectToMove(gameObject, G.rigidcontroller.transform.position, Quaternion.identity, 0.3f, CollectObject);
    }

    public void CollectObject(GameObject obj)
    {
        G.inventory.AddItem(ID, gameObject);
        gameObject.SetActive(false);
    }
}
