using UnityEngine;
using System.Collections;

public class RandomAmbience : MonoBehaviour
{
    [System.Serializable]
    public class Ambience
    {
        public AudioClip ambient;
        public float volume = 1f;
        public float pitch = 1f;
    }

    public Ambience[] ambients;
    public Vector2 delay = new Vector2(5f, 10f);
    private AudioSource aud;
    private int lastIndex = -1;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        if (aud == null) aud = gameObject.AddComponent<AudioSource>();
        StartCoroutine(PlayAmbientLoop());
    }

    IEnumerator PlayAmbientLoop()
    {
        yield return new WaitForSeconds(Random.Range(delay.x, delay.y));
        while (true)
        {
            int index;
            do
            {
                index = Random.Range(0, ambients.Length);
            } while (ambients.Length > 1 && index == lastIndex);

            lastIndex = index;
            Ambience current = ambients[index];

            RandomPoint();
            aud.clip = current.ambient;
            aud.volume = current.volume;
            aud.pitch = current.pitch;
            aud.Play();

            yield return new WaitForSeconds(aud.clip.length);

            float waitTime = Random.Range(delay.x, delay.y);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void RandomPoint()
    {
        Vector3 randomDirection = Random.onUnitSphere;

        float distance = Random.Range(15f, 25f);

        transform.position = G.gm.player.transform.position + randomDirection * distance;
    }
}
