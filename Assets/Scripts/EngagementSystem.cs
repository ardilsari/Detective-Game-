using UnityEngine;
using System.Collections.Generic;

public class EngagementSystem : MonoBehaviour
{
    public static EngagementSystem instance;

    [System.Serializable]
    public class EngagementMetrics
    {
        public float addictionLevel; // 0-100 (Ne kadar bağımlı)
        public float playtimeMultiplier; // Oyun süresini uzatan faktör
        public float rewardDopamine; // Ödül hormonu simülasyonu
        public int consecutivePlaySessions; // Ardışık oturum sayısı
        public float nextRewardCountdown; // Sonraki ödüne kadar bekleme
    }

    [System.Serializable]
    public class CliffhangerEvent
    {
        public int eventId;
        public string title;
        public string description;
        public float triggerTime; // Bölüm sonundan kaç dakika önce tetiklen
        public bool isTriggered;
    }

    [System.Serializable]
    public class PsychologicalMechanic
    {
        public string mechanicName;
        public string description;
        public float engagementBoost; // Ne kadar bağımlılık artırır
        public bool isActive;
    }

    private EngagementMetrics metrics;
    private List<CliffhangerEvent> cliffhangers = new List<CliffhangerEvent>();
    private List<PsychologicalMechanic> mechanics = new List<PsychologicalMechanic>();
    
