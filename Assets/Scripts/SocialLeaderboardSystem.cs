using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProfile
{
    public string username;
    public string userId;
    public int totalScore;
    public int level;
    public int chaptersCompleted;
    public int achievementsUnlocked;
    public int playtimeHours;
    public string favoriteChapter;
    public string profileBio;
    public string avatarUrl;
    public bool isPublic; // Profil halkla paylaşılsın mı?
}

[System.Serializable]
public class LeaderboardEntry
{
    public int rank;
    public PlayerProfile player;
    public int score;
    public float completionTime; // saat cinsinden
    public int chaptersCompleted;
    public List<string> unlockedSecrets;
    public System.DateTime recordDate;
}

[System.Serializable]
public class GlobalChallenge
{
    public int challengeId;
    public string challengeName;
    public string description;
    public int targetScore;
    public int participantCount;
    public System.DateTime startDate;
    public System.DateTime endDate;
    public List<LeaderboardEntry> topPlayers;
}

[System.Serializable]
public class SocialFeature
{
    public int featureId;
    public string featureName;
    public string description;
    public bool isEnabled;
}

public class SocialLeaderboardSystem : MonoBehaviour
{
    public static SocialLeaderboardSystem instance;

    private PlayerProfile currentPlayer;
    private List<LeaderboardEntry> globalLeaderboard = new List<LeaderboardEntry>();
    private List<LeaderboardEntry> friendsLeaderboard = new List<LeaderboardEntry>();
    private List<GlobalChallenge> activeChallenges = new List<GlobalChallenge>();
    private List<PlayerProfile> friends = new List<PlayerProfile>();

