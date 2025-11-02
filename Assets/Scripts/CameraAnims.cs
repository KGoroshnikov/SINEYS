using UnityEngine;

public class CameraAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController animatorController;

    void Awake()
    {
        G.cameraAnims = this;
    }

    public void Fall()
    {
        animator.runtimeAnimatorController = animatorController;
        animator.SetTrigger("Fall");
    }

    public void Getup()
    {
        animator.SetTrigger("Getup");
    }

    public void DisableAnimator()
    {
        animator.runtimeAnimatorController = null;
    }
}
