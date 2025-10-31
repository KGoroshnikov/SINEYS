using TMPro;
using UnityEngine;


public class MessageManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public AudioClip sfx;

    public void Awake() => G.message = this;

    public void Message(string message)
    {
        if (timer > 0.125f)
        {
            TextMeshProUGUI textnew = Instantiate(text, text.transform.position, text.transform.rotation);
            textnew.text = message;
            textnew.transform.parent = text.transform.parent;
            textnew.gameObject.SetActive(true);
            G.CreateSFX(sfx, 0.25f,0.6f);
            timer = 0;
        }
    }
    float timer;
    private void Update()
    {
        timer += Time.deltaTime;
    }
}
