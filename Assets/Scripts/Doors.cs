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
        doors[0].localPosition = new Vector3(doors[0].localPosition.x, doors[0].localPosition.y, Mathf.Clamp(doors[0].localPosition.z + currentSpeed * Time.deltaTime, minXCoord, maxXCoord));
        doors[1].localPosition = new Vector3(doors[1].localPosition.x,
            doors[1].localPosition.y, Mathf.Clamp(doors[1].localPosition.z - currentSpeed * Time.deltaTime, -maxXCoord, -minXCoord));

        if (Mathf.Abs(doors[0].localPosition.z - maxXCoord) <= 0.05f && Mathf.Abs(doors[1].localPosition.z + maxXCoord) <= 0.05f)
        {
            G.deathSystem.PlayerDied(0);
        }
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
