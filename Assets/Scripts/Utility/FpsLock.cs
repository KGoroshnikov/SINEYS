using UnityEngine;

public class FpsLock : MonoBehaviour
{
    public bool vsync = true;
    public int fpslimit = 60;
    private void Awake()
    {
        Application.targetFrameRate = fpslimit;

        if (vsync)
            QualitySettings.vSyncCount = 1;
        else
            QualitySettings.vSyncCount = 0;
    }
}
