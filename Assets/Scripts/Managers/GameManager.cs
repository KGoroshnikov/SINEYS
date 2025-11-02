using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cursor;
    [HideInInspector] public bool cantEsc;
    [HideInInspector] public bool cantZoom;

    public AudioClip upgradeSFX;
    public AudioClip deathSFX;
    private void Awake()
    {
        G.gm = this;
        G.playerDied = false;
    }

    private void Start()
    {
        G.HideCursor();
    }
    bool deadge;
    public void Death()
    {
        deadge = true;
        StartCoroutine(SmertVnishete());
       
    }
    IEnumerator SmertVnishete()
    {
        G.CreateSFX(deathSFX);
        G.HideCursor();
        G.fader.FadeIn(0.5f);
        G.fader.GetComponent<Image>().raycastTarget = true;
        Time.timeScale = 1;
        yield return new WaitForSeconds(1);
        Delay.InvokeDelayed(() => SceneManager.LoadScene(2), 1f);
    }

    private void Update()
    {
        if (G.playerDied && !deadge) Death();
    }
}

public class G
{
    public static GameManager gm;
    public static RigidbodyFirstPersonController rigidcontroller;
    public static MiniGame miniGame;
    public static Crane crane;
    public static DeathSystem deathSystem;
    public static Inventory inventory;
    public static RayCast raycast;
    public static bool playerDied;
    public static CameraAnims cameraAnims;
    public static Fader fader;
    public static Shaker shaker;
    public static AABB2DScreenOutline aabb;
    public static MessageManager message;
    public static SmoothAudio smoothAudio;
    public static Consumer consumer;
    public static Container container;
    public static WallParasite parasite;
    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        gm.cursor.SetActive(true);
        gm.cursor.GetComponent<CCursor>().Update();
    }
    public static void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gm.cursor.SetActive(false);
    }


    public static void CreateSFX(AudioClip clip, float volume = 1, float pitch = 1)
    {
        if (clip == null) return;
        GameObject sfx = new GameObject("SFX");
        AudioSource asfx = sfx.AddComponent<AudioSource>();
        asfx.clip = clip;
        asfx.volume = volume;
        asfx.pitch = pitch;
        asfx.Play();
        sfx.AddComponent<DestroyAfterTime>().time = clip.length + 1f;
    }

    public static void CreateSFX(AudioClip clip, Transform transform, float volume = 1, float pitch = 1)
    {
        if (clip == null) return;
        GameObject sfx = new GameObject("SFX");
        AudioSource asfx = sfx.AddComponent<AudioSource>();
        asfx.clip = clip;
        asfx.volume = volume;
        asfx.pitch = pitch;
        sfx.transform.position = transform.position;
        asfx.spatialBlend = 1;
        asfx.rolloffMode = AudioRolloffMode.Linear;
        asfx.maxDistance = 10;
        asfx.Play();
        sfx.AddComponent<DestroyAfterTime>().time = clip.length + 1;
    }
}
