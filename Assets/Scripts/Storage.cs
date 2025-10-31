using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private List<Transform> possiblePosition;

    public List<Transform> GetPositions() => possiblePosition;
}
