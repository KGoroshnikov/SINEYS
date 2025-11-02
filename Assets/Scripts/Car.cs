using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float colldown = 5;
    [SerializeField] private float forwardThr = 0.2f;

    [SerializeField] private float mushroomGrowTime = 60;
    [SerializeField] private Transform[] mushroomPoses;
    [SerializeField] private GameObject[] mushrooms;
    [SerializeField] private GameObject mushroomPref;

    private bool canHitPlayer = true;
    public AudioClip sbienieSFX;

    void ResetCD() => canHitPlayer = true;


    public void SpawnMushrooms()
    {
        mushrooms = new GameObject[3] { null, null, null };
        for (int i = 0; i < 3; i++) GrowMushroom();
        InvokeRepeating("GrowMushroom", mushroomGrowTime, mushroomGrowTime);
    }

    public void MushroomTaken(GameObject obj)
    {
        for(int i = 0; i < mushrooms.Length; i++)
        {
            if (mushrooms[i] == obj)
            {
                mushrooms[i] = null;
                break;
            }
        }
    }

    void GrowMushroom()
    {
        for(int i = 0; i < mushrooms.Length; i++)
        {
            if (mushrooms[i] != null) continue;

            GameObject muushroom = Instantiate(mushroomPref, mushroomPoses[i].position, mushroomPoses[i].rotation);
            muushroom.transform.SetParent(mushroomPoses[i]);
            muushroom.transform.localScale = Vector3.one;
            muushroom.GetComponent<Item>().onPickup.AddListener(MushroomTaken);
            mushrooms[i] = muushroom;
            return;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        if (!collision.gameObject.CompareTag("Player") || !canHitPlayer) return;

        Vector3 toPlr = G.rigidcontroller.transform.position - transform.position;
        float thr = Vector3.Dot(transform.forward, toPlr);
        Debug.Log(thr);
        if (thr < forwardThr) return;

        canHitPlayer = false;

        StartCoroutine(stunPlayer());

        Invoke("ResetCD", colldown);
    }

    IEnumerator stunPlayer()
    {
        G.CreateSFX(sbienieSFX,0.5f);
        float shakePowerSave = G.shaker.shakeStrength;
        G.shaker.shakeStrength /= 3;
        G.shaker.active = true;
        G.crane.DisableCrane();
        G.rigidcontroller.SetFreezeState(true);
        G.cameraAnims.Fall();
        for (int i = 0; i < 3; i++)
        {
            G.fader.FadeIn(0.5f, 0.9f);
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / 0.5f;
                yield return null;
            }
            G.fader.FadeIn(0.5f, 0.4f);
            while (t > 0)
            {
                t -= Time.deltaTime / 0.5f;
                yield return null;
            }
        }
        G.shaker.shakeStrength = shakePowerSave;
        G.shaker.active = false;
        Debug.Log(G.playerDied);
        if (!G.playerDied)
        {
            G.cameraAnims.Getup();
            G.crane.DisableCrane();
            G.rigidcontroller.SetFreezeState(false);
        }
    }
}
