# 🕵️ Detective Game - Çözülmemiş Cinayetler (Mystery Unsolved)

Tarihinin en meşhur çözülmemiş cinayetlerini çöz! **7 bölüm**, **50+ saat oyun süresi**, **branching storylines** ve **gizli içerik** ile dolu büyük bir detective/mystery oyunu.

---

## 🎮 Oyun Özeti

**Detective Game**, oyuncuları tarihinin en gizemli cinayetlerini araştırma rolüne sokan bir **narrative-driven mystery oyunudur**.

### Ana Özellikler:
- ✅ **7 Gerçek Cinayeti İnceleme** - Jack the Ripper, Lizzie Borden, Black Dahlia, Axeman, Hinterkaifeck, Villisca, ve 7. Bölüm Gizem
- ✅ **150+ Karar Yolu** - Her seçim sonucu değiştirir
- ✅ **15+ Başarı** + **5 Global Challenge** - Tekrar oynama değeri
- ✅ **Gizli İçerik** - 10/10 zorluk seviyeleri
- ✅ **Leaderboard Sistemi** - Steam, Discord, Twitter entegrasyonu
- ✅ **Çoklu Ending'ler** - Her oyun farklı sonuçlandırılabilir
- ✅ **Türkçe Tam Lokalizasyon** - Diyaloglar, UI, sesler

---

## 📋 Yapı (Architecture)

### Tamamlanan Sistemler (✅ Tier 1)

```
Assets/Scripts/
├── Core Systems
│   ├── GameManager.cs                    ← Ana oyun kontrol
│   ├── ChapterManager.cs                 ← 7 Bölüm sistemi
│   └── WitnessSystem.cs                  ← Tanık/Şüpheli yönetimi
│
├── Engagement Systems
│   ├── EngagementSystem.cs              ← Bağımlılık mekanikleri (daily rewards, streaks)
│   ├── AchievementSystem.cs             ← 15+ Başarı + Challenge Modes
│   └── SocialLeaderboardSystem.cs       ← Leaderboard + Global Challenges
│
├── Narrative Systems
│   └── StoryBranchingSystem.cs          ← 150+ Karar yolu + Gizli İçerik + Ending'ler
│
├── UI Systems
│   └── UICanvasManager.cs               ← Menu, HUD, Leaderboard, Settings
│
├── Scene/World
│   └── SceneManager.cs                  ← Chapter Sahneleri + Lokasyonlar
│
└── Audio Systems
    └── AudioManager.cs                  ← Müzik, SFX, Voice Lines, Volume Control
```

### Veri Dosyaları
```
Assets/Data/
├── RealCasesChapters.json              ← 7 Gerçek Cinayetin Detayları
├── CharacterDatabase.json              ← Karakterler, Profiller
├── EvidenceDatabase.json               ← Kanıtlar, İpuçları
└── DialogueScripts.json                ← Diyaloglar ve Voice Lines
```

---

## 🎯 Sistem Açıklamaları

### 1️⃣ ChapterManager (7 Bölüm Sistemi)
```csharp
// Her bölüm:
- 6-8 saat oyun süresi
- 20-30 tanık/şüpheli
- 100+ kanıt
- 25-40 karar noktası
- Multiple endings

// Bölümler:
1. Jack the Ripper (1888) - Londra
2. Lizzie Borden (1892) - ABD
3. Black Dahlia (1947) - Hollywood
4. Axeman (1918) - New Orleans
5. Hinterkaifeck (1921) - Bavyera
6. Villisca (1912) - Iowa
7. Seri Katil Ağı - Global Bağlantılar
```

### 2️⃣ AchievementSystem (Başarılar & Challenge)
```
BAŞARILAR (15+):
├── Bölüm Tamamlama (7) - Her bölüm = 1 başarı
├── Dedektif Rütbesi (3) - Başlayan, Seçkin, Master
├── İnceleme Başarıları (4) - İpucu, Kanıt, Tanık
├── Hız Başarıları (3) - 1 saat, 45 dk, 30 dk
├── Mükemmel Oyun (2) - Hiç hata, tüm görevler
└── Gizli Başarılar (2) - Gizli sahneler, 7. bölüm

CHALLENGE MODLARI (5):
├── Haftalık - Hızlı Çözüm
├── Aylık - Master Tarihçi
├── Özel - Blind Mode (İpucu yok)
└── Seri Katil Ağı - Tüm cinayetleri bağla
```

