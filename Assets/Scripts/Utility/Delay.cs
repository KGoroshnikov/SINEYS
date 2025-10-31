using System;
using System.Collections;
using UnityEngine;

public static class Delay
{
    private class DelayRunner : MonoBehaviour { }

    private static DelayRunner runner;

    private static void InitRunner()
    {
        if (runner != null) return;

        GameObject runnerObj = new GameObject("~DelayRunner");
        runner = runnerObj.AddComponent<DelayRunner>();
    }

    public static void InvokeDelayed(Action action, float delay)
    {
        InitRunner();
        runner.StartCoroutine(InvokeCoroutine(action, delay));
    }

    private static IEnumerator InvokeCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
