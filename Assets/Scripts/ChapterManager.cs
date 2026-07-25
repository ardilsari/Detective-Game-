using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Chapter
{
    public int chapterId;
    public string chapterName;
    public string chapterDescription;
    public int estimatedPlayTime; // dakika cinsinden (120 = 2 saat)
    public int difficultyLevel; // 1-7
    public Case mainCase;
    public List<SubMission> subMissions;
    public RewardSystem rewards;
    public bool isUnlocked;
    public bool isCompleted;
}

[System.Serializable]
public class SubMission
{
    public int missionId;
    public string missionName;
    public string objective;
    public bool isCompleted;
    public int rewardPoints;
    public List<string> requiredActions; // Yapılması gereken aksiyonlar
}

[System.Serializable]
public class RewardSystem
{
    public int experiencePoints;
    public int unlockedAbilities; // Yeni yetenekler
    public List<string> unlockedTools; // Araç-gereç kilidini aç
    public string nextChapterUnlock;
    public List<Achievement> achievements;
}

[System.Serializable]
public class Achievement
{
    public int achievementId;
    public string name;
    public string description;
    public bool isUnlocked;
    public int rewardPoints;
}

[System.Serializable]
public class PlayerProgress
{
    public int currentChapter;
    public int totalExperience;
    public int currentLevel;
    public Dictionary<int, ChapterProgress> chapterProgresses;
    public List<Achievement> unlockedAchievements;
    public Dictionary<string, bool> unlockedTools;
}

