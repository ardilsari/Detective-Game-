using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DecisionBranch
{
    public int decisionId;
    public string decisionText;
    public string consequence;
    public List<string> unlockedContent; // Açılan sahne/karakter/bilgi
    public int scoreImpact; // Puan değişimi
    public bool isIrreversible; // Geri dönüş mümkün mü?
    public List<int> nextDecisions; // Bağlı sonraki kararlar
}

[System.Serializable]
public class StoryPath
{
    public int pathId;
    public string pathName;
    public string pathDescription;
    public List<DecisionBranch> decisions;
    public bool isUnlocked;
    public bool isCompleted;
    public int pathScore;
    public List<string> unlockedCharacters;
    public List<string> unlockedScenes;
}

[System.Serializable]
public class SecretContent
{
    public int secretId;
    public string name;
    public string description;
    public List<int> requiredDecisions; // Bu kararları verirsen açılır
    public int difficulty; // Ne kadar gizli?
    public bool isUnlocked;
    public string contentType; // Scene, Character, Document, Ending
}

[System.Serializable]
public class MultipleEnding
{
    public int endingId;
    public string endingName;
    public string endingDescription;
    public List<int> decisionPath; // Hangi kararlar bu sonuca ulaştı
    public string cinematicScene;
    public int endingScore;
    public bool isUnlocked;
}

public class StoryBranchingSystem : MonoBehaviour
{
    public static StoryBranchingSystem instance;