    // Sosyal Ağ Entegrasyonu
    private bool steamConnected = false;
    private bool discordConnected = false;
    private bool twitterConnected = false;

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
        InitializePlayerProfile();
        InitializeLeaderboard();
        InitializeGlobalChallenges();
    }

    private void InitializePlayerProfile()
    {
        currentPlayer = new PlayerProfile
        {
            username = "Detective_" + Random.Range(1000, 9999),
            userId = System.Guid.NewGuid().ToString(),
            totalScore = 0,
            level = 1,
            chaptersCompleted = 0,
            achievementsUnlocked = 0,
            playtimeHours = 0,
            profileBio = "Gerçeği arayan dedektif",
            avatarUrl = "default_avatar",
            isPublic = true
        };

        Debug.Log("✓ Oyuncu Profili Oluşturuldu: " + currentPlayer.username);
    }

    private void InitializeLeaderboard()
    {
        // Global Leaderboard - Top 100 dedektif
        for (int i = 1; i <= 20; i++)
        {
            globalLeaderboard.Add(new LeaderboardEntry
            {
                rank = i,
                player = new PlayerProfile
                {
                    username = "Detective_" + (1000 + i),
                    userId = System.Guid.NewGuid().ToString(),
                    level = 7 - (i / 3),
                    profileBio = "Master Detective"
                },
                score = 50000 - (i * 1000),
                completionTime = 14.0f + (i * 0.5f),
                chaptersCompleted = 7,
                unlockedSecrets = new List<string> { "Secret1", "Secret2", "Secret3" },
                recordDate = System.DateTime.Now.AddDays(-Random.Range(1, 30))
            });
        }

        Debug.Log("✓ Global Leaderboard Yüklendi (Top 20)");
    }

    private void InitializeGlobalChallenges()
    {
        // HAFTALIK CHALLENGE
        activeChallenges.Add(new GlobalChallenge
        {
            challengeId = 1,
            challengeName = "Hızlı Çözüm - Haftalık",
            description = "Herhangi bir bölümü en hızlı şekilde çöz. En az sürede tamamlayanlar ödül kazanır!",
            targetScore = 5000,
            participantCount = 1247,
            startDate = System.DateTime.Now.AddDays(-7),
            endDate = System.DateTime.Now.AddDays(7),
            topPlayers = new List<LeaderboardEntry>()
        });

        // AYLIK CHALLENGE - Master Historian
        activeChallenges.Add(new GlobalChallenge
        {
            challengeId = 2,
            challengeName = "Master Tarihçi - Aylık",
            description = "Tüm 7 bölümü tamamla ve maksimum gizli içeriği aç. Ayın sonunda önemli ödüller kazanacak oyuncuları ilan edeceğiz!",
            targetScore = 50000,
            participantCount = 342,
            startDate = System.DateTime.Now.AddDays(-15),
            endDate = System.DateTime.Now.AddDays(15),
            topPlayers = new List<LeaderboardEntry>()
        });

        // ÖZEL CHALLENGE
        activeChallenges.Add(new GlobalChallenge
        {
            challengeId = 3,
            challengeName = "Blind Mode - Hiç İpucu Yok",
            description = "İpuçları görmeden bölüm çöz. Cesaret göster! En fazla puan kazananlar Steam'de özel rozetler alacak.",
            targetScore = 10000,
            participantCount = 89,
            startDate = System.DateTime.Now.AddDays(-30),
            endDate = System.DateTime.Now.AddDays(30),
            topPlayers = new List<LeaderboardEntry>()
        });

        Debug.Log("✓ " + activeChallenges.Count + " Global Challenge Aktif");
    }

    public void UpdatePlayerScore(int scoreGain)
    {
        currentPlayer.totalScore += scoreGain;
        currentPlayer.level = (currentPlayer.totalScore / 10000) + 1; // Her 10k puan = 1 level
        
        Debug.Log("📊 Puan Güncellendi: +" + scoreGain);
        Debug.Log("Toplam Puan: " + currentPlayer.totalScore);
        Debug.Log("Level: " + currentPlayer.level);

        // Leaderboard'u güncelle
        UpdateLeaderboardPosition();
    }

    public void CompleteChapter(int chapterId)
    {
        currentPlayer.chaptersCompleted++;
        
        if (currentPlayer.chaptersCompleted == 7)
        {
            Debug.Log("🏆 TÜMSÜ BÖLÜMLER TAMAMLANDI!");
            UnlockSpecialAchievement("Master_Historian");
        }

        SavePlayerProgress();
    }

    private void UpdateLeaderboardPosition()
    {
        // Oyuncunun sırasını hesapla
        int rank = 1;
        foreach (LeaderboardEntry entry in globalLeaderboard)
        {
            if (currentPlayer.totalScore > entry.score)
                break;
            rank++;
        }

        // Eğer Top 20'ye girerse
        if (rank <= 20)
        {
            Debug.Log("⭐ LEADERBOARD'A GİRDİN!");
            Debug.Log("Sıran: #" + rank);
            ShowLeaderboardNotification(rank);
        }
    }

    // SOSYAL MEDYA ENTEGRASYONU
    public void ConnectSteam()
    {
        // TODO: Steam API entegrasyonu
        // Steamworks SDK ile bağlantı
        steamConnected = true;
        Debug.Log("✓ Steam bağlantısı kuruldu");
    }

    public void ConnectDiscord()
    {
        // TODO: Discord Rich Presence
        // Oyuncunun hangi bölümü oynadığını Discord'da göster
        discordConnected = true;
        Debug.Log("✓ Discord bağlantısı kuruldu");
    }

    public void ShareToTwitter(string message)
    {
        // TODO: Twitter API entegrasyonu
        string tweetText = message + " #DetectiveGame #MysteryUnraveled";
        string twitterUrl = "https://twitter.com/intent/tweet?text=" + System.Uri.EscapeDataString(tweetText);
        
        Debug.Log("🐦 Tweet Gönderiliyor: " + tweetText);
        // Application.OpenURL(twitterUrl);
    }

    public void ShareScreenshot()
    {
        // TODO: Oyun içi screenshot alma ve sosyal medyada paylaşma
        Debug.Log("📸 Screenshot Kaydedildi!");
        Debug.Log("Paylaş: Discord / Twitter / Steam");
    }

    public void AddFriend(string userId)
    {
        // TODO: Arkadaş ekleme sistemi
        Debug.Log("✓ Arkadaş eklendi: " + userId);
        // friends.Add(newFriend);
    }

    public void ViewFriendsLeaderboard()
    {
        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("👥 ARKADAŞ LEADERBOARD'U");
        Debug.Log("═══════════════════════════════════════");
        
        int rank = 1;
        foreach (LeaderboardEntry entry in friendsLeaderboard)
        {
            Debug.Log("#" + rank + " | " + entry.player.username + " | Puan: " + entry.score);
            rank++;
        }
        Debug.Log("═══════════════════════════════════════\n");
    }

    public void ViewGlobalLeaderboard()
    {
        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🏆 GLOBAL LEADERBOARD - TOP 20");
        Debug.Log("═══════════════════════════════════════");
        
        foreach (LeaderboardEntry entry in globalLeaderboard)
        {
            Debug.Log("#" + entry.rank + " | " + entry.player.username + " | Level: " + entry.player.level + " | Puan: " + entry.score);
        }
        Debug.Log("═══════════════════════════════════════\n");
    }

    public void ViewActiveChallenges()
    {
        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🎯 ACTIVE GLOBAL CHALLENGES");
        Debug.Log("═══════════════════════════════════════");
        
        foreach (GlobalChallenge challenge in activeChallenges)
        {
            Debug.Log("\n" + challenge.challengeName);
            Debug.Log("Açıklama: " + challenge.description);
            Debug.Log("Katılımcı: " + challenge.participantCount);
            Debug.Log("Bitiş: " + challenge.endDate.ToString("dd/MM/yyyy"));
        }
        Debug.Log("═══════════════════════════════════════\n");
    }

    private void ShowLeaderboardNotification(int rank)
    {
        // TODO: Canvas üzerinde leaderboard notifikasyonu göster
        Debug.Log("\n🎉 BAŞARI AÇILDI!");
        Debug.Log("Leaderboard'a girdiyseniz! #" + rank);
    }

    private void UnlockSpecialAchievement(string achievementKey)
    {
        // TODO: Özel başarıyı aç ve bildirim gönder
        Debug.Log("🏆 Özel Başarı Açıldı: " + achievementKey);
    }

    public void SavePlayerProgress()
    {
        string json = JsonUtility.ToJson(currentPlayer, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerProfile.json", json);
        Debug.Log("✓ Oyuncu profili kaydedildi");
    }

    public PlayerProfile GetCurrentPlayer() => currentPlayer;
    public List<LeaderboardEntry> GetGlobalLeaderboard() => globalLeaderboard;
    public List<GlobalChallenge> GetActiveChallenges() => activeChallenges;
    public bool IsSteamConnected() => steamConnected;
    public bool IsDiscordConnected() => discordConnected;
}
