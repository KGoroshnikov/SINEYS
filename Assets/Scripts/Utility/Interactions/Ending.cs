using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject endPanel;
    
    public void End()
    {
        G.gm.cantEsc = true;
        G.raycast.enabled = false;
        G.rigidcontroller.enabled = false;
        //endPanel.SetActive(true);
        G.fader.FadeIn(2);
        Delay.InvokeDelayed(() => SceneManager.LoadScene(3), 3f);
    }
}