    private float sessionStartTime;
    private int totalSessionsCompleted = 0;
    private float lastRewardTime;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeEngagementSystem();
        InitializeCliffhangers();
        InitializePsychologicalMechanics();
        sessionStartTime = Time.time;
    }

    private void InitializeEngagementSystem()
    {
        metrics = new EngagementMetrics
        {
            addictionLevel = 0f,
            playtimeMultiplier = 1.0f,
            rewardDopamine = 0f,
            consecutivePlaySessions = 0,
            nextRewardCountdown = 300f // 5 dakika
        };

        Debug.Log("✓ Engagement Sistemi Başlatıldı");
    }

    private void InitializeCliffhangers()
    {
        // Her bölümün sonunda cliffhanger'lar
        cliffhangers.Add(new CliffhangerEvent
        {
            eventId = 1,
            title = "Beklenmedik Suçlu",
            description = "Bulduğun suçlu aslında kurban olmaya hazırlanmış bir köpek mu? Sonraki bölümde gerçeği öğren!",
            triggerTime = 5f, // Son 5 dakika
            isTriggered = false
        });

        cliffhangers.Add(new CliffhangerEvent
        {
            eventId = 2,
            title = "Kara Para Çevresi",
            description = "Tanıklar arasında korkunç bir bağlantı var. Kara para operasyonu ortaya çıkıyor!",
            triggerTime = 10f,
            isTriggered = false
        });

        cliffhangers.Add(new CliffhangerEvent
        {
            eventId = 3,
            title = "Mütercim Tercüman Gizli",
            description = "Polis müdürü katıl mı? Gizli belgeleri buldunuz. Ama o dosyadan para akıyor!",
            triggerTime = 8f,
            isTriggered = false
        });

        cliffhangers.Add(new CliffhangerEvent
        {
            eventId = 4,
            title = "Gerçek Komplocu",
            description = "Tüm cinayetler planlı mıydı? Arka plandaki gerçek güç kim?",
            triggerTime = 12f,
            isTriggered = false
        });

        cliffhangers.Add(new CliffhangerEvent
        {
            eventId = 5,
            title = "Aile Gizemi",
            description = "Suçlu senin yakını mı? Ailenin karanlık sırları ortaya çıkıyor!",
            triggerTime = 7f,
            isTriggered = false
        });

        cliffhangers.Add(new CliffhangerEvent
        {
            eventId = 6,
            title = "Zaman Tuzağı",
            description = "Tüm kanıtlar yanlış! Gerçek cinayet tarihi farklı mı?",
            triggerTime = 15f,
            isTriggered = false
        });

        Debug.Log("✓ " + cliffhangers.Count + " Cliffhanger Etkinliği Yüklendi");
    }

    private void InitializePsychologicalMechanics()
    {
        // Bağımlılık mekanikleri
        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Ödül Gecikme",
            description = "Oyuncu ipucu buldukça beklemesi gerek (dopamin spike)",
            engagementBoost = 15f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Streaks (Oturum Serisi)",
            description = "Her gün oynadığında bonus puan. Seri bozulmasını istemiyorsun!",
            engagementBoost = 25f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "FOMO (Kaçırma Korkusu)",
            description = "Bölüm 2 yarın kilit açılacak! Şimdi bitmez mi?",
            engagementBoost = 30f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Cliffhanger Teaser",
            description = "Bölüm sonunda kararsız çıkıyor. Sonraki bölümü HAYAL et!",
            engagementBoost = 35f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Kademeli Zorluk Artışı",
            description = "Her bölüm biraz daha zor. İstediğin challenge var!",
            engagementBoost = 20f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Başarı Kilidi",
            description = "Tüm başarıları açmak için oyunun tüm içeriğini oynamalısın!",
            engagementBoost = 28f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Gizli Senaryolar",
            description = "Bazı kararlar gizli sahnelere açıyor. Replay ister misin?",
            engagementBoost = 32f,
            isActive = true
        });

        mechanics.Add(new PsychologicalMechanic
        {
            mechanicName = "Puan Liderlik",
            description = "Arkadaşlarınızdan daha yüksek puan alabilir misiniz?",
            engagementBoost = 22f,
            isActive = false // Steam/Online entegrasyonu gerekir
        });

        Debug.Log("✓ " + mechanics.Count + " Psikolojik Mekanik Hazırlandı");
    }

    private void Update()
    {
        UpdateEngagementMetrics();
        CheckCliffhangerTriggers();
        UpdateRewardSystem();
    }

    private void UpdateEngagementMetrics()
    {
        float sessionDuration = (Time.time - sessionStartTime) / 60f; // dakika

        // Oyuncu ne kadar uzun oynuyorsa addiction artıyor
        metrics.addictionLevel = Mathf.Min(100f, sessionDuration * 0.5f);

        // Oturum sayısı arttıkça multiplier artıyor
        metrics.playtimeMultiplier = 1.0f + (totalSessionsCompleted * 0.1f);

        // Dopamine efekti: Ödülleri aşamalı ver
        if (Time.time - lastRewardTime > metrics.nextRewardCountdown)
        {
            TriggerDopamineReward();
        }
    }

    private void CheckCliffhangerTriggers()
    {
        ChapterManager chapterMgr = ChapterManager.instance;
        if (chapterMgr == null) return;

        Chapter currentChapter = chapterMgr.GetCurrentChapter();
        if (currentChapter == null) return;

        // Bölümün son X dakikasında cliffhanger göster
        // TODO: Gerçek countdown ile entegre et
    }

    private void TriggerDopamineReward()
    {
        // Rastgele ödül ver (dopamin spike)
        int randomReward = Random.Range(50, 200);
        metrics.rewardDopamine = Mathf.Min(100f, metrics.rewardDopamine + 25f);

        Debug.Log("💰 Bonus Puan: +" + randomReward);
        Debug.Log("🧠 Dopamine Seviyesi: " + metrics.rewardDopamine);

        lastRewardTime = Time.time;
        metrics.nextRewardCountdown = Random.Range(180f, 420f); // 3-7 dakika sonraki ödül
    }

    private void UpdateRewardSystem()
    {
        // Ödül sistemini dinamik olarak güncelle
        if (metrics.consecutivePlaySessions >= 7)
        {
            Debug.Log("🏆 HEFTALIKİ STREAKİ BAŞARDIN! +500 BONUS PUAN!");
            metrics.consecutivePlaySessions = 0;
        }
    }

    public void TriggerCliffhanger(int chapterId)
    {
        if (chapterId > 0 && chapterId <= cliffhangers.Count)
        {
            CliffhangerEvent cliffhanger = cliffhangers[chapterId - 1];
            
            Debug.Log("\n╔════════════════════════════════════╗");
            Debug.Log("║         BÖLÜM SONU TEASER          ║");
            Debug.Log("╠════════════════════════════════════╣");
            Debug.Log("║ " + cliffhanger.title);
            Debug.Log("║");
            Debug.Log("║ " + cliffhanger.description);
            Debug.Log("╚════════════════════════════════════╝\n");

            cliffhanger.isTriggered = true;

            // UI'da göster (Canvas animasyonu)
            ShowCliffhangerAnimation(cliffhanger);
        }
    }

    private void ShowCliffhangerAnimation(CliffhangerEvent cliffhanger)
    {
        // TODO: Canvas üzerinde fade-in/fade-out animasyonu
        // 3 saniye bekle, sonra kaybol
        // Ses efekti: Dramatic music sting
    }

    public void CompletePlaySession()
    {
        float sessionDuration = (Time.time - sessionStartTime) / 60f; // dakika
        totalSessionsCompleted++;
        metrics.consecutivePlaySessions++;

        Debug.Log("\n=== OTURUMİ TAMAMLADINIZ ===");
        Debug.Log("Oturum Süresi: " + Mathf.Round(sessionDuration) + " dakika");
        Debug.Log("Toplam Bağımlılık Seviyesi: " + Mathf.Round(metrics.addictionLevel) + "/100");
        Debug.Log("Ardışık Oturum: " + metrics.consecutivePlaySessions + " gün");
        Debug.Log("=============================\n");

        SaveEngagementData();
    }

    public void ShowNextChapterTeaser()
    {
        // Sonraki bölümü teaser yap
        ChapterManager chapterMgr = ChapterManager.instance;
        if (chapterMgr == null) return;

        Chapter nextChapter = chapterMgr.GetCurrentChapter();
        if (nextChapter == null) return;

        Debug.Log("\n🎬 SONRAKI BÖLÜM FRAGMANI 🎬");
        Debug.Log("Başlık: " + nextChapter.chapterName);
        Debug.Log("'" + nextChapter.chapterDescription + "'");
        Debug.Log("\n⏱️ YAKINLARDA GELİYOR...\n");
    }

    public float GetAddictionLevel() => metrics.addictionLevel;
    public float GetPlaytimeMultiplier() => metrics.playtimeMultiplier;
    public int GetConsecutiveSessions() => metrics.consecutivePlaySessions;

    private void SaveEngagementData()
    {
        string json = JsonUtility.ToJson(metrics, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/EngagementData.json", json);
    }

    public void LoadEngagementData()
    {
        string path = Application.persistentDataPath + "/EngagementData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            metrics = JsonUtility.FromJson<EngagementMetrics>(json);
        }
    }
}
