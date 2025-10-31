using UnityEngine;
using UnityEngine.Events;

public class DelayEvent : MonoBehaviour
{
    public UnityEvent onDelay;
    public float delay;
    public bool active = true;
    float timer;
    void Update()
    {
        if (active) timer += Time.deltaTime;
        if(timer>delay)
        {
            onDelay.Invoke();
            active = false;
            timer = 0;
        }
    }
}
