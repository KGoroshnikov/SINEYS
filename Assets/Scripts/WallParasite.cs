using Deform;
using System.Collections;
using UnityEngine;

public class WallParasite : MonoBehaviour, IUsable
{
    public int[] eated;
    [HideInInspector] public Task currentTask;
    public GameObject pipeSpherePref;
    public Deformable deformable;
    public Deformable deformable2;
    public Shaker rotShaker;
    private void Awake()
    {
        G.parasite = this;
    }

    public void Use()
    {
        currentTask.DisplayUpdate();
        GameObject pipeSphere = Instantiate(pipeSpherePref, pipeSpherePref.transform.parent);
        pipeSphere.SetActive(true);
        deformable.AddDeformer(pipeSphere.GetComponentInChildren<SpherifyDeformer>());
        deformable2.AddDeformer(pipeSphere.GetComponentInChildren<SpherifyDeformer>());
        GetComponent<Animation>().Play();
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        rotShaker.shakeSpeed = 5f;
        yield return new WaitForSeconds(0.65f);
        rotShaker.shakeSpeed = 0.5f;
    }

    public void ResetEated()
    {
        for (int i = 0; i < eated.Length; i++)
        {
            eated[i] = 0;
        }
    }
}
