# Sanal Gerçeklik Uygulamaları

Unity ve OpenXR kullanılarak geliştirilen, sanal gerçeklik etkileşimlerini ve
küçük mini oyunları deneyimlemeye yönelik bir VR projesi.

Projede XR Interaction Toolkit ile nesne tutma, fırlatma, kapı açma ve el
etkileşimleri; ayrıca basketbol ve cornhole gibi skor tabanlı mini oyunlar
bulunur. VR gözlüğü olmadan geliştirme ve test için **XR Interaction
Simulator** kullanılabilir.

## Özellikler

- OpenXR tabanlı VR altyapısı
- XR Interaction Toolkit ile nesne etkileşimleri
- El takibi ve etkileşimli VR örnekleri
- XR Interaction Simulator ile masaüstü test desteği
- Hareketli sürgülü kapı etkileşimi
- Kağıdı buruşturup yeni nesne oluşturma etkileşimi
- Basketbol potası ve cornhole skor sistemleri
- Topların oyun alanından çıktığında başlangıç konumuna dönmesi
- Ses efektleri ve skor göstergesi

## Kullanılan teknolojiler

| Teknoloji | Sürüm |
| --- | --- |
| Unity | `6000.3.14f1` |
| Universal Render Pipeline | `17.3.0` |
| XR Interaction Toolkit | `3.5.1` |
| OpenXR Plugin | `1.17.1` |
| XR Hands | `1.8.1` |
| AR Foundation | `6.5.0` |
| Unity Input System | `1.19.0` |
| ProBuilder | `6.0.9` |

Paketlerin tamamı [Packages/manifest.json](Packages/manifest.json) üzerinden
tanımlanır ve proje Unity Hub ile açıldığında Unity tarafından yüklenir.

## Gereksinimler

- Unity Hub
- Unity `6000.3.14f1`
- OpenXR destekli bir VR cihazı veya
  XR Interaction Simulator ile test etmek için klavye/fare
- Windows üzerinde geliştirme için güncel ekran kartı sürücüleri

## Kurulum

1. Depoyu klonlayın:

   ```bash
   git clone https://github.com/Muratti216/SanalGerceklikUygulamalari.git
   cd SanalGerceklikUygulamalari
   ```

2. Unity Hub'ı açın ve **Add > Add project from disk** seçeneğiyle proje
   klasörünü ekleyin.
3. Projeyi Unity `6000.3.14f1` ile açın.
4. Unity'nin paketleri ve asset'leri içe aktarmasını bekleyin.
5. `Assets/Scenes` klasöründen bir sahne açın ve **Play** düğmesine basın.

> `Library`, `Temp`, `Logs` ve build çıktıları Git'e eklenmez. Unity bu klasörleri
> projeyi ilk açtığında yeniden oluşturur.

## Sahneler

Sahneler `Assets/Scenes` klasöründe bulunur:

- **BasicScene**: Temel VR etkileşimlerini denemek için kullanılan sahne.
- **BasicSceneCURRENT**: Temel sahnenin güncel geliştirme sürümü.
- **ZenHome**: Ev/oda temalı VR deneyimi.
- **SampleScene**: Unity'nin örnek sahnesi.

Başlangıç için `BasicSceneCURRENT` sahnesini açmanız önerilir, ders işlenilirken yapılmış olan 
mini-oyunların iskeleti burada hazır olarak bulunur ve istediğiniz gibi geliştirebilirsiniz.

## Kontroller

Kontroller kullanılan XR cihazına veya XR Interaction Simulator ayarlarına göre
değişebilir. Genel etkileşim akışı şöyledir:

- El veya kontrolcü ile nesneye yaklaşın.
- Select/Grip girdisiyle nesneyi tutun.
- Tuşu bırakarak nesneyi bırakın veya fırlatın.
- Masaüstü testinde önce Game penceresine tıklayın, ardından XR Interaction
  Simulator girdilerini kullanın.

Simulator prefab'ı sahnede yoksa XR Interaction Toolkit'in **Samples**
sekmesinden **XR Interaction Simulator** örneğini içe aktarıp sahneye ekleyin.

## Mini oyunlar ve skor sistemi

Skor sistemi `ScoreManager` üzerinden merkezi olarak yönetilir ve sahnedeki
skor metnini günceller.

- **Kağıt atma:** Buruşturulmuş kağıt `Basketball` etiketiyle alt trigger'a
  girdiğinde başarılı atış sayılır ve puan kazanır. Bu etiket mevcut prefab
  yapısında geriye dönük uyumluluk için kullanılmaktadır.
- **Hareketli kova puanlaması:** Puanlama alt trigger'a gerçekten girme olayına
  göre yapılır; bu nedenle kova kağıdın altından hareket etse bile geçerli
  bir giriş puansız kalmaz. Eski üstten geçiş şartı `BasketZone` içindeki
  `Require Top Pass` alanından isteğe bağlı açılabilir.
- **Cornhole:** Deliğe giren torba 3 puan, tahtada duran torba 1 puan
  değerindedir.
- **Skor bölgeleri:** Tanımlı etikete sahip nesneler bölgeye girdiğinde puan
  kazanır; bölgeden çıkınca puan geri alınabilir.
- **Atılan kağıtları temizleme:** `BasketZone` bileşenindeki `Destroy Scored
  Object` seçeneği açılırsa, `Successful Shots Before Destroy` değerine
  ulaşıldığında son başarılı atışta kovaya giren kağıt otomatik olarak silinir.
  Bu değer Inspector'da ayarlanabilir; sahnedeki varsayılan değer 10'dur.
- **Zorluk eğrisi:** Çöp kovasına `TrashBinMovement` bileşeni eklenerek skor
  eşiklerinde kovanın konumu değiştirilebilir. Varsayılan `3, 6, 9`
  eşikleriyle kova her eşikte yeni bir konuma geçer. Varsayılan hareket dünya
  Z eksenindedir; kovanın rotasyonu hareket yönünü değiştirmez. `Movement Axis`,
  `Movement Distance` ve `Movement Duration` değerleri Inspector'dan
  değiştirilebilir.
- **Salınım fazı:** `Enable Oscillation` açıkken kova `Oscillation Start Score`
  değerine ulaşıldığında sürekli ileri-geri hareket eder. Varsayılan olarak
  12. skordan sonra hareket başlar, kağıt atışlarını zorlaştırır ve 30. skorda
  durarak başlangıç konumuna döner. `Oscillation Distance`, `Oscillation
  Duration` ve `Return To Start At End` Inspector'dan değiştirilebilir.
- **Top sıfırlama:** Top belirlenen yüksekliğin altına düşer veya başlangıç
  konumundan çok uzaklaşırsa otomatik olarak geri getirilir.

## Proje yapısı

```text
Assets/
├── Scenes/          # VR sahneleri
├── Scripts/         # Etkileşim ve oyun mantığı
├── MiniGamesMAT/    # Mini oyun materyalleri
├── MiniGamesPrefab/ # Mini oyun prefab'ları
├── MyHomeAssets/    # Ev sahnesi asset'leri
├── Settings/        # XR ve proje ayarları
├── XR/              # XR ile ilgili asset'ler
└── XRI/             # XR Interaction Toolkit asset'leri
Packages/            # Unity paket tanımları
ProjectSettings/     # Unity proje ayarları
```

Özel oyun kodları `Assets/Scripts` altında bulunur. Başlıca betikler:

- `ScoreManager.cs`
- `ScoreZone.cs`
- `BasketZone.cs`
- `CornholeZone.cs`
- `CornholeScore.cs`
- `SlidingDoor.cs`
- `PaperCrumpler.cs`
- `BallRespawn.cs`
- `TrashBinMovement.cs`

## Katkıda bulunma

1. Bu depoyu fork'layın.
2. Yeni bir branch oluşturun:

   ```bash
   git checkout -b feature/yeni-etkilesim
   ```

3. Değişikliklerinizi yapıp Unity içinde test edin.
4. Açıklayıcı bir commit oluşturun ve pull request gönderin.

## Lisans

Bu depoda henüz ayrı bir lisans dosyası bulunmamaktadır. Projeyi yeniden
dağıtmadan veya içindeki üçüncü taraf asset'leri kullanmadan önce ilgili
asset'lerin lisanslarını kontrol edin.
