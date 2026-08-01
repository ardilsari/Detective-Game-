using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectiveTask
{
    public int taskId;
    public string taskTitle;
    public string taskDescription;
    public string taskType; // Investigation, Interrogation, Evidence, Decision, Finale
    public int chapterId;
    public bool isCompleted;
    public bool isOptional;
    public int rewardPoints;
    public string linkedCharacterId; // Kim ile ilgili?
    public List<int> linkedEvidenceIds;
    public string location; // Nerede yapılacak?
    public bool hasHint;
    public string hint;
    public int difficulty; // 1-10
    public float completionProgress; // 0-1
}

[System.Serializable]
public class QuestLine
{
    public int questLineId;
    public string questName;
    public string questDescription;
    public int chapterId;
    public List<int> objectives; // ObjectiveTask ID'leri
    public bool isMainQuest;
    public bool isActive;
    public bool isCompleted;
    public int rewardPoints;
    public string rewardAchievement;
    public float completionPercentage;
    public int currentObjectiveIndex;
}

[System.Serializable]
public class ObjectiveMarker
{
    public int markerId;
    public string markerName;
    public Vector3 markerPosition;
    public string markerType; // Location, Character, Evidence
    public int linkedObjectiveId;
    public bool isDiscovered;
    public bool isPinned;
}

[System.Serializable]
public class QuestJournal
{
    public int journalEntryId;
    public string entryTitle;
    public string entryContent;
    public int chapterId;
    public System.DateTime entryDate;
    public List<int> linkedObjectives;
    public List<int> linkedCharacters;
    public bool isImportant;
    public bool isSolved;
}

public class QuestObjectiveTracker : MonoBehaviour
{
    public static QuestObjectiveTracker instance;

    private List<ObjectiveTask> allObjectives = new List<ObjectiveTask>();
    private List<QuestLine> questLines = new List<QuestLine>();
    private List<ObjectiveMarker> markers = new List<ObjectiveMarker>();
    private List<QuestJournal> journal = new List<QuestJournal>();

