using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private float speedOpen;
    [SerializeField] private float speedClose;
    private float currentSpeed;

    [SerializeField] private Transform[] doors;

    [SerializeField] private float maxXCoord; // position
    [SerializeField] private float minXCoord;

    private int dir = 1;

    void Start()
    {
        currentSpeed = speedOpen;
        dir = 1;
    }

    void FixedUpdate()
    {
        doors[0].localPosition = new Vector3(Mathf.Clamp(doors[0].localPosition.x + currentSpeed * Time.deltaTime, minXCoord, maxXCoord),
            doors[0].localPosition.y, doors[0].localPosition.z);
        doors[1].localPosition = new Vector3(Mathf.Clamp(doors[1].localPosition.x - currentSpeed * Time.deltaTime, -maxXCoord, -minXCoord),
            doors[1].localPosition.y, doors[1].localPosition.z);
    }

    public void CloseDoors()
    {
        currentSpeed = -speedClose;
        dir = -1;
    }
    
    public void OpenDoors()
    {
        currentSpeed = speedOpen;
        dir = 1;
    }
}
