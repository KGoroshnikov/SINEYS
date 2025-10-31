using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent onEnter;
    public bool once;
    bool active;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !active)
        {
            onEnter.Invoke();
            if (once)
                active = true;
        }
    }
}