    private QuestLine currentActiveQuest;
    private ObjectiveTask currentActiveObjective;
    private int totalQuestsCompleted = 0;
    private int totalObjectivesCompleted = 0;

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
        InitializeObjectives();
        InitializeQuestLines();
        Debug.Log("✓ Quest Objective Tracker Başlatıldı");
    }

    private void InitializeObjectives()
    {
        // BÖLÜM 1: Jack the Ripper - Soruşturma Görevleri
        allObjectives.Add(new ObjectiveTask
        {
            taskId = 1001,
            taskTitle = "Whitechapel'i İnceले",
            taskDescription = "Cinayetlerin gerçekleştiği Whitechapel bölgesini keşfet",
            taskType = "Investigation",
            chapterId = 1,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 500,
            linkedCharacterId = "",
            linkedEvidenceIds = new List<int> { 1001, 1002 },
            location = "Whitechapel_Street",
            hasHint = true,
            hint = "Bölgenin arka sokaklarında gizli ipuçları var",
            difficulty = 3,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 1002,
            taskTitle = "Dr. Druitt'i Sorgula",
            taskDescription = "Dr. Druitt'i polis istasyonuna çağırarak sorgula",
            taskType = "Interrogation",
            chapterId = 1,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 750,
            linkedCharacterId = "101",
            linkedEvidenceIds = new List<int> { 1003, 1004, 1005 },
            location = "Police_Station_1888",
            hasHint = true,
            hint = "Tıbbi arka planı hakkında sorular sor",
            difficulty = 6,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 1003,
            taskTitle = "Aaron Kosminski'yi Bul",
            taskDescription = "Aaron Kosminski'nin nerede olduğunu keşfet ve sorgulamaya hazırla",
            taskType = "Investigation",
            chapterId = 1,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 600,
            linkedCharacterId = "102",
            linkedEvidenceIds = new List<int> { 1006, 1007 },
            location = "Asylum",
            hasHint = true,
            hint = "Ruh sağlığı kurumu araştır",
            difficulty = 7,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 1004,
            taskTitle = "Kurban Otopsi Raporları",
            taskDescription = "Tüm beş kurbanın otopsi raporlarını topla ve analiz et",
            taskType = "Evidence",
            chapterId = 1,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 800,
            linkedCharacterId = "",
            linkedEvidenceIds = new List<int> { 1008, 1009, 1010, 1011, 1012 },
            location = "Morgue",
            hasHint = true,
            hint = "Doktor examiner'a konuş",
            difficulty = 5,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 1005,
            taskTitle = "Suçluyu Seç",
            taskDescription = "Bölüm 1'in sonunda suçluyu belirle. Yanlış seçim yapabilirsin!",
            taskType = "Decision",
            chapterId = 1,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 1000,
            linkedCharacterId = "",
            linkedEvidenceIds = new List<int>(),
            location = "Court_Room",
            hasHint = true,
            hint = "Tüm kanıtları dikkatlice incele",
            difficulty = 9,
            completionProgress = 0f
        });

        // BÖLÜM 2: Lizzie Borden - Soruşturma Görevleri
        allObjectives.Add(new ObjectiveTask
        {
            taskId = 2001,
            taskTitle = "Borden Evini İnceле",
            taskDescription = "Cinayetlerin gerçekleştiği Borden evini keşfet",
            taskType = "Investigation",
            chapterId = 2,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 500,
            linkedCharacterId = "",
            linkedEvidenceIds = new List<int> { 2001, 2002 },
            location = "Borden_House",
            hasHint = true,
            hint = "Evdeki kan izlerini dikkatle gözlemle",
            difficulty = 4,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 2002,
            taskTitle = "Lizzie'yi Sorgulamaya Çağır",
            taskDescription = "Lizzie Borden'ı resmi sorgulamaya çağır",
            taskType = "Interrogation",
            chapterId = 2,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 800,
            linkedCharacterId = "201",
            linkedEvidenceIds = new List<int> { 2003, 2004, 2005 },
            location = "Police_Station_1892",
            hasHint = true,
            hint = "Alibi çelişkilerini bulup sor",
            difficulty = 7,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 2003,
            taskTitle = "Hizmetçi Bridget'i Sorguला",
            taskDescription = "Hizmetçi Bridget'i sorgulamaya al",
            taskType = "Interrogation",
            chapterId = 2,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 650,
            linkedCharacterId = "202",
            linkedEvidenceIds = new List<int> { 2006, 2007 },
            location = "Police_Station_1892",
            hasHint = true,
            hint = "Bölüm zaman çizelgesini sor",
            difficulty = 6,
            completionProgress = 0f
        });

        // BÖLÜM 3: Black Dahlia
        allObjectives.Add(new ObjectiveTask
        {
            taskId = 3001,
            taskTitle = "Hollywood'u Araştır",
            taskDescription = "Black Dahlia'nın Hollywood bağlantılarını keşfet",
            taskType = "Investigation",
            chapterId = 3,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 700,
            linkedCharacterId = "",
            linkedEvidenceIds = new List<int> { 3001, 3002, 3003 },
            location = "Hollywood_Boulevard",
            hasHint = true,
            hint = "Yapımcı ve yönetmenleri sor",
            difficulty = 6,
            completionProgress = 0f
        });

        allObjectives.Add(new ObjectiveTask
        {
            taskId = 3002,
            taskTitle = "Dr. Hodel'i Sorguला",
            taskDescription = "Şüpheli Dr. George Hodel'i sorgulamaya çağır",
            taskType = "Interrogation",
            chapterId = 3,
            isCompleted = false,
            isOptional = false,
            rewardPoints = 900,
            linkedCharacterId = "302",
            linkedEvidenceIds = new List<int> { 3004, 3005, 3006 },
            location = "Police_Station_1947",
            hasHint = true,
            hint = "Tıbbi geçmişini araştır",
            difficulty = 8,
            completionProgress = 0f
        });

        Debug.Log("✓ " + allObjectives.Count + " Amaç Yüklendi");
    }

    private void InitializeQuestLines()
    {
        // BÖLÜM 1 ANA QUEST
        QuestLine chapter1MainQuest = new QuestLine
        {
            questLineId = 101,
            questName = "Jack the Ripper'ı Yakalayın",
            questDescription = "Whitechapel cinayetlerini çöz ve suçluyu bulunuz",
            chapterId = 1,
            objectives = new List<int> { 1001, 1002, 1003, 1004, 1005 },
            isMainQuest = true,
            isActive = true,
            isCompleted = false,
            rewardPoints = 3500,
            rewardAchievement = "Ripper_Hunter",
            completionPercentage = 0f,
            currentObjectiveIndex = 0
        };
        questLines.Add(chapter1MainQuest);
        currentActiveQuest = chapter1MainQuest;

        // BÖLÜM 1 SIDE QUEST
        QuestLine chapter1SideQuest = new QuestLine
        {
            questLineId = 102,
            questName = "Polis Müdürü'nü Yardım Et",
            questDescription = "Müdür Morse'a gizli bilgiler sağla",
            chapterId = 1,
            objectives = new List<int> { 1006, 1007 },
            isMainQuest = false,
            isActive = false,
            isCompleted = false,
            rewardPoints = 1500,
            rewardAchievement = "Police_Helper",
            completionPercentage = 0f,
            currentObjectiveIndex = 0
        };
        questLines.Add(chapter1SideQuest);

        // BÖLÜM 2 ANA QUEST
        QuestLine chapter2MainQuest = new QuestLine
        {
            questLineId = 201,
            questName = "Lizzie Borden Davası",
            questDescription = "Borden ailesinin cinayetini çöz",
            chapterId = 2,
            objectives = new List<int> { 2001, 2002, 2003, 2004 },
            isMainQuest = true,
            isActive = false,
            isCompleted = false,
            rewardPoints = 3500,
            rewardAchievement = "Borden_Case_Closed",
            completionPercentage = 0f,
            currentObjectiveIndex = 0
        };
        questLines.Add(chapter2MainQuest);

        // BÖLÜM 3 ANA QUEST
        QuestLine chapter3MainQuest = new QuestLine
        {
            questLineId = 301,
            questName = "Black Dahlia Gizemi",
            questDescription = "Hollywood'un en ünlü cinayetini çöz",
            chapterId = 3,
            objectives = new List<int> { 3001, 3002, 3003 },
            isMainQuest = true,
            isActive = false,
            isCompleted = false,
            rewardPoints = 4000,
            rewardAchievement = "Dahlia_Truth",
            completionPercentage = 0f,
            currentObjectiveIndex = 0
        };
        questLines.Add(chapter3MainQuest);

        Debug.Log("✓ " + questLines.Count + " Quest Line Yüklendi");
    }

    public void CompleteObjective(int objectiveId)
    {
        ObjectiveTask objective = GetObjectiveById(objectiveId);
        if (objective == null)
        {
            Debug.LogWarning("⚠️ Amaç bulunamadı: " + objectiveId);
            return;
        }

        objective.isCompleted = true;
        objective.completionProgress = 1f;
        totalObjectivesCompleted++;

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("✅ AMAÇ TAMAMLANDI!");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Amaç: " + objective.taskTitle);
        Debug.Log("Puan: +" + objective.rewardPoints);
        Debug.Log("═══════════════════════════════════════\n");

        // Bağlı quest'i kontrol et
        UpdateRelatedQuestProgress(objectiveId);

        // UI güncellemesini tetikle
        UICanvasManager.instance.ShowNotification("✅ " + objective.taskTitle + " tamamlandı!");
        AudioManager.instance.PlaySFX("sfx_objective_complete");
    }

    public void UpdateObjectiveProgress(int objectiveId, float progress)
    {
        ObjectiveTask objective = GetObjectiveById(objectiveId);
        if (objective == null)
            return;

        objective.completionProgress = Mathf.Clamp01(progress);
        
        if (progress >= 1f && !objective.isCompleted)
        {
            CompleteObjective(objectiveId);
        }

        Debug.Log("📊 " + objective.taskTitle + ": " + (progress * 100) + "%");
    }

    private void UpdateRelatedQuestProgress(int objectiveId)
    {
        foreach (QuestLine quest in questLines)
        {
            if (quest.objectives.Contains(objectiveId))
            {
                int completedCount = 0;
                foreach (int objId in quest.objectives)
                {
                    ObjectiveTask obj = GetObjectiveById(objId);
                    if (obj != null && obj.isCompleted)
                        completedCount++;
                }

                quest.completionPercentage = (float)completedCount / quest.objectives.Count;

                if (quest.completionPercentage >= 1f && !quest.isCompleted)
                {
                    CompleteQuestLine(quest.questLineId);
                }

                Debug.Log("📈 Quest: " + quest.questName + " - " + (quest.completionPercentage * 100) + "%");
            }
        }
    }

    public void CompleteQuestLine(int questLineId)
    {
        QuestLine quest = GetQuestLineById(questLineId);
        if (quest == null || quest.isCompleted)
            return;

        quest.isCompleted = true;
        quest.completionPercentage = 1f;
        totalQuestsCompleted++;

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🎉 QUEST TAMAMLANDI!");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Quest: " + quest.questName);
        Debug.Log("Puan: +" + quest.rewardPoints);
        Debug.Log("Başarı: " + quest.rewardAchievement);
        Debug.Log("═══════════════════════════════════════\n");

        // Başarı aç
        AchievementSystem.instance.UnlockAchievementByName(quest.rewardAchievement);

        // UI güncellemesini tetikle
        UICanvasManager.instance.ShowNotification("🎉 " + quest.questName + " tamamlandı!");
        AudioManager.instance.PlaySFX("sfx_quest_complete");
    }

    public void ActivateQuestLine(int questLineId)
    {
        QuestLine quest = GetQuestLineById(questLineId);
        if (quest == null)
            return;

        quest.isActive = true;
        currentActiveQuest = quest;

        Debug.Log("⭐ Active Quest: " + quest.questName);
    }

    public void AddJournalEntry(int chapterId, string title, string content, List<int> linkedObjectives)
    {
        QuestJournal entry = new QuestJournal
        {
            journalEntryId = journal.Count + 1,
            entryTitle = title,
            entryContent = content,
            chapterId = chapterId,
            entryDate = System.DateTime.Now,
            linkedObjectives = linkedObjectives,
            linkedCharacters = new List<int>(),
            isImportant = false,
            isSolved = false
        };

        journal.Add(entry);

        Debug.Log("📔 Günlük Girdisi Eklendi: " + title);
    }

    public void CreateMarker(int markerType, string markerName, Vector3 position, int linkedObjective)
    {
        ObjectiveMarker marker = new ObjectiveMarker
        {
            markerId = markers.Count + 1,
            markerName = markerName,
            markerPosition = position,
            markerType = markerType == 0 ? "Location" : (markerType == 1 ? "Character" : "Evidence"),
            linkedObjectiveId = linkedObjective,
            isDiscovered = false,
            isPinned = false
        };

        markers.Add(marker);

        Debug.Log("📍 Marker Oluşturuldu: " + markerName + " (" + marker.markerType + ")");
    }

    public void DiscoverMarker(int markerId)
    {
        ObjectiveMarker marker = GetMarkerById(markerId);
        if (marker != null)
        {
            marker.isDiscovered = true;
            Debug.Log("🔍 Marker Keşfedildi: " + marker.markerName);
        }
    }

    public void PinMarker(int markerId)
    {
        ObjectiveMarker marker = GetMarkerById(markerId);
        if (marker != null)
        {
            marker.isPinned = !marker.isPinned;
            Debug.Log(marker.isPinned ? "📌 Marker Sabitlendi: " : "📍 Marker Sabitleme Kaldırıldı: " + marker.markerName);
        }
    }

    // GETTER METHODS
    public ObjectiveTask GetObjectiveById(int objectiveId)
    {
        foreach (ObjectiveTask obj in allObjectives)
        {
            if (obj.taskId == objectiveId)
                return obj;
        }
        return null;
    }

    public QuestLine GetQuestLineById(int questLineId)
    {
        foreach (QuestLine quest in questLines)
        {
            if (quest.questLineId == questLineId)
                return quest;
        }
        return null;
    }

    public ObjectiveMarker GetMarkerById(int markerId)
    {
        foreach (ObjectiveMarker marker in markers)
        {
            if (marker.markerId == markerId)
                return marker;
        }
        return null;
    }

    public List<ObjectiveTask> GetActiveObjectives()
    {
        List<ObjectiveTask> activeObjs = new List<ObjectiveTask>();
        foreach (ObjectiveTask obj in allObjectives)
        {
            if (!obj.isCompleted && currentActiveQuest.objectives.Contains(obj.taskId))
                activeObjs.Add(obj);
        }
        return activeObjs;
    }

    public List<QuestLine> GetAllQuestLines() => questLines;
    public QuestLine GetCurrentActiveQuest() => currentActiveQuest;
    public List<QuestJournal> GetJournalEntries() => journal;
    public List<ObjectiveMarker> GetAllMarkers() => markers;
    public int GetTotalQuestsCompleted() => totalQuestsCompleted;
    public int GetTotalObjectivesCompleted() => totalObjectivesCompleted;
}