### 3️⃣ StoryBranchingSystem (Karar Yolları)
```
Özellikler:
- 150+ Karar Noktası
- Geri Dönülemez Kararlar (Irrevokable)
- Gizli İçerik Açma Mekanizmi
- 7 Farklı Ending (Bölüm başına)
- Secret Content (10/10 zorluk)

Gizli İçerik Örnekleri:
- Jack the Ripper'ın Gerçek Kimliği
- Lizzie Borden'ın Günlüğü
- Black Dahlia Hollywood Bağlantıları
- 7. Bölüm - Seri Katil Ağı
```

### 4️⃣ SocialLeaderboardSystem (Sosyal Özellikler)
```
LEADERBOARD:
- Global Top 100 (Puanlar)
- Arkadaş Leaderboard
- Bölüm Bazlı Records
- Completion Time Ranking

GLOBAL CHALLENGES:
- Haftalık Challenge
- Aylık Challenge
- Özel Challenge (Blind Mode)
- Sezonal Challenge

SOSYAL MEDYA:
- Steam Integration (Başarılar, Leaderboard)
- Discord Rich Presence (Şu an oynadığı bölüm)
- Twitter Sharing (Screenshot + Mesaj)
```

### 5️⃣ UICanvasManager (Arayüz)
```
PANELLAR:
├── Main Menu (5 Buton)
├── Chapter Select (7 Bölüm)
├── Gameplay HUD (Puan, Timer, Bölüm Adı)
├── Interrogation (Tanık Sorgusu)
├── Evidence Panel (Kanıtlar)
├── Leaderboard (Top 10)
├── Achievements (Grid View)
└── Settings (Ses, Grafik, Dil)

UI ÖZELLIKLERI:
- Responsive Design (1920x1080 referansı)
- Real-time Score Update
- Timer Gösterimi
- Notification System
- Achievement Pop-ups
```

### 6️⃣ SceneManager (Sahne & Lokasyonlar)
```
CHAPTER SAHNELERI:
1. Chapter1_Whitechapel (8 Lokasyon)
2. Chapter2_FallRiver (8 Lokasyon)
3. Chapter3_Hollywood (8 Lokasyon)
4. Chapter4_NewOrleans (8 Lokasyon)
5. Chapter5_Bavyera (8 Lokasyon)
6. Chapter6_Villisca (8 Lokasyon)
7. Chapter7_GlobalConnections (8 Lokasyon)

LOKASYON SİSTEMİ:
- Spawn Position
- Camera Position
- Available Characters
- Available Evidence
- Environment Audio
- Visited Tracking
```

### 7️⃣ AudioManager (Ses Sistemi)
```
AUDIO SOURCES (4):
├── BGM Source (0 Priority) - Müzik
├── SFX Source (50 Priority) - Efektler
├── Voice Source (25 Priority) - Konuşmalar
└── Ambient Source (75 Priority) - Ortam Sesi

MÜZIK TEMALARı (7+):
- Victorian London Theme
- Victorian America Theme
- 1940s Jazz
- Jazz Blues 1918
- Dark Bavarian Folk
- Grim Americana
- Epic Finale
- Main Menu Theme

VOICE LINES:
- Character Dialogues (8+ Karakter)
- Interrogation Scripts
- Evidence Commentary
- Decision Consequences

SES EFEKTLERİ:
- Footsteps, Door Knocks
- Evidence Pickup
- Achievement Unlock
- Decision Made
- Mystery Solved
- Notification Ping

VOLUME KONTROL:
- Master Volume
- BGM Volume
- SFX Volume
- Voice Volume
- Mute Toggle
```

---

## 🏗️ Proje Kurulumu

### Gereksinimler:
- **Unity 2022 LTS+**
- **Visual Studio Community** (C# IDE)
- **Git** (versiyon kontrolü)

### Kurulum Adımları:
```bash
# Repository klonla
git clone https://github.com/ardilsari/Detective-Game-.git

# Unity'de aç
# File → Open Project → Detective-Game- seç

# Scene'leri yükle
# Assets/Scenes/ → MainMenu scene'i çalıştır
```

---

## 📊 Oyun İstatistikleri

| Metrik | Değer |
|--------|-------|
| Toplam Bölüm Sayısı | 7 |
| Bölüm Başına Süresi | 6-8 saat |
| Toplam Oyun Süresi | 50+ saat |
| Karar Noktaları | 150+ |
| Karakterler | 40+ |
| Kanıtlar | 100+ |
| Başarılar | 15+ |
| Possible Endings | 20+ |
| Gizli İçerik | 10+ (Zorluk: 7-10/10) |
| Global Challenges | 5 |
| Leaderboard Entries | Top 100 |

---

## 🎬 Kullanım Örneği

```csharp
// Bölüm Başlat
ChapterManager.instance.LoadChapter(1);

// Karar Ver
StoryBranchingSystem.instance.MakeDecision(101);

// Başarı Aç
AchievementSystem.instance.UnlockAchievement(1001);

// Leaderboard Kontrol
SocialLeaderboardSystem.instance.ViewGlobalLeaderboard();

// Müzik Çal
AudioManager.instance.PlayBackgroundMusic("Music_Victorian_London");

// Sahne Yükle
SceneManager.instance.LoadChapter(1);
SceneManager.instance.TravelToLocation("Whitechapel_Street");
```

---

## 📁 Veri Formatları

### RealCasesChapters.json Örneği:
```json
{
  "chapters": [
    {
      "chapterId": 1,
      "caseName": "Jack the Ripper",
      "location": "Whitechapel, London",
      "year": 1888,
      "victims": 5,
      "suspects": ["Dr. Druitt", "Aaron Kosminski", "Montague Druitt"],
      "realHistoryWiki": "https://en.wikipedia.org/wiki/Jack_the_Ripper",
      "evidence": ["Victim Photos", "Police Reports", "Medical Records"]
    }
  ]
}
```

---

## 🚀 Gelecek Özellikler (Tier 2-3)

### Tier 2 (Yazılmayı bekliyor):
- [ ] Character System (3D modeller, animasyonlar)
- [ ] Quest/Objective Tracker
- [ ] Save/Load System
- [ ] Advanced Hint System

### Tier 3:
- [ ] Mobile Support (Android/iOS)
- [ ] Multiplayer Leaderboard (Online)
- [ ] DLC Packs (Yeni cinayetler)

### Tier 4 (Platform):
- [ ] Steam Achievements
- [ ] Discord Integration
- [ ] Cloud Save

---

## 📝 Kontrol Şeması (Planlanan)

| Aksiyon | Tuş/Düğme |
|---------|-----------|
| Menüde Seçim | Mouse / Gamepad |
| Konuşmacıyı Atla | Space / A Butonu |
| Kanıt Detayı | Right Click / X Butonu |
| Karar Seç | Click / D-Pad |
| Ayarları Aç | ESC / Start |
| Leaderboard | Tab / LB |

---

## 🎨 Tasarım Felsefesi

**Detective Game**, şu ilkelere dayanarak tasarlanmıştır:

1. **Tarihsel Doğruluk** - Gerçek cinayetleri araştırırız
2. **Oyuncu Seçimi** - Her karar önemlidir
3. **Tekrar Oynama Değeri** - Gizli içerik ve multiple ending'ler
4. **Sosyal Bağlantı** - Leaderboard ve challenge'lar
5. **Bağımlılık Mekanikleri** - Daily rewards, streaks, seasonal events
6. **Erişilebilirlik** - Türkçe tam lokalizasyon, sesli diyaloglar

---

## 📊 Geliştirme İlerleme

```
✅ TAMAMLANDI (Tier 1)
├── ✅ ChapterManager (7 Bölüm)
├── ✅ EngagementSystem (Daily Rewards, Streaks)
├── ✅ AchievementSystem (15+ Başarı + Challenge)
├── ✅ StoryBranchingSystem (150+ Karar, Gizli İçerik)
├── ✅ SocialLeaderboardSystem (Leaderboard + Challenges)
├── ✅ UICanvasManager (Menu, HUD, Settings)
├── ✅ SceneManager (7 Chapter Sahne)
└── ✅ AudioManager (Müzik, SFX, Voice)

🚧 YAPILIYOR (Tier 2)
├── Character System (3D Modeller)
├── Animation System
├── Quest Tracker
├── Save/Load System
└── Hint System

⏳ PLANLANDI (Tier 3-4)
├── Mobile Support
├── Steam Integration
├── Discord Integration
└── Online Leaderboard
```

---

## 🤝 Katkıda Bulunma

Bu proje açık kaynak değildir. Ancak bug raporları ve öneriler hoş karşılanır!

---

## 📞 İletişim

**Geliştirici:** Ardil Sarı  
**Email:** sariardil3@gmail.com  
**GitHub:** [@ardilsari](https://github.com/ardilsari)

---

## 📜 Lisans

Özel Lisans - Tüm hakları saklıdır. © 2026 Detective Game

---

## 🎮 Oynamaya Başla!

```bash
# Ana menüyü aç ve ilk bölümü seç
Unity Editor'de Play butonuna bas
```

**Gerçekleri açığa çıkar. Cinayetleri çöz. Tarihçi ol.** 🕵️‍♂️

---

*Last Updated: 01 Ağustos 2026*
*Game Build: v0.3.0 (Core Systems Complete)*
