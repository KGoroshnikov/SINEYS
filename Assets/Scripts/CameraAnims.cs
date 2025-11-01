using UnityEditor.Animations;
using UnityEngine;

public class CameraAnims : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AnimatorController animatorController;

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
