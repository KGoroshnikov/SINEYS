using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    public AudioClip[] clips;
    public float volume = 1;
    public float pitch = 1;
   public void PlaySound(int id)
    {
        G.CreateSFX(clips[id], volume, pitch);
    }
}
