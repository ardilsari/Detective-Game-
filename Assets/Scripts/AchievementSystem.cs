using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AchievementSystem : MonoBehaviour
{
    public static AchievementSystem instance;

    [System.Serializable]
    public class Achievement
    {
        public int achievementId;
        public string name;
        public string description;
        public string icon; // Sprite referansı
        public int rewardPoints;
        public bool isUnlocked;
        public float unlockedTime;
        public AchievementRarity rarity; // Common, Rare, Legendary
    }

    [System.Serializable]
    public class ChallengeMode
    {
        public int challengeId;
        public string challengeName;
        public string objective;
        public int timeLimit; // dakika
        public int scoreBudget;
        public bool isCompleted;
        public int bestScore;
    }

    public enum AchievementRarity { Common, Rare, Epic, Legendary }

    private List<Achievement> achievements = new List<Achievement>();
    private List<ChallengeMode> challenges = new List<ChallengeMode>();
    private int totalAchievementPoints = 0;

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
        InitializeAchievements();
        InitializeChallenges();
    }

    private void InitializeAchievements()
    {
        // CHAPTER COMPLETION ACHIEVEMENTS
        for (int i = 1; i <= 7; i++)
        {
            achievements.Add(new Achievement
            {
                achievementId = i,
                name = "Bölüm " + i + " Çözüldü",
                description = "Bölüm " + i + " olayını başarıyla çöz",
                rarity = (AchievementRarity)((i - 1) / 2), // Zorluk arttıkça rarity artıyor
                rewardPoints = 100 + (i * 50),
                isUnlocked = false
            });
        }

        // DETECTIVE RANK ACHIEVEMENTS
        achievements.Add(new Achievement
        {
            achievementId = 101,
            name = "Başlayan Dedektif",
            description = "Oyunu başlat",
            rarity = AchievementRarity.Common,
            rewardPoints = 10,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 102,
            name = "Seçkin Dedektif",
            description = "3 Bölümü tamamla",
            rarity = AchievementRarity.Rare,
            rewardPoints = 150,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 103,
            name = "Master Dedektif",
            description = "Tüm 7 Bölümü tamamla",
            rarity = AchievementRarity.Legendary,
            rewardPoints = 500,
            isUnlocked = false
        });

        // INVESTIGATION ACHIEVEMENTS
        achievements.Add(new Achievement
        {
            achievementId = 201,
            name = "İpucu Avcısı",
            description = "50 ipucu topla",
            rarity = AchievementRarity.Common,
            rewardPoints = 50,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 202,
            name = "Kanıt Uzmanı",
            description = "100 kanıt topla",
            rarity = AchievementRarity.Rare,
            rewardPoints = 100,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 203,
            name = "Tanık Taraftar",
            description = "50 tanığı sorgula",
            rarity = AchievementRarity.Common,
            rewardPoints = 75,
            isUnlocked = false
        });

        // SPEED RUN ACHIEVEMENTS
        achievements.Add(new Achievement
        {
            achievementId = 301,
            name = "Hızlı Çözüm",
            description = "Herhangi bir bölümü 1 saatten az sürede çöz",
            rarity = AchievementRarity.Rare,
            rewardPoints = 200,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 302,
            name = "Şimşek Dedektif",
            description = "Herhangi bir bölümü 45 dakikadan az sürede çöz",
            rarity = AchievementRarity.Epic,
            rewardPoints = 300,
            isUnlocked = false
        });

        // PERFECT PLAY ACHIEVEMENTS
        achievements.Add(new Achievement
        {
            achievementId = 401,
            name = "Kusursuz Araştırma",
            description = "Herhangi bir bölümdeki tüm alt görevleri tamamla",
            rarity = AchievementRarity.Epic,
            rewardPoints = 250,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 402,
            name = "Adalet Yeminli",
            description = "Tüm 7 bölümde yanlış sonuç elde etmeden tamamla",
            rarity = AchievementRarity.Legendary,
            rewardPoints = 700,
            isUnlocked = false
        });

        // SECRET ACHIEVEMENTS
        achievements.Add(new Achievement
        {
            achievementId = 501,
            name = "Gizli Sahneler",
            description = "Gizli sahneleri açmak için gizli kararları ver",
            rarity = AchievementRarity.Epic,
            rewardPoints = 150,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 502,
            name = "Tarihçi",
            description = "Gerçek tarihçilerin yapamadığını yap - tüm cinayetleri çöz",
            rarity = AchievementRarity.Legendary,
            rewardPoints = 1000,
            isUnlocked = false
        });

        // REPLAY ACHIEVEMENTS
        achievements.Add(new Achievement
        {
            achievementId = 601,
            name = "Tekrar Oyuncu",
            description = "Herhangi bir bölümü 2 kez oyna",
            rarity = AchievementRarity.Common,
            rewardPoints = 50,
            isUnlocked = false
        });

        achievements.Add(new Achievement
        {
            achievementId = 602,
            name = "Takıntılı Dedektif",
            description = "Herhangi bir bölümü 5 kez oyna",
            rarity = AchievementRarity.Rare,
            rewardPoints = 200,
            isUnlocked = false
        });

        Debug.Log("✓ " + achievements.Count + " Başarı Yüklendi");
    }

    private void InitializeChallenges()
    {
        // CHALLENGE MODES - Replay değeri artırmak için
        challenges.Add(new ChallengeMode
        {
            challengeId = 1,
            challengeName = "Çelik Alın Zorluk",
            objective = "Herhangi bir bölümü yalnızca 5 tanık sorgulama hakkı ile çöz",
            timeLimit = 120,
            scoreBudget = 500,
            isCompleted = false
        });

        challenges.Add(new ChallengeMode
        {
            challengeId = 2,
            challengeName = "Hızlı Zeka",
            objective = "60 dakika içinde herhangi bir bölümü çöz",
            timeLimit = 60,
            scoreBudget = 1000,
            isCompleted = false
        });

        challenges.Add(new ChallengeMode
        {
            challengeId = 3,
            challengeName = "Körü Körüne Güven",
            objective = "İpuçları görmeden (blind mode) bölüm çöz",
            timeLimit = 240,
            scoreBudget = 300,
            isCompleted = false
        });

        challenges.Add(new ChallengeMode
        {
            challengeId = 4,
            challengeName = "Hiç Hata Yok",
            objective = "Herhangi bir bölümde hiç yanlış ipucu kullanmadan çöz",
            timeLimit = 120,
            scoreBudget = 2000,
            isCompleted = false
        });

        challenges.Add(new ChallengeMode
        {
            challengeId = 5,
            challengeName = "Tarihçi Meydan Okuması",
            objective = "Tüm 7 bölümü ardışık 1 oturmada çöz (14 saat!)",
            timeLimit = 840, // 14 saat
            scoreBudget = 5000,
            isCompleted = false
        });

        Debug.Log("✓ " + challenges.Count + " Challenge Modu Yüklendi");
    }

    public void UnlockAchievement(int achievementId)
    {
        foreach (Achievement ach in achievements)
        {
            if (ach.achievementId == achievementId && !ach.isUnlocked)
            {
                ach.isUnlocked = true;
                ach.unlockedTime = Time.time;
                totalAchievementPoints += ach.rewardPoints;

                Debug.Log("\n🏆 BAŞARI AÇILDI!");
                Debug.Log("Ad: " + ach.name);
                Debug.Log("Açıklama: " + ach.description);
                Debug.Log("Rarity: " + ach.rarity);
                Debug.Log("+ " + ach.rewardPoints + " Puan");
                Debug.Log("Toplam Başarı Puanı: " + totalAchievementPoints + "\n");

                ShowAchievementPopup(ach);
                SaveAchievements();
                return;
            }
        }
    }

    public void CompleteChallenge(int challengeId, int score)
    {
        foreach (ChallengeMode challenge in challenges)
        {
            if (challenge.challengeId == challengeId && !challenge.isCompleted)
            {
                if (score >= challenge.scoreBudget)
                {
                    challenge.isCompleted = true;
                    challenge.bestScore = score;

                    Debug.Log("\n⭐ CHALLENGE TAMAMLANDI!");
                    Debug.Log("Ad: " + challenge.challengeName);
                    Debug.Log("Hedef: " + challenge.objective);
                    Debug.Log("Skor: " + score + "/" + challenge.scoreBudget);
                    Debug.Log("Yeni başarılar açıldı!\n");

                    SaveChallenges();
                    return;
                }
                else
                {
                    Debug.Log("❌ Challenge başarısız. Gerekli Skor: " + challenge.scoreBudget + ", Aldığın: " + score);
                }
            }
        }
    }

    private void ShowAchievementPopup(Achievement achievement)
    {
        // TODO: Canvas üzerinde popup animasyonu göster
        // - Achievement icon
        // - Name ve description
        // - Reward points
        // - Fade in/out animation (3 saniye)
        // - Ses efekti
    }

    public int GetTotalAchievementPoints() => totalAchievementPoints;

    public int GetUnlockedAchievementCount()
    {
        int count = 0;
        foreach (Achievement ach in achievements)
        {
            if (ach.isUnlocked) count++;
        }
        return count;
    }

    public float GetCompletionPercentage()
    {
        return (GetUnlockedAchievementCount() / (float)achievements.Count) * 100f;
    }

    public List<Achievement> GetAchievements() => achievements;
    public List<ChallengeMode> GetChallenges() => challenges;

    public void SaveAchievements()
    {
        string json = JsonUtility.ToJson(new AchievementDataWrapper { achievements = achievements }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Achievements.json", json);
    }

    public void LoadAchievements()
    {
        string path = Application.persistentDataPath + "/Achievements.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            AchievementDataWrapper wrapper = JsonUtility.FromJson<AchievementDataWrapper>(json);
            achievements = wrapper.achievements;
        }
    }

    private void SaveChallenges()
    {
        string json = JsonUtility.ToJson(new ChallengeDataWrapper { challenges = challenges }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Challenges.json", json);
    }

    [System.Serializable]
    private class AchievementDataWrapper
    {
        public List<Achievement> achievements = new List<Achievement>();
    }

    [System.Serializable]
    private class ChallengeDataWrapper
    {
        public List<ChallengeMode> challenges = new List<ChallengeMode>();
    }
}