    private List<StoryPath> storyPaths = new List<StoryPath>();
    private List<SecretContent> secretContents = new List<SecretContent>();
    private List<MultipleEnding> endings = new List<MultipleEnding>();
    private List<int> playerDecisions = new List<int>(); // Oyuncunun verdiği tüm kararlar

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
        InitializeStoryPaths();
        InitializeSecretContent();
        InitializeEndings();
    }

    private void InitializeStoryPaths()
    {
        // BÖLÜM 1: Jack the Ripper - İki Ana Yol
        StoryPath path1 = new StoryPath
        {
            pathId = 101,
            pathName = "İçerideki Çıkmazı Seç",
            pathDescription = "Doktor Druitt teorisini takip et. Tıbbi bilgi ve Whitechapel bağlantıları.",
            isUnlocked = true,
            decisions = new List<DecisionBranch>
            {
                new DecisionBranch
                {
                    decisionId = 101,
                    decisionText = "Doktor Druitt'in tıbbi geçmişini araştır",
                    consequence = "Tıbbi kütüphanelere gideceksin. Kanıtlar bulacaksın ama zaman harcayacaksın.",
                    unlockedContent = new List<string> { "Medical Records", "Whitechapel History" },
                    scoreImpact = 50,
                    isIrreversible = false
                },
                new DecisionBranch
                {
                    decisionId = 102,
                    decisionText = "Druitt'i suçlu olarak suçla",
                    consequence = "Tarih seni yanlış yapmış. Druitt beraata geldi. Zorluk yükselişi.",
                    unlockedContent = new List<string> { "Alternative Endings" },
                    scoreImpact = -100,
                    isIrreversible = true
                },
                new DecisionBranch
                {
                    decisionId = 103,
                    decisionText = "Kosminski teorisini incele",
                    consequence = "Kosminski'nin sınırlandırılması ve davranış sorunları açığa çıkıyor.",
                    unlockedContent = new List<string> { "Asylum Records", "Police Misconduct" },
                    scoreImpact = 75,
                    isIrreversible = false,
                    nextDecisions = new List<int> { 104, 105 }
                },
                new DecisionBranch
                {
                    decisionId = 104,
                    decisionText = "Polis yolsuzluğunu rapor et",
                    consequence = "Polise karşı kanıt getiriyorsun. Siyasi baskı başlıyor.",
                    unlockedContent = new List<string> { "Police Records Unsealed" },
                    scoreImpact = 100,
                    isIrreversible = true
                }
            }
        };

        storyPaths.Add(path1);

        // BÖLÜM 2: Lizzie Borden - Aile Gerilimi
        StoryPath path2 = new StoryPath
        {
            pathId = 102,
            pathName = "Aile İçi Cinayet",
            pathDescription = "Lizzie Borden ve ailesinin dinamiğini çöz. Miras motive ediyor mu?",
            isUnlocked = false,
            decisions = new List<DecisionBranch>
            {
                new DecisionBranch
                {
                    decisionId = 201,
                    decisionText = "Lizzie'yi suçlu olarak hedef al",
                    consequence = "Tarih seni haklı çıkartıyor! Halk seni doğru yolda gördü.",
                    unlockedContent = new List<string> { "Lizzie Private Letters", "Secret Diary" },
                    scoreImpact = 200,
                    isIrreversible = true
                },
                new DecisionBranch
                {
                    decisionId = 202,
                    decisionText = "Hizmetçi Bridget'i incele",
                    consequence = "Bridget'in alibi tutarsız. Ama cinsiyetçi polis soruşturması var.",
                    unlockedContent = new List<string> { "Servant Interview Transcripts" },
                    scoreImpact = 80,
                    isIrreversible = false
                }
            }
        };

        storyPaths.Add(path2);

        Debug.Log("✓ " + storyPaths.Count + " Ana Hikaye Yolu Yüklendi");
    }

    private void InitializeSecretContent()
    {
        // GİZLİ İÇERİK - Oyuncuları Tekrar Oynamaya Teşvik Eder
        secretContents.Add(new SecretContent
        {
            secretId = 1001,
            name = "Jack the Ripper'ın Gerçek Kimliği",
            description = "Modern DNA teknolojisi ile yapılan analiz. Gerçek suçlu kim?",
            requiredDecisions = new List<int> { 103, 104 }, // Kosminski + Polis yolsuzluğu
            difficulty = 9, // Çok gizli
            contentType = "Document",
            isUnlocked = false
        });

        secretContents.Add(new SecretContent
        {
            secretId = 1002,
            name = "Tarihçilerin Kaçırdığı Kanıt",
            description = "Arsiv dosyalarında bulunan gizli mektup.",
            requiredDecisions = new List<int> { 104 }, // Polis yolsuzluğu
            difficulty = 8,
            contentType = "Document",
            isUnlocked = false
        });

        secretContents.Add(new SecretContent
        {
            secretId = 2001,
            name = "Lizzie Borden'ın Gizli Günlüğü",
            description = "Lizzie'nin özel yazıları ve itirafları.",
            requiredDecisions = new List<int> { 201 }, // Lizzie suçlu seç
            difficulty = 7,
            contentType = "Document",
            isUnlocked = false
        });

        secretContents.Add(new SecretContent
        {
            secretId = 3001,
            name = "Black Dahlia - Hollywood Bağlantısı",
            description = "Elizabeth Short'un Hollywood ünlüleri ile gizli ilişkileri.",
            requiredDecisions = new List<int> { 301, 302, 303 }, // Tüm Hollywood kararları
            difficulty = 9,
            contentType = "Scene",
            isUnlocked = false
        });

        secretContents.Add(new SecretContent
        {
            secretId = 7001,
            name = "7. Bölüm - Seri Katil Ağı",
            description = "Tüm cinayetlerin bir ağda bağlandığı kanıtlar.",
            requiredDecisions = new List<int> { 104, 201, 302, 401, 501, 601 }, // Her bölümden kritik karar
            difficulty = 10, // Maksimum gizli
            contentType = "Document",
            isUnlocked = false
        });

        Debug.Log("✓ " + secretContents.Count + " Gizli İçerik Yüklendi");
    }

    private void InitializeEndings()
    {
        // BÖLÜM 1 SONLARı
        endings.Add(new MultipleEnding
        {
            endingId = 1001,
            endingName = "Druitt'i Cezalandırmak",
            endingDescription = "Doktor Druitt suçludur sonucu. Ama yapılan hata mı?",
            decisionPath = new List<int> { 101, 102 },
            cinematicScene = "Chapter1_Ending_Druitt",
            endingScore = 750,
            isUnlocked = false
        });

        endings.Add(new MultipleEnding
        {
            endingId = 1002,
            endingName = "Kosminski Teorisi",
            endingDescription = "Aaron Kosminski Jack the Ripper. Polis neden sakladı?",
            decisionPath = new List<int> { 103, 104 },
            cinematicScene = "Chapter1_Ending_Kosminski",
            endingScore = 950,
            isUnlocked = false
        });

        endings.Add(new MultipleEnding
        {
            endingId = 1003,
            endingName = "Gizem Çözülmedi",
            endingDescription = "Jack the Ripper sonsuza kadar gizem kalacak.",
            decisionPath = new List<int> { },
            cinematicScene = "Chapter1_Ending_Mystery",
            endingScore = 500,
            isUnlocked = false
        });

        // BÖLÜM 2 SONLARı
        endings.Add(new MultipleEnding
        {
            endingId = 2001,
            endingName = "Lizzie Suçlu",
            endingDescription = "Lizzie Borden ebeveynlerini öldürdü. Tarih seni haklı çıkartıyor!",
            decisionPath = new List<int> { 201 },
            cinematicScene = "Chapter2_Ending_Lizzie_Guilty",
            endingScore = 1000,
            isUnlocked = false
        });

        endings.Add(new MultipleEnding
        {
            endingId = 2002,
            endingName = "Bridget Suçlu",
            endingDescription = "Hizmetçi Bridget Sullivan sahibelerini öldürdü.",
            decisionPath = new List<int> { 202 },
            cinematicScene = "Chapter2_Ending_Bridget_Guilty",
            endingScore = 650,
            isUnlocked = false
        });

        // BÖLÜM 7 - FINAL ENDING
        endings.Add(new MultipleEnding
        {
            endingId = 7001,
            endingName = "Tarihçi Olarak İz Bırak",
            endingDescription = "Tüm cinayetleri çözdün. Kitab yaz ve tarihçi ol.",
            decisionPath = new List<int> { 104, 201, 302, 401, 501, 601, 701 },
            cinematicScene = "Chapter7_Ending_Published_Historian",
            endingScore = 5000,
            isUnlocked = false
        });

        endings.Add(new MultipleEnding
        {
            endingId = 7002,
            endingName = "Gizli Tutmak",
            endingDescription = "Gerçekleri saklı tutarsın. Yaşam normal devam ediyor.",
            decisionPath = new List<int> { },
            cinematicScene = "Chapter7_Ending_Silent_Keeper",
            endingScore = 2000,
            isUnlocked = false
        });

        Debug.Log("✓ " + endings.Count + " Farklı Ending Yüklendi");
    }

    public void MakeDecision(int decisionId)
    {
        DecisionBranch decision = FindDecisionById(decisionId);
        if (decision == null) return;

        // Karar kaydı
        playerDecisions.Add(decisionId);

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("📍 KARAR VERİLDİ!");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log(decision.decisionText);
        Debug.Log("\n▸ Sonuç: " + decision.consequence);
        Debug.Log("▸ Puan Değişimi: " + decision.scoreImpact);
        Debug.Log("▸ Geri Dönüş: " + (decision.isIrreversible ? "HAYIR - Kalıcı!" : "Evet"));

        // Kilit açılmış içerikleri göster
        if (decision.unlockedContent.Count > 0)
        {
            Debug.Log("\n✓ Açılmış İçerikler:");
            foreach (string content in decision.unlockedContent)
            {
                Debug.Log("  • " + content);
            }
        }

        Debug.Log("═══════════════════════════════════════\n");

        // Puan güncelle
        GameManager gameManager = GameManager.instance;
        if (gameManager != null)
        {
            gameManager.AddScore(decision.scoreImpact);
        }

        // Gizli içeriği kontrol et
        CheckSecretContent();

        // Sonraki kararları aç
        if (decision.nextDecisions.Count > 0)
        {
            Debug.Log("Yeni Kararlar Mevcut!");
        }

        // Canvas'ta göster
        ShowDecisionAnimation(decision);
    }

    private void CheckSecretContent()
    {
        foreach (SecretContent secret in secretContents)
        {
            if (!secret.isUnlocked && AllDecisionsMade(secret.requiredDecisions))
            {
                secret.isUnlocked = true;
                
                Debug.Log("\n🔓 GİZLİ İÇERİK AÇILDI!");
                Debug.Log("Ad: " + secret.name);
                Debug.Log("Açıklama: " + secret.description);
                Debug.Log("Tür: " + secret.contentType);
                Debug.Log("Zorluk: " + secret.difficulty + "/10\n");

                ShowSecretContentPopup(secret);
            }
        }
    }

    private bool AllDecisionsMade(List<int> requiredDecisions)
    {
        foreach (int reqDecision in requiredDecisions)
        {
            if (!playerDecisions.Contains(reqDecision))
                return false;
        }
        return true;
    }

    private DecisionBranch FindDecisionById(int decisionId)
    {
        foreach (StoryPath path in storyPaths)
        {
            foreach (DecisionBranch decision in path.decisions)
            {
                if (decision.decisionId == decisionId)
                    return decision;
            }
        }
        return null;
    }

    private void ShowDecisionAnimation(DecisionBranch decision)
    {
        // TODO: Canvas üzerinde karar seçiminin sonucunu göster
        // - Arka plan rengi değişimi
        // - Sonuç metni fade in
        // - Ses efekti
        // - 3-4 saniye bekle
    }

    private void ShowSecretContentPopup(SecretContent secret)
    {
        // TODO: Gizli içerik popup'ı göster
        // - Lock iconunun açılması animasyonu
        // - Parlama efekti
        // - Özel ses efekti
    }

    public List<int> GetPlayerDecisions() => playerDecisions;
    public List<SecretContent> GetUnlockedSecrets()
    {
        List<SecretContent> unlockedSecrets = new List<SecretContent>();
        foreach (SecretContent secret in secretContents)
        {
            if (secret.isUnlocked)
                unlockedSecrets.Add(secret);
        }
        return unlockedSecrets;
    }

    public MultipleEnding GetEndingForPath(List<int> decisionPath)
    {
        foreach (MultipleEnding ending in endings)
        {
            if (ending.decisionPath.Count == decisionPath.Count)
            {
                bool match = true;
                foreach (int decision in ending.decisionPath)
                {
                    if (!decisionPath.Contains(decision))
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return ending;
            }
        }
        return endings[0]; // Default ending
    }

    public void SaveDecisions()
    {
        string json = JsonUtility.ToJson(new DecisionDataWrapper { decisions = playerDecisions }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerDecisions.json", json);
    }

    [System.Serializable]
    private class DecisionDataWrapper
    {
        public List<int> decisions = new List<int>();
    }
}
