using UnityEngine;

public class MatOffset : MonoBehaviour
{
    public Material material;
    public Vector2 offset;
    
    void FixedUpdate()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
