using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    private void Start()
    {
        LoadSettings();
    }
    public void Volume(Slider slider)
    {
        AudioListener.volume = slider.value;
        SaveSettings();
    }
    public void Sensivity (Slider slider)
    {
        G.rigidcontroller.mouseLook.XSensitivity = slider.value;
        G.rigidcontroller.mouseLook.YSensitivity = slider.value;
        SaveSettings();
    }
    public void Brightness(Slider slider)
    {
        //Camera.main.GetComponent<AmplifyColorBase>().Exposure = slider.value;
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.SetFloat("Sensivity", G.rigidcontroller.mouseLook.XSensitivity);
        PlayerPrefs.SetFloat("Brightness",1);
    }
    public void LoadSettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume",1);
        G.rigidcontroller.mouseLook.XSensitivity = PlayerPrefs.GetFloat("Sensivity", 2);
        G.rigidcontroller.mouseLook.YSensitivity = PlayerPrefs.GetFloat("Sensivity",2);
        //brightness = PlayerPrefs.GetFloat("Brightness",1);
    }
}
