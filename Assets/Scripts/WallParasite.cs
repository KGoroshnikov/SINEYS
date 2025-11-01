using UnityEngine;

public class WallParasite : MonoBehaviour, IUsable
{
    public int[] eated;
    [HideInInspector] public Task currentTask;
    private void Awake()
    {
        G.parasite = this;
    }

    public void Use()
    {
        currentTask.DisplayUpdate();
    }

    public void ResetEated()
    {
        for (int i = 0; i < eated.Length; i++)
        {
            eated[i] = 0;
        }
    }
}
