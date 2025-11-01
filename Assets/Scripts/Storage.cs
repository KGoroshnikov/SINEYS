using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [System.Serializable]
    public class Lvl
    {
        public List<Transform> possiblePosition;
    }
    [SerializeField] private List<Lvl> allLvls;

    public List<Lvl> GetPositions() => allLvls;
}
