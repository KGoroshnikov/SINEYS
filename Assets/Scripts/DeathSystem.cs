using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class DeathSystem : MonoBehaviour
{
    [SerializeField] private Transform doorsPos;
    [SerializeField] private PlayableDirector doorsCS;

    [SerializeField] private Transform wallPos;
    [SerializeField] private PlayableDirector wallCS;

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
        // крик монстра
        G.shaker.active = true;
        yield return new WaitForSeconds(2f);
        
        G.miniGame.AutoRelesePlayer();
        G.crane.DisableCrane();
        G.rigidcontroller.SetFreezeState(true);
        G.fader.FadeIn(0.1f);
        yield return new WaitForSeconds(0.1f);
        G.shaker.active = false;
        G.fader.FadeOut(1f);
        if (deathId == 0)
        {
            G.rigidcontroller.transform.SetParent(doorsPos);
            G.rigidcontroller.transform.localPosition = Vector3.zero;
            G.rigidcontroller.transform.localEulerAngles = Vector3.zero;

            doorsCS.Play();
        }
        else if (deathId == 1)
        {
            G.rigidcontroller.transform.SetParent(wallPos);
            G.rigidcontroller.transform.localPosition = Vector3.zero;
            G.rigidcontroller.transform.localEulerAngles = Vector3.zero;

            wallCS.Play();
        }
    }

    public void EndGame()
    {
        StartCoroutine(ExitGame());
    }

    IEnumerator ExitGame()
    {
        G.fader.FadeIn(0.1f);
        yield return new WaitForSeconds(3f);
        // go to menu
    }
}
