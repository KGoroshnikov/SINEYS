using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [System.Serializable]
    public class tierObjects
    {
        public Vector2Int randAmount;
        public List<GameObject> possibleItems = new List<GameObject>();
    }
    [SerializeField] private List<tierObjects> allTiersItems = new List<tierObjects>();

    [SerializeField] private int currentTier;

    [SerializeField] private List<Transform> possiblePosition;

    public void RefreshItems()
    {
        List<Transform> avalPoses = possiblePosition;

        int amount = Random.Range(allTiersItems[currentTier].randAmount.x, allTiersItems[currentTier].randAmount.y);
        for(int i = 0; i < amount; i++)
        {
            int posIdx = Random.Range(0, avalPoses.Count);
            avalPoses.RemoveAt(posIdx);
        }
    }
}
