using UnityEngine;

public class CornholeZone : MonoBehaviour
{
    public CornholeScore score;
    public bool isHole;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bag") && other.attachedRigidbody)
            score.Enter(other.attachedRigidbody, isHole);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bag") && other.attachedRigidbody)
            score.Exit(other.attachedRigidbody, isHole);
    }
}