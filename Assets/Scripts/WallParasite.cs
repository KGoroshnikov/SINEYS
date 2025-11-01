using Deform;
using System.Collections;
using UnityEngine;

public class WallParasite : MonoBehaviour, IUsable
{
    [SerializeField] private Transform eatPoint;
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
        for(int i = 0; i < currentTask.resources.Length; i++)
        {
            if (currentTask.resources[i].count <= 0) continue;

            int remain = currentTask.resources[i].count - eated[currentTask.resources[i].id];
            if (remain <= 0) continue;

            ItemInv item = G.inventory.GetSpecificItem(currentTask.resources[i].id);
            if (item == null) continue;
            item.obj.SetActive(true);
            item.obj.transform.position = G.rigidcontroller.transform.position;
            MoveObjects.Instance.AddObjectToMove(item.obj, eatPoint.position,
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), 0.4f, EatObject);

            eated[currentTask.resources[i].id] += 1;
            break;
        }

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

    public void EatObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ResetEated()
    {
        for (int i = 0; i < eated.Length; i++)
        {
            eated[i] = 0;
        }
    }
}
