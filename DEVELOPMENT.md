# Detective Game - Geliştirme Rehberi

## Kurulum Adımları

### 1. Unity Kurulumu
```bash
# Unity 2021 LTS veya daha yeni sürümü indir
# https://unity.com/download
```

### 2. Projeyi Klonla
```bash
git clone https://github.com/ardilsari/Detective-Game-.git
cd Detective-Game-
```

### 3. Unity'de Aç
- Unity Hub'ı aç
- "Add" tıkla
- Detective-Game- klasörünü seç
- Proje açılacak

---

## Proje Yapısı

```
Detective-Game-/
├── Assets/
│   ├── Scripts/
│   │   ├── GameManager.cs          ← Ana oyun kontrolü
│   │   ├── CaseData.cs             ← Olay verileri yapısı
│   │   ├── WitnessSystem.cs        ← Tanık sorgulama
│   │   ├── EvidenceSystem.cs       ← Kanıt toplama
│   │   ├── UIManager.cs            ← UI kontrolü
│   │   └── DataLoader.cs           ← JSON veri yükleme
│   ├── Data/
│   │   └── CasesData.json          ← Örnek olay verileri
│   ├── Scenes/
│   │   └── MainGame.unity          ← Ana oyun sahne
│   └── Resources/
├── ProjectSettings/
└── README.md
```

---

## Sistem Açıklaması

### 1. **CaseData.cs** - Olay Yönetimi
- Tanıklar, kanıtlar, şüpheliler için veri yapıları
- Statik metotlar ile verilere erişim

```csharp
// Örnek kullanım
Case currentCase = CaseData.GetCase();
Witness witness = CaseData.GetWitness(1);
Evidence evidence = CaseData.GetEvidence(1);
```

### 2. **GameManager.cs** - Ana Kontrolü
- Oyunun genel akışını yönetir
- Gün sayısı, puan sistemi
- Olay çözümü denetimi

```csharp
// Tanık sorgula
GameManager.instance.InterrogateWitness(1);

// Kanıt topla
GameManager.instance.CollectEvidence(1);

// Suçluyu suçla
GameManager.instance.AccuseSuspect(1);
```

### 3. **WitnessSystem.cs** - Tanık Sistemi
- Tanıkları sorgula
- İfadeleri göster
- İpuçları ver

### 4. **EvidenceSystem.cs** - Kanıt Sistemi
- Kanıtları topla
- Kanıtları tanıklarla bağla
- Detayları göster

### 5. **UIManager.cs** - Arayüz
- Detective masası UI
- Tanık sorgulama paneli
- Kanıt analiz paneli
- Şüpheli seçim paneli

### 6. **DataLoader.cs** - Veri Yükleme
- JSON dosyasından olay yükle
- Verileri serialize/deserialize et

---

## Oyun Mekanikler

### Tanık Sorgulama
1. Tanık listesinden birini seç
2. İfadeleri oku
3. İpuçları topla
4. Kanıtlarla bağla

### Kanıt Toplama
1. Kanıt listesinden birini seç
2. Detayları oku
3. Tanıklarla eşleştir
4. Puan kazan

### Suçluyu Bulma
1. Tüm tanıkları sorgula
2. Tüm kanıtları topla
3. Kanıtları analiz et
4. Şüpheli seç ve suçla

### Puan Sistemi
- Tanık sorgulama: +5 puan
- Kanıt toplama: +10 puan
- Doğru sonuç: +100 puan
- Yanlış sonuç: -50 puan

---

## Veri Formatı (JSON)

```json
{
  "cases": [
    {
      "caseId": 1,
      "caseName": "Olay Adı",
      "daysToSolve": 10,
      "correctSuspectId": 1,
      "witnesses": [...],
      "evidences": [...],
      "suspectProfiles": [...]
    }
  ]
}
```

---

## Yeni Olay Ekleme

1. `Assets/Data/CasesData.json` dosyasını aç
2. Yeni olay nesnesini `cases` dizisine ekle
3. Tanıkları, kanıtları, şüphelileri doldur
4. `correctSuspectId` ile doğru şüpheliyi işaretle

---

## Özellikler (TODO)

- [ ] Grafik ve görseller
- [ ] Ses ve müzik
- [ ] Animasyonlar
- [ ] Daha fazla olay
- [ ] Multiplayer
- [ ] Achievements sistemi
- [ ] Leaderboard
- [ ] Steam integrasyonu

---

## Derleme ve Yayınlama

### PC Build
```
File → Build Settings → PC, Mac & Linux Standalone → Build
```

### Steam Yayıncılığı
1. Steamworks hesabı oluştur
2. Uygulama kimliği al
3. Oyunu yükle
4. Onay bekle

---

## Sorun Giderme

### JSON Yükleme Hatası
- `Assets/Resources/` klasöründe olduğundan emin ol
- Dosya adını `.json` uzantısı olmadan yazma
- JSON formatını doğrula (JSONLint kullan)

### UI Gösterilmiyor
- Canvas objesinin Scene'de olduğundan emin ol
- UIManager referanslarını ata
- Console'da hataları kontrol et

### Oyun Çöküyor
- Unity version uyumluluğunu kontrol et
- Tüm scriptleri derle
- Missing references kontrol et

---

## Kontribüsyon

Geliştirilmesine yardımcı olmak isterseniz:
1. Fork et
2. Feature branch oluştur (`git checkout -b feature/yeni-olay`)
3. Commit et (`git commit -m 'Yeni olay eklendi'`)
4. Push et (`git push origin feature/yeni-olay`)
5. Pull Request aç

---

**Başarılar! Detective Game'i geliştirmeyi dileriz!** 🔍🎮
