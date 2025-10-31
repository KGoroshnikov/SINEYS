using SplineMesh;
using UnityEngine;

public class SplinePoint : MonoBehaviour
{
    public int id;
    public Spline spline;

    void Update()
    {
        spline.nodes[id].Position = spline.transform.InverseTransformPoint(transform.position);


    }
}
