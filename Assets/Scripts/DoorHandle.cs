using UnityEngine;

public class DoorHandle : MonoBehaviour, IUsable
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool holding;

    [SerializeField] private Doors doors;

    public void Use()
    {
        holding = true;
        animator.SetTrigger("Rotate");
        doors.CloseDoors();
    }

    public void OnExitInteraction()
    {
        holding = false;
        animator.SetTrigger("Idle");
        doors.OpenDoors();
    }
}
