using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float amount = 1;
    public float speed = 1;
    private Vector3 lastPos;
    private float dist;
    private Vector3 rotation = Vector3.zero;
    private void Awake()
    {
        //G.shaker = GetComponent<Shaker>();
    }

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        dist += (transform.position - lastPos).magnitude;
        lastPos = transform.position;
        rotation.z = Mathf.Sin(dist * speed) * amount;
        transform.localEulerAngles = rotation;
    }
}