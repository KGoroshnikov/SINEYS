using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    public bool active;

    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(2);
            }
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    Time.timeScale = i;
                }
            }
        }
    }
}
