using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ScoreZone : MonoBehaviour
{
    public string requiredTag = "ScoreObject";
    public int points = 10;
    public bool loseOnExit = true;
    public AudioSource sound;

    readonly HashSet<Collider> scored = new();

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(requiredTag) || scored.Contains(other)) return;

        var grab = other.GetComponentInParent<XRGrabInteractable>();
        if (grab != null && grab.isSelected) return; // hâlâ elde tutuluyor

        scored.Add(other);
        ScoreManager.Instance.Add(points);
        if (sound) sound.Play();
    }

    void OnTriggerExit(Collider other)
    {
        if (loseOnExit && scored.Remove(other))
            ScoreManager.Instance.Add(-points);
    }
}