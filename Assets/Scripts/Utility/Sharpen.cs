using UnityEngine;

[ExecuteInEditMode]
[System.Serializable]

public class Sharpen : MonoBehaviour
{
    [Range(-2.0f, 2.0f)]
    public float strength = 0.5f;

    [Range(0.0f, 8.0f)]
    public float edgeMult = 0.2f;

    Shader shader { get; set; }

    Material _material;

    Material material
    {
        get
        {
            if (!_material)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }

            return _material;
        }

    }

    void blit(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    void OnDisable()
    {
        if (_material)
        {
            DestroyImmediate(_material);
        }
    }

    void Awake()
    {
        shader = Shader.Find("Hidden/Image Effects/Sharpen");
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_strength", strength);
        material.SetFloat("_edgeMult", edgeMult);

        blit(source, destination);
    }

}

