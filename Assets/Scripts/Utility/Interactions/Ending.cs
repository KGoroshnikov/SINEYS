using UnityEngine;

public class Ending : MonoBehaviour
{
    public GameObject endPanel;
    
    public void End()
    {
        G.gm.cantEsc = true;
        G.raycast.enabled = false;
        G.rigidcontroller.enabled = false;
        endPanel.SetActive(true);
        Delay.InvokeDelayed(() => G.ShowCursor(), 1.35f);
    }
}
