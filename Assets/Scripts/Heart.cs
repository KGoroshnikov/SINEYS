using System.Collections;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private Material monsterMat;

    [SerializeField] private float timeToRed;
    [SerializeField] private float timeToDark;

    [ColorUsageAttribute(false, true), SerializeField] private Color redColor;
    [ColorUsageAttribute(false, true), SerializeField] private Color darkColor;

    [SerializeField] private Material pipeMat;
    [ColorUsageAttribute(false, true), SerializeField] private Color pipeRed;
    [ColorUsageAttribute(false, true), SerializeField] private Color pipeWhite;

    [SerializeField] private Transform arrowMain;

    [SerializeField] private Vector2 mainArrowAngles;
    [SerializeField] private float mainArrowTimeDeath;
    private float mainArrowT;


    [SerializeField] private float fedPenalty;
    [SerializeField] private float fedGood;
    [SerializeField] private float timingSpace = 0.2f;
    private bool allowedToFeed;

    void Start()
    {
        StartCoroutine(matAnim());
    }

    float NormalizeAngle180(float a)
    {
        return Mathf.Repeat(a + 180f, 360f) - 180f;
    }

    public void Fed()
    {
        if (!allowedToFeed)
        {
            mainArrowT += fedPenalty;
        }
        else
        {
             mainArrowT -= fedGood;
        }
    }

    public void Heartbeat()
    {
        StartCoroutine(matAnim());
        allowedToFeed = true;
        Invoke("StopFeedTiming", timingSpace);
    }
    
    void StopFeedTiming()
    {
        allowedToFeed = false;
    }

    void Update()
    {
        mainArrowT = Mathf.Clamp(mainArrowT + Time.deltaTime / mainArrowTimeDeath, 0, 1);
        float angle = Mathf.Lerp(mainArrowAngles.x, mainArrowAngles.y, mainArrowT);
        angle = NormalizeAngle180(angle);
        arrowMain.localEulerAngles = new Vector3(arrowMain.localEulerAngles.x, arrowMain.localEulerAngles.y, angle);

        pipeMat.SetColor("_EmissionColor", Color.Lerp(pipeWhite, pipeRed, mainArrowT));

        if (mainArrowT >= 1 && !G.gm.cantEsc)
        {
            G.deathSystem.PlayerDied(1);
        }
    }

    IEnumerator matAnim()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / timeToRed;
            monsterMat.SetColor("_Emission", Color.Lerp(darkColor, redColor, t));
            yield return null;
        }
        while (t > 0)
        {
            t -= Time.deltaTime / timeToDark;
            monsterMat.SetColor("_Emission", Color.Lerp(darkColor, redColor, t));
            yield return null;
        }
        yield return null;
    }

    public void StartTiming()
    {
        
    }
    
    public void EndTiming()
    {
        
    }
}
