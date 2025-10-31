using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public void Volume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    public void Sensivity (Slider slider)
    {
        G.rigidcontroller.mouseLook.XSensitivity = slider.value;
        G.rigidcontroller.mouseLook.YSensitivity = slider.value;
    }
    public void Brightness(Slider slider)
    {
        Camera.main.GetComponent<AmplifyColorBase>().Exposure = slider.value;
    }
}
