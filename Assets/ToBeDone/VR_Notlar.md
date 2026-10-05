# Sanal Gerçeklik Uygulamaları – Proje Notları

Unity 6.3 LTS · XR Interaction Toolkit 3.x · OpenXR · VR gözlüğü yok, **XR Interaction Simulator** ile test.

---

## 1. Proje Kurulumu

1. Unity Hub'da **VR** şablonunu indir ve proje oluştur.
2. Şablon şu paketleri hazır getirir: XR Interaction Toolkit, XR Plugin Management, OpenXR.
3. Package Manager > XR Interaction Toolkit > **Samples** sekmesinden import et:
   - Starter Assets
   - XR Interaction Simulator (eski adı XR Device Simulator)
4. `Assets/Samples/XR Interaction Toolkit/<sürüm>/XR Interaction Simulator` içindeki prefab'ı sahneye sürükle.
5. Sahnede şunlar bulunmalı: XR Origin (XR Rig), XR Interaction Manager, EventSystem, zemin (Plane), ışık.

### Paketlerin farkı

| Paket | Ne işe yarar |
|---|---|
| OpenXR Plugin | Temel OpenXR desteği (PC VR dahil). Bu ödev için yeterli. |
| Unity OpenXR: Meta | Meta Quest'e özel özellikler (passthrough, anchor vb.). |
| Unity OpenXR: Android XR | Android XR cihazlarına özel eklenti. |

---

## 2. Sahne Nesneleri

### Küpler (3 adet)
- 3D Object > Cube, Scale `0.2, 0.2, 0.2`
- **Rigidbody** (Mass ≈ 0.2) + **XR Grab Interactable**
- Ctrl+D ile çoğalt, yan yana yerleştir

### Küre
- 3D Object > Sphere, Scale `0.15, 0.15, 0.15`
- **Rigidbody** (Mass 0.5–1, Collision Detection: *Continuous Dynamic*) + **XR Grab Interactable**
- Throw On Detach: açık, Throw Velocity Scale: 3–5

### Kapı (menteşeli)
- Cube, Scale `1, 2, 0.1`, adı `Door`
- **Rigidbody** (Mass 1–2, Angular Damping 1–2)
- **Hinge Joint**: Anchor menteşe kenarında, Axis `0, 1, 0`, Use Limits açık, Min 0 / Max 100
- **XR Grab Interactable**: Movement Type *Velocity Tracking*, Throw On Detach kapalı
- Kapıya çocuk (child) bir küp ekleyip kol olarak kullan; gerekirse kolun collider'ını Grab Interactable'ın **Colliders** listesine ekle

### İsteğe bağlı: küreyi atmaya yardımcı script

```csharp
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public class BallAssistThrow : MonoBehaviour
{
    public float force = 5f;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<XRGrabInteractable>().selectExited.AddListener(_ =>
        {
            rb.AddForce(Camera.main.transform.forward * force, ForceMode.VelocityChange);
        });
    }
}
```

---

## 3. Sıkça Karşılaşılan Sorunlar

- **Nesne tutulmuyor:** Collider, Rigidbody ve XR Interaction Manager'ı kontrol et.
- **Nesneler birbirinin içinden geçiyor:** Rigidbody'de Collision Detection'ı *Continuous Dynamic* yap.
- **Kontrolcüler hareket etmiyor:** XR Interaction Simulator prefab'ı sahnede mi, Game görünümüne tıkladın mı?
- **Kapı zor açılıyor:** Mass'i düşür, kolu büyüt.

---

## 4. Sonraki Ödev: Kendi Odanı Tasarla – Kaynaklar

### Unity Learn
- **Create with VR** – ilk ders: *1.1 – VR Project Setup*. Boş sahneye oda ve dış ortam ekleyip simülatörle test etmeyi anlatır.
  - https://learn.unity.com/tutorial/1-1-vr-project-setup
- **VR Beginner: The Escape Room** – XR Interaction Toolkit ile temel VR etkileşimleri, ışınlanma noktaları, etkileşimli nesneler, ses efektleri.
  - https://learn.unity.com/course/vr-beginner-the-escape-room

### Oda tasarımı için ipuçları
1. **ProBuilder** (Package Manager): duvar, kapı boşluğu, mobilya taslağı için.
2. **Hazır modeller:** Unity Asset Store, Sketchfab, Poly Pizza, Kenney. Lisansı kontrol et, kaynak göster.
3. **Işıklandırma:** Light Baking ve Reflection Probes kullan, VR'da performans için önemli.
4. **Ölçek:** 1 Unity birimi = 1 metre. Kapı ~2 m, masa ~0.75 m, tavan ~2.5 m.
