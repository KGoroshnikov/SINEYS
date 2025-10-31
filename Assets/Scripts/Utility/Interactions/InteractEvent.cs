using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour, IUsable
{
    public UnityEvent onClick;
    public bool once;
    bool active;
    public void Use()
    {
        if (!active)
        {
            onClick.Invoke();
            if (once) active = true;
        }
    }
}
