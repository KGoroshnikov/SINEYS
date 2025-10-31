using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioClip clickSFX;
    public AudioClip hoverSFX;
    public GameObject cursor;
    public Fader fader;
    public void StartGame()
    {
        fader.FadeIn(1);
        Cursor.lockState = CursorLockMode.Locked;
        cursor.SetActive(false);
        Delay.InvokeDelayed(() => SceneManager.LoadScene(1), 1);
    }

    public void Leave()
    {
        fader.FadeIn(1);
        Cursor.lockState = CursorLockMode.Locked;
        cursor.SetActive(false);
        Delay.InvokeDelayed(() => Application.Quit(), 1);
    }
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void ClickSFX()
    {
        G.CreateSFX(clickSFX, 0.5f);
    }
    public void PlayHover()
    {
        G.CreateSFX(hoverSFX, 0.8f);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