[System.Serializable]
public class ChapterProgress
{
    public int chapterId;
    public float completionPercentage;
    public int mainCaseStatus; // 0: Not Started, 1: In Progress, 2: Completed (Correct), 3: Completed (Wrong)
    public List<bool> subMissionsStatus;
    public int chapterScore;
    public int playTime; // dakika
}

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance;
    
    private List<Chapter> chapters = new List<Chapter>();
    private Chapter currentChapter;
    private PlayerProgress playerProgress;
    
    private int totalPlayTime = 0;
    private float engagementMultiplier = 1.0f; // Bağımlılık faktörü

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
        InitializeChapters();
        LoadPlayerProgress();
    }

    private void InitializeChapters()
    {
        // Her bölüm yaklaşık 2 saat (120 dakika)
        for (int i = 1; i <= 7; i++)
        {
            Chapter chapter = new Chapter
            {
                chapterId = i,
                chapterName = "Bölüm " + i + ": " + GetChapterTitle(i),
                chapterDescription = GetChapterDescription(i),
                estimatedPlayTime = 120, // 2 saat
                difficultyLevel = i,
                isUnlocked = (i == 1), // Sadece ilk bölüm baştan açık
                isCompleted = false,
                subMissions = CreateSubMissions(i),
                rewards = CreateRewards(i)
            };

            chapters.Add(chapter);
        }

        Debug.Log("✓ 7 Bölüm başarıyla oluşturuldu!");
    }

    private string GetChapterTitle(int chapterId)
    {
        switch (chapterId)
        {
            case 1: return "Londra'nın Gizemi";
            case 2: return "Karanlık Sokaklar";
            case 3: return "Adli Laboratuvar";
            case 4: return "Gizli Bağlantılar";
            case 5: return "Son Kanıt";
            case 6: return "Derin Bir Komplo";
            case 7: return "Gerçeğin Yüzü";
            default: return "Bilinmeyen";
        }
    }

    private string GetChapterDescription(int chapterId)
    {
        string[] descriptions = {
            "Londra'da bir esnafın ölümü araştırılıyor. İlk ipuçlarını bul ve şüphelileri tanı.",
            "Cinayetler artıyor. Tanıklar sessiz kalıyor. Gerçeği ortaya çıkar.",
            "Adli tıp laboratuvarında yeni kanıtlar ortaya çıkıyor. Laboratuvar müdürü öldü.",
            "Cinayetler bir desen izliyor. Şüpheliler birbirlerine bağlı mı?",
            "Gizli belgeleri buldunuz. Kişiler kim? Motivleri nedir?",
            "Tüm kanıtlar bir noktaya işaret ediyor. Ama korkunç bir gerçek var.",
            "Son bölüm. Tüm bilgiler bir araya geliyor. Gerçek suçlu kim?"
        };
        
        return descriptions[chapterId - 1];
    }

    private List<SubMission> CreateSubMissions(int chapterId)
    {
        List<SubMission> missions = new List<SubMission>();

        // Her bölümde 5-8 alt görev
        int missionCount = 5 + chapterId; // Bölüm ilerledikçe daha fazla görev

        for (int i = 0; i < missionCount; i++)
        {
            missions.Add(new SubMission
            {
                missionId = i + 1,
                missionName = "Görev " + (i + 1),
                objective = GetMissionObjective(chapterId, i),
                isCompleted = false,
                rewardPoints = 50 + (chapterId * 10) + (i * 5),
                requiredActions = GetRequiredActions(chapterId, i)
            });
        }

        return missions;
    }

    private string GetMissionObjective(int chapterId, int missionIndex)
    {
        // Dinamik görev açıklamaları
        string[] actions = {
            "Tanıkları sorgula",
            "Kanıt topla",
            "Olay yerini incele",
            "Dosyaları analiz et",
            "Şüphelileri takip et",
            "Gizli bilgiyi ortaya çıkar",
            "Kanıtları bağla",
            "Son sorgulama yap"
        };

        return actions[missionIndex % actions.Length];
    }

    private List<string> GetRequiredActions(int chapterId, int missionIndex)
    {
        List<string> actions = new List<string>();

        // Her görev için gerekli aksiyonlar
        actions.Add("En az 3 tanık sorgula");
        actions.Add("En az 5 kanıt topla");
        
        if (chapterId > 3)
            actions.Add("Gizli bağlantı bul");
        
        if (chapterId > 5)
            actions.Add("DNA analizi tamamla");

        return actions;
    }

    private RewardSystem CreateRewards(int chapterId)
    {
        return new RewardSystem
        {
            experiencePoints = 1000 + (chapterId * 500),
            unlockedAbilities = chapterId,
            unlockedTools = new List<string>
            {
                "Adli Tıp Aracı Seviye " + chapterId,
                "Sorgulama Yeteneği " + chapterId,
                "Analiz Modu " + chapterId
            },
            nextChapterUnlock = (chapterId < 7) ? "Bölüm " + (chapterId + 1) : "DLC İçeriği",
            achievements = CreateAchievements(chapterId)
        };
    }

    private List<Achievement> CreateAchievements(int chapterId)
    {
        List<Achievement> achievements = new List<Achievement>();

        achievements.Add(new Achievement
        {
            achievementId = chapterId * 100 + 1,
            name = "Dedektif Başlangıcı",
            description = "Bölüm " + chapterId + " tamamla",
            rewardPoints = 100
        });

        achievements.Add(new Achievement
        {
            achievementId = chapterId * 100 + 2,
            name = "Kusursuz Araştırma",
            description = "Tüm alt görevleri tamamla",
            rewardPoints = 200
        });

        achievements.Add(new Achievement
        {
            achievementId = chapterId * 100 + 3,
            name = "Hızlı Çözüm",
            description = "Bölümü 1 saatten az sürede çöz",
            rewardPoints = 150
        });

        return achievements;
    }

    public void UnlockNextChapter()
    {
        int nextChapterId = currentChapter.chapterId + 1;
        if (nextChapterId <= chapters.Count)
        {
            chapters[nextChapterId - 1].isUnlocked = true;
            Debug.Log("✓ Bölüm " + nextChapterId + " açıldı!");
            
            // Oyuncu bağımlılığını artır (Next Chapter Hype)
            engagementMultiplier += 0.1f;
        }
    }

    public void CompleteChapter(bool isCorrect)
    {
        if (currentChapter == null) return;

        currentChapter.isCompleted = true;

        int baseScore = 1000 + (currentChapter.chapterId * 500);
        int bonusScore = isCorrect ? 500 : -200;
        int totalScore = baseScore + bonusScore;

        playerProgress.currentLevel += 1;
        playerProgress.totalExperience += totalScore;

        Debug.Log("✓ Bölüm " + currentChapter.chapterId + " tamamlandı!");
        Debug.Log("Kazanılan Puan: " + totalScore);

        // Bağımlılık mekanizmi: Teasers göster
        if (currentChapter.chapterId < 7)
        {
            ShowNextChapterTeaser();
        }

        // Ilerlemişse başarı açarken açılışı göster
        UnlockNextChapter();
    }

    private void ShowNextChapterTeaser()
    {
        int nextId = currentChapter.chapterId + 1;
        Chapter nextChapter = chapters[nextId - 1];

        Debug.Log("\n=== SONRAKI BÖLÜME HAZIRLANIN ===");
        Debug.Log("BÖLÜM " + nextId + ": " + nextChapter.chapterName);
        Debug.Log(nextChapter.chapterDescription);
        Debug.Log("\nTahmini Oyun Süresi: " + nextChapter.estimatedPlayTime + " dakika");
        Debug.Log("Zorluk Seviyesi: " + nextChapter.difficultyLevel + "/7");
        Debug.Log("=====================================\n");

        // UI'da teaser göster (Canvas animasyonu ile)
    }

    public void LoadChapter(int chapterId)
    {
        if (chapterId > 0 && chapterId <= chapters.Count)
        {
            Chapter chapter = chapters[chapterId - 1];

            if (!chapter.isUnlocked && chapterId != 1)
            {
                Debug.LogWarning("Bu bölüm henüz açılmamış!");
                return;
            }

            currentChapter = chapter;
            playerProgress.currentChapter = chapterId;

            Debug.Log("\n========== BÖLÜM " + chapterId + " BAŞLADI ==========");
            Debug.Log("Ad: " + chapter.chapterName);
            Debug.Log("Açıklama: " + chapter.chapterDescription);
            Debug.Log("Zorluk: " + chapter.difficultyLevel + "/7");
            Debug.Log("Beklenen Süre: " + chapter.estimatedPlayTime + " dakika");
            Debug.Log("==============================================\n");
        }
    }

    public void SavePlayerProgress()
    {
        string json = JsonUtility.ToJson(playerProgress, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerProgress.json", json);
        Debug.Log("✓ Oyuncu ilerlemesi kaydedildi.");
    }

    public void LoadPlayerProgress()
    {
        string path = Application.persistentDataPath + "/PlayerProgress.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            playerProgress = JsonUtility.FromJson<PlayerProgress>(json);
            Debug.Log("✓ Oyuncu ilerlemesi yüklendi.");
        }
        else
        {
            playerProgress = new PlayerProgress
            {
                currentChapter = 1,
                totalExperience = 0,
                currentLevel = 1,
                chapterProgresses = new Dictionary<int, ChapterProgress>(),
                unlockedAchievements = new List<Achievement>(),
                unlockedTools = new Dictionary<string, bool>()
            };
            SavePlayerProgress();
        }
    }

    public Chapter GetCurrentChapter() => currentChapter;
    public List<Chapter> GetAllChapters() => chapters;
    public PlayerProgress GetPlayerProgress() => playerProgress;
    public float GetEngagementMultiplier() => engagementMultiplier;
}
