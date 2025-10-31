using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    public GameObject panel;
    public AudioClip hoverSFX;
    public AudioClip clickSFX;

    public GameObject leaveButton;
    public GameObject leaveButtonConfirm;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!panel.activeInHierarchy && !G.gm.cantEsc)
            {
                G.aabb.currentObject = null;
                panel.SetActive(true);
                G.gm.cantEsc = true;
                G.ShowCursor();
                G.raycast.enabled = false;
                G.rigidcontroller.enabled = false;

                Delay.InvokeDelayed(() => Time.timeScale = 0, 0);
            }
            else if (panel.activeInHierarchy)
            {
                Resume();
            }
        }
    }
    public void Resume()
    {
        if (panel.activeInHierarchy)
        {
            panel.SetActive(false);
            G.gm.cantEsc = false;
            G.HideCursor();
            G.raycast.enabled = true;
            G.rigidcontroller.enabled = true;
            Time.timeScale = 1;
            leaveButton.SetActive(true);
            leaveButtonConfirm.SetActive(false);
        }
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void ToMenu()
    {
        G.HideCursor();
        G.fader.FadeIn(1);
        G.fader.GetComponent<Image>().raycastTarget = true;
        Time.timeScale = 1;
        Delay.InvokeDelayed(() => SceneManager.LoadScene(0), 1f);
    }


    public void ClickSFX()
    {
        G.CreateSFX(clickSFX, 0.3f);
    }
    public void PlayHover()
    {
        G.CreateSFX(hoverSFX, 0.75f);
    }
}
