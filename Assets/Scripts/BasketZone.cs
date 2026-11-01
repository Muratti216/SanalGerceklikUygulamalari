using System.Collections.Generic;
using UnityEngine;

public class BasketZone : MonoBehaviour
{
    public bool isTop;            // üst bölge mi?
    public BasketZone topZone;    // sadece alt bölgede: üst bölgeyi buraya bağla
    [Tooltip("Açılırsa nesnenin önce üst trigger'dan geçmesi gerekir.")]
    public bool requireTopPass = false;
    public int points = 1;
    public AudioSource sound;
    [Header("Atılan nesneyi temizleme")]
    public bool destroyScoredObject = false;
    [Min(1)] public int successfulShotsBeforeDestroy = 10;

    readonly HashSet<Rigidbody> passedTop = new();
    readonly HashSet<Rigidbody> scored = new();
    int successfulShots;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Basketball") || !other.attachedRigidbody) return;
        var rb = other.attachedRigidbody;

        if (isTop)
        {
            if (rb.linearVelocity.y < 0) passedTop.Add(rb);
        }
        else if ((!requireTopPass || (topZone != null && topZone.Consume(rb))) &&
                 scored.Add(rb))
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.Add(points);
            if (sound) sound.Play();

            if (destroyScoredObject)
            {
                successfulShots++;
                if (successfulShots >= successfulShotsBeforeDestroy)
                {
                    successfulShots = 0;
                    Destroy(rb.gameObject);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isTop && other.attachedRigidbody)
            scored.Remove(other.attachedRigidbody);

        // Üst bölgeden yukarı çıkan top geçersiz sayılsın
        if (isTop && other.attachedRigidbody && other.attachedRigidbody.linearVelocity.y > 0)
            passedTop.Remove(other.attachedRigidbody);
    }

    public bool Consume(Rigidbody rb) => passedTop.Remove(rb);
}