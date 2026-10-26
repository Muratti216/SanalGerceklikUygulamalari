using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSimpleInteractable))]
public class SlidingDoor : MonoBehaviour
{
    public float openDistance = 1.5f;   // eksi yaparsan ters yöne açılır

    XRSimpleInteractable interactable;
    IXRSelectInteractor interactor;
    float closedX;
    float grabOffset;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        closedX = transform.localPosition.x;
        interactable.selectEntered.AddListener(OnSelect);
        interactable.selectExited.AddListener(OnRelease);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject;
        grabOffset = transform.localPosition.x - HandLocalX();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        interactor = null;
    }

    float HandLocalX()
    {
        Vector3 hand = interactor.GetAttachTransform(interactable).position;
        return transform.parent.InverseTransformPoint(hand).x;
    }

    void Update()
    {
        if (interactor == null) return;

        float x = HandLocalX() + grabOffset;
        float a = closedX;
        float b = closedX + openDistance;
        x = Mathf.Clamp(x, Mathf.Min(a, b), Mathf.Max(a, b));

        var p = transform.localPosition;
        p.x = x;
        transform.localPosition = p;
    }
}