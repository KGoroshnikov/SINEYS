using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Crane : MonoBehaviour
{
    [SerializeField] private List<Transform> poses;

    [SerializeField] private Transform craneObj;

    [SerializeField] private VisualEffect puffVfx;

    [SerializeField] private float heightDown;
    private float normalHeight;

    [SerializeField] private float speed;
    [SerializeField] private float moveDownSpeed;

    [SerializeField] private float chanceGoToPlayer = 0.15f;
    private bool chasingPlayer = false;

    [SerializeField] private float rangeCatch;
    [SerializeField] private Transform playerCatchedPos;

    [SerializeField] private Animator animator;

    private bool gamePassed = true;

    private bool stopped;

    private Vector3 targetPos;

    void Awake()
    {
        G.crane = this;
    }

    void Start()
    {
        normalHeight = craneObj.position.y;
        ChoosePos();
        StartCoroutine(CraneLogic());
    }

    void ChoosePos()
    {
        targetPos = poses[Random.Range(0, poses.Count)].position;
        if (Random.value <= chanceGoToPlayer)
        {
            chasingPlayer = true;
            gamePassed = false;
        }
        else
        {
            chasingPlayer = false;
            gamePassed = true;
        }
    }

    public void PassGame()
    {
        gamePassed = true;
        chasingPlayer = false;
    }

    public void DisableCrane()
    {
        stopped = true;
    }
    public void EnableCrane()
    {
        stopped = false;
    }


    IEnumerator CraneLogic()
    {
        while (true)
        {
            while (Mathf.Abs(transform.position.x - targetPos.x) > 0.02 || Mathf.Abs(craneObj.position.z - targetPos.z) > 0.02)
            {
                if (stopped || G.gm.cantEsc)
                {
                    yield return null;
                    continue;
                }
                if (chasingPlayer) targetPos = G.rigidcontroller.transform.position;
    
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, transform.position.y, transform.position.z), Time.deltaTime * speed);
                craneObj.position = Vector3.MoveTowards(craneObj.position, new Vector3(craneObj.position.x, craneObj.position.y, targetPos.z), Time.deltaTime * speed);
                yield return null;
            }


            animator.SetTrigger("Open");

            while (craneObj.position.y > heightDown)
            {
                if (stopped || G.gm.cantEsc)
                {
                    yield return null;
                    continue;
                }
                craneObj.position = Vector3.MoveTowards(craneObj.position, new Vector3(craneObj.position.x, heightDown, craneObj.position.z), Time.deltaTime * moveDownSpeed);
                yield return null;
            }

            puffVfx.Play();

            animator.SetTrigger("Close");

            bool playerTrapped = false;
            if (chasingPlayer && Vector3.Distance(new Vector3(craneObj.position.x, G.rigidcontroller.transform.position.y, craneObj.position.z), G.rigidcontroller.transform.position) <= rangeCatch)
            {
                G.miniGame.StartGame(playerCatchedPos);
                playerTrapped = true;
            }

            yield return new WaitForSeconds(1);

            while (craneObj.position.y < normalHeight)
            {
                if (stopped || G.gm.cantEsc)
                {
                    yield return null;
                    continue;
                }
                craneObj.position = Vector3.MoveTowards(craneObj.position, new Vector3(craneObj.position.x, normalHeight, craneObj.position.z), Time.deltaTime * moveDownSpeed);
                yield return null;
            }

            while (playerTrapped && !gamePassed)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            ChoosePos();

        }
    }

    void OnDrawGizmosSelected()
    {
        if (G.rigidcontroller == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(craneObj.position.x, G.rigidcontroller.transform.position.y, craneObj.position.z), rangeCatch);
    }

}
