using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 

[RequireComponent(typeof(XRSimpleInteractable))]
public class PaperCrumpler : MonoBehaviour
{
    [Header("Elde Belirecek Kağıt Prefab'i")]
    [Tooltip("Buruşturulmuş kağıt prefabini buraya sürükle")]
    public GameObject crumpledPaperPrefab;

    private XRSimpleInteractable simpleInteractable;

    void Awake()
    {
        simpleInteractable = GetComponent<XRSimpleInteractable>();
        // Etkileşime (Tetik/Grip) basıldığında fonksiyonu çağır
        simpleInteractable.selectEntered.AddListener(OnPaperGrabbed);
    }

    private void OnPaperGrabbed(SelectEnterEventArgs args)
    {
        // 1. Masadaki düz kağıdın pozisyonunda yeni buruşturulmuş kağıt üret
        GameObject crumpled = Instantiate(crumpledPaperPrefab, transform.position, transform.rotation);
        
        // 2. Yeni üretilen objenin XR Grab bileşenini al
        XRGrabInteractable grabInteractable = crumpled.GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            // 3. Etkileşim Yöneticisini (Interaction Manager) çağır
            var manager = simpleInteractable.interactionManager;
            
            // 4. Düz kağıtla olan etkileşimi iptal et ve buruşturulmuş kağıdı zorla oyuncunun eline ver
            manager.CancelInteractableSelection((IXRSelectInteractable)simpleInteractable);
            manager.SelectEnter(args.interactorObject, (IXRSelectInteractable)grabInteractable);
        }
        else
        {
            Debug.LogWarning("Buruşturulmuş kağıt prefab'inde XRGrabInteractable eksik!");
        }
    }

    void OnDestroy()
    {
        // Hafıza sızıntısını (Memory Leak) önlemek için dinleyiciyi kaldır
        simpleInteractable.selectEntered.RemoveListener(OnPaperGrabbed);
    }
}