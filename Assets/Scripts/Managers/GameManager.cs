using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cursor;
    [HideInInspector] public bool cantEsc;
    [HideInInspector] public bool cantZoom;
    private void Awake() => G.gm = this;

    private void Start()
    {
        G.HideCursor();
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
