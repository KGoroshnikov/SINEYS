using System;
using UnityEngine;

public class AutoMoveAndRotate : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3andSpace moveUnitsPerSecond;
    public Vector2 minmaxMoveMultiplier = new Vector2(1f, 1f);

    [Header("Rotation Settings")]
    public Vector3andSpace rotateDegreesPerSecond;
    public Vector2 minmaxRotateMultiplier = new Vector2(1f, 1f);

    [Header("Options")]
    public bool ignoreTimescale = false;

    private float m_LastRealTime;
    private float moveMultiplier;
    private float rotateMultiplier;

    private void Start()
    {
        moveMultiplier = UnityEngine.Random.Range(minmaxMoveMultiplier.x, minmaxMoveMultiplier.y);
        rotateMultiplier = UnityEngine.Random.Range(minmaxRotateMultiplier.x, minmaxRotateMultiplier.y);

        m_LastRealTime = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        if (ignoreTimescale)
        {
            deltaTime = Time.realtimeSinceStartup - m_LastRealTime;
            m_LastRealTime = Time.realtimeSinceStartup;
        }

        transform.Translate(moveUnitsPerSecond.value * moveMultiplier * deltaTime, moveUnitsPerSecond.space);
        transform.Rotate(rotateDegreesPerSecond.value * rotateMultiplier * deltaTime, rotateDegreesPerSecond.space);
    }

    [Serializable]
    public class Vector3andSpace
    {
        public Vector3 value;
        public Space space = Space.Self;
    }
}
