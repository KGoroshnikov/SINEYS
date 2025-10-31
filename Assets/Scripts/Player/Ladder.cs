using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ladder : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //G.rigidcontroller.OnLadderEnter(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //G.rigidcontroller.OnLadderExit(this);
    }
}
