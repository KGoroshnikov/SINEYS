using System.Collections;
using UnityEngine;


public class TextureCycler : MonoBehaviour
{
    [Header("Textures & timing")]
    public Texture[] textures;

    public Vector2 delayRange = new Vector2(0.25f,0.5f);

    float delay = 1f;

    public bool playOnStart = true;

    Renderer _renderer;
    Coroutine _cycleCoroutine;
    int _currentIndex = 0;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        delay = Random.Range(delayRange.x,delayRange.y);
    }

    void OnEnable()
    {
        if (playOnStart)
            StartCycle();
    }

    void OnDisable()
    {
        StopCycle();
    }

    public void StartCycle()
    {
        if (_cycleCoroutine != null) StopCoroutine(_cycleCoroutine);
        _cycleCoroutine = StartCoroutine(CycleRoutine());
    }

    public void StopCycle()
    {
        if (_cycleCoroutine != null)
        {
            StopCoroutine(_cycleCoroutine);
            _cycleCoroutine = null;
        }
    }

    IEnumerator CycleRoutine()
    {
        var wait = new WaitForSeconds(Mathf.Max(0f, delay));

        while (true)
        {
            ApplyTexture(textures[_currentIndex]);

            _currentIndex = (_currentIndex + 1) % textures.Length;
            yield return wait;
        }
    }

    void ApplyTexture(Texture tex)
    {
        if (_renderer == null) return;
        if (tex == null)
        {
            return;
        }

        Material mat = _renderer.material;
        if (mat == null) return;

        mat.mainTexture = tex;
        if (mat.HasProperty("_EmissionMap"))
            mat.SetTexture("_EmissionMap", tex);


        mat.EnableKeyword("_EMISSION");
    }

    public void Next()
    {
        if (textures == null || textures.Length == 0) return;
        _currentIndex = (_currentIndex + 1) % textures.Length;
        ApplyTexture(textures[_currentIndex]);
    }

    public void Previous()
    {
        if (textures == null || textures.Length == 0) return;
        _currentIndex = (_currentIndex - 1 + textures.Length) % textures.Length;
        ApplyTexture(textures[_currentIndex]);
    }

    public void SetDelay(float newDelay)
    {
        delay = newDelay;
        if (_cycleCoroutine != null)
        {
            StopCycle();
            StartCycle();
        }
    }
}
