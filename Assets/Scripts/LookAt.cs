using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform obj;
    public float rotationSpeed = 5f; 
    void Update()
    {
        if (obj == null) obj = Camera.main.transform;
        Vector3 direction = obj.position - transform.position;

        if (direction.sqrMagnitude > 0.001f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
