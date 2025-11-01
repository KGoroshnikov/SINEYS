using System.Collections;
using UnityEngine;

public class DeathSystem : MonoBehaviour
{
    [SerializeField] private Transform doorsPos;

    private bool playerDead;

    void Awake()
    {
        G.deathSystem = this;
    }

    // 0 - doors
    // 1 - wall flash
    public void PlayerDied(int deathId)
    {
        if (playerDead) return;
        playerDead = true;
        StartCoroutine(deathProcess(deathId));
    }

    IEnumerator deathProcess(int deathId)
    {
        G.miniGame.AutoRelesePlayer();
        G.crane.DisableCrane();
        G.rigidcontroller.SetFreezeState(true);
        G.fader.FadeIn(0.1f);
        yield return new WaitForSeconds(0.1f);
        G.fader.FadeOut(1f);
        if (deathId == 0)
        {
            G.rigidcontroller.transform.position = doorsPos.position;
            G.rigidcontroller.transform.rotation = doorsPos.rotation;
        }
    }
}
