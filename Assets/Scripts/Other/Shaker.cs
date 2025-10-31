using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public bool active = true;

    [System.Flags]
    public enum ShakeType
    {
        None = 0,
        Position = 1 << 0,
        Rotation = 1 << 1,
        Scale = 1 << 2,
        All = Position | Rotation | Scale
    }

    [System.Flags]
    public enum Directions
    {
        None = 0,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2,
        All = X | Y | Z
    }

    public ShakeType shakeType = ShakeType.Position;
    public Directions direction = Directions.All;

    public bool uniformAxes = false;

    [Range(0f, 30f)] public float shakeStrength = 1f;
    [Range(0.1f, 20f)] public float shakeSpeed = 5f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private float smoothShakeStrength;

    private void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        originalScale = transform.localScale;

        smoothShakeStrength = shakeStrength;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShakeIt(0.5f);
        }

        if (!active) return;

        smoothShakeStrength = Mathf.Lerp(smoothShakeStrength, shakeStrength, Time.deltaTime * 5f);

        if (shakeType.HasFlag(ShakeType.Position)) ShakePosition();
        if (shakeType.HasFlag(ShakeType.Rotation)) ShakeRotation();
        if (shakeType.HasFlag(ShakeType.Scale)) ShakeScale();
    }

    private void ShakePosition()
    {
        float t = Time.time * shakeSpeed;

        Vector3 offset = new Vector3(
            (Mathf.PerlinNoise(t, 0f) - 0.5f),
            (Mathf.PerlinNoise(0f, t) - 0.5f),
            (Mathf.PerlinNoise(t, t) - 0.5f)
        ) * smoothShakeStrength;

        offset = ApplyDirection(offset);
        transform.localPosition = originalPosition + offset;
    }

    private void ShakeRotation()
    {
        float t = Time.time * shakeSpeed;

        Vector3 offset = new Vector3(
            (Mathf.PerlinNoise(t, 0f) - 0.5f),
            (Mathf.PerlinNoise(0f, t) - 0.5f),
            (Mathf.PerlinNoise(t, t) - 0.5f)
        ) * smoothShakeStrength * 10f;

        offset = ApplyDirection(offset);
        transform.localRotation = originalRotation * Quaternion.Euler(offset);
    }

    private void ShakeScale()
    {
        float t = Time.time * shakeSpeed;

        Vector3 offset = new Vector3(
            (Mathf.PerlinNoise(t, 0f) - 0.5f),
            (Mathf.PerlinNoise(0f, t) - 0.5f),
            (Mathf.PerlinNoise(t, t) - 0.5f)
        ) * smoothShakeStrength * 0.1f;

        if (uniformAxes)
        {
            float uniformValue = 0f;
            int count = 0;

            if (direction.HasFlag(Directions.X)) { uniformValue += offset.x; count++; }
            if (direction.HasFlag(Directions.Y)) { uniformValue += offset.y; count++; }
            if (direction.HasFlag(Directions.Z)) { uniformValue += offset.z; count++; }

            if (count > 0)
            {
                uniformValue /= count;

                if (direction.HasFlag(Directions.X)) offset.x = uniformValue;
                if (direction.HasFlag(Directions.Y)) offset.y = uniformValue;
                if (direction.HasFlag(Directions.Z)) offset.z = uniformValue;
            }
        }

        offset = ApplyDirection(offset);

        Vector3 newScale = originalScale + offset;
        newScale.x = Mathf.Max(0.1f, newScale.x);
        newScale.y = Mathf.Max(0.1f, newScale.y);
        newScale.z = Mathf.Max(0.1f, newScale.z);

        transform.localScale = newScale;
    }

    private Vector3 ApplyDirection(Vector3 offset)
    {
        Vector3 result = Vector3.zero;

        if (direction.HasFlag(Directions.X)) result.x = offset.x;
        if (direction.HasFlag(Directions.Y)) result.y = offset.y;
        if (direction.HasFlag(Directions.Z)) result.z = offset.z;

        return result;
    }

    void OnDisable()
    {
        if (shakeType.HasFlag(ShakeType.Position)) transform.localPosition = originalPosition;
        if (shakeType.HasFlag(ShakeType.Rotation)) transform.localRotation = originalRotation;
        if (shakeType.HasFlag(ShakeType.Scale)) transform.localScale = originalScale;
    }

    public void ShakeIt(float duration, float strenght = 0)
    {
        if (strenght == 0)
            strenght = shakeStrength;

        StopAllCoroutines();
        StartCoroutine(EnableShake(duration, strenght));
    }

    IEnumerator EnableShake(float duration, float strenght)
    {
        float startStrenght = shakeStrength;
        shakeStrength = strenght;
        active = true;
        yield return new WaitForSeconds(duration);
        active = false;
        shakeStrength = startStrenght;
        ResetToOriginal();
    }

    public void ResetToOriginal()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        transform.localScale = originalScale;
    }

}
