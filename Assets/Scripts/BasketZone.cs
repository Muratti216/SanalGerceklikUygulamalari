using System.Collections.Generic;
using UnityEngine;

public class BasketZone : MonoBehaviour
{
    public bool isTop;            // üst bölge mi?
    public BasketZone topZone;    // sadece alt bölgede: üst bölgeyi buraya bağla
    public int points = 1;
    public AudioSource sound;

    readonly HashSet<Rigidbody> passedTop = new();

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Basketball") || !other.attachedRigidbody) return;
        var rb = other.attachedRigidbody;

        if (isTop)
        {
            if (rb.linearVelocity.y < 0) passedTop.Add(rb);
        }
        else if (topZone != null && topZone.Consume(rb))
        {
            ScoreManager.Instance.Add(points);
            if (sound) sound.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Üst bölgeden yukarı çıkan top geçersiz sayılsın
        if (isTop && other.attachedRigidbody && other.attachedRigidbody.linearVelocity.y > 0)
            passedTop.Remove(other.attachedRigidbody);
    }

    public bool Consume(Rigidbody rb) => passedTop.Remove(rb);
}