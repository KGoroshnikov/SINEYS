using UnityEngine;
using UnityEngine.SceneManagement;

public class StartCS : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("GAME");
    }
}
