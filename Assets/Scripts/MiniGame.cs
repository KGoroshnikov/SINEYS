using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Transform sector;

    [SerializeField] private GameObject[] objectsGame;

    [SerializeField] private Animator animator;

    [SerializeField] private float speed = 120f;
    [SerializeField] private float sectorAngle = 60f;
    [SerializeField] private float restartDelay = .4f;

    [SerializeField] private MeshRenderer[] bulbs;
    [SerializeField] private Material[] redGreenBulbs;

    [SerializeField] private float autoRelease = 10; // sec

    private int cntWin;

    private int dir = 1;
    private bool roundActive;

    void Awake()
    {
        G.miniGame = this;
    }

    public void StartGame(Transform playerCatchedPos)
    {
        G.rigidcontroller.SetFreezeState(true);
        MoveObjects.Instance.AddObjectToMove(G.rigidcontroller.gameObject, playerCatchedPos.position, playerCatchedPos.rotation, 1f);
        G.rigidcontroller.transform.SetParent(playerCatchedPos);

        bulbs[0].sharedMaterial = redGreenBulbs[0];
        bulbs[1].sharedMaterial = redGreenBulbs[0];
        cntWin = 0;
        for (int i = 0; i < objectsGame.Length; i++)
            objectsGame[i].SetActive(true);
        animator.SetTrigger("Appear");
        roundActive = true;
        RandomizeSectorRotation();

        Invoke("AutoRelesePlayer", autoRelease);
    }

    void AutoRelesePlayer()
    {
        G.rigidcontroller.SetFreezeState(false);
        Invoke("HideObjects", 2);
        CancelInvoke("AutoRelesePlayer");
        animator.SetTrigger("Hide");
        G.crane.PassGame();
        roundActive = false;
    }

    private void Update()
    {
        if (!roundActive) return;
        if (hand != null)
        {
            hand.Rotate(0f, speed * dir * Time.deltaTime, 0f, Space.Self);
        }

        HandleInput();
    }

    private void HandleInput()
    {
        if (!roundActive) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            bool isWin = CheckPointerInSector();
            if (isWin) OnWin();
            else OnLose();
        }
    }

    private bool CheckPointerInSector()
    {
        Vector3 handFwd = Vector3.ProjectOnPlane(hand.forward, Vector3.up).normalized;
        Vector3 sectorFwd = Vector3.ProjectOnPlane(sector.forward, Vector3.up).normalized;

        float angle = Mathf.Abs(Vector3.SignedAngle(sectorFwd, handFwd, Vector3.up));

        return angle <= (sectorAngle * 0.5f);
    }

    private void OnWin()
    {
        cntWin = Mathf.Clamp(cntWin + 1, 0, 2);
        bulbs[cntWin - 1].sharedMaterial = redGreenBulbs[1];

        Debug.Log("Win!");
        StartCoroutine(RestartCoroutine());
    }

    private void OnLose()
    {
        cntWin = 0;
        bulbs[0].sharedMaterial = redGreenBulbs[0];
        bulbs[1].sharedMaterial = redGreenBulbs[0];

        Debug.Log("Lose!");
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        roundActive = false;

        yield return new WaitForSeconds(restartDelay);

        RestartLvl();

        yield return new WaitForSeconds(0.05f);
        if (cntWin < 2) roundActive = true;
    }

    private void RestartLvl()
    {
        if (cntWin >= 2)
        {
            G.rigidcontroller.SetFreezeState(false);
            Invoke("HideObjects", 2);
            CancelInvoke("AutoRelesePlayer");
            animator.SetTrigger("Hide");
            roundActive = false;
            G.crane.PassGame();
            return;
        }

        dir *= -1;
        RandomizeSectorRotation();
    }

    void ReleasePlayer()
    {
        
    }

    void HideObjects()
    {
        for (int i = 0; i < objectsGame.Length; i++)
            objectsGame[i].SetActive(true);
    }

    private void RandomizeSectorRotation()
    {
        float y = Random.Range(0f, 360f);
        Vector3 e = sector.localEulerAngles;
        sector.localEulerAngles = new Vector3(e.x, y, e.z);
    }

}
