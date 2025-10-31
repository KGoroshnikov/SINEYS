using UnityEngine;

public class ShakeSpread : MonoBehaviour
{
    public float duration = 0.5f;
    public float amount = 0.1f;

    public float minDistance = 1f;
    public float maxDistance = 5f;

    public bool onAwake;

    private void OnEnable()
    {
        if (onAwake)
            Shake();
    }

    public void Shake()
    {
        Transform player = G.gm.player.transform;
        
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance > maxDistance) return;

        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);
        float influence = Mathf.Clamp01(t);

        G.shaker.ShakeIt(duration, amount * influence);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.7f);
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        Gizmos.color = new Color(0f, 1f, 0f, 0.7f);
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}
