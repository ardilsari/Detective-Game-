using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ChapterScene
{
    public int chapterId;
    public string sceneName;
    public string sceneDescription;
    public List<string> locations; // Whitechapel, Asylum, Police Station, etc.
    public List<string> characters; // Karakterlerin prefab isimleri
    public string backgroundMusic;
    public bool isUnlocked;
    public bool isCompleted;
}

[System.Serializable]
public class LocationData
{
    public string locationName;
    public Vector3 spawnPosition;
    public Vector3 cameraPosition;
    public List<string> availableCharacters;
    public List<string> availableEvidence;
    public string environmentAudio;
    public bool isVisited;
}

[System.Serializable]
public class EnvironmentState
{
    public string environmentName;
    public bool[] objectStates; // Hangi objeler açık/kapalı
    public Color lightingColor;
    public float lightingIntensity;
    public int timeOfDay; // 0-23 saat
}

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    private List<ChapterScene> chapterScenes = new List<ChapterScene>();
    private List<LocationData> currentLocations = new List<LocationData>();
    private EnvironmentState currentEnvironment;
    private int currentChapterId = -1;
    private string currentLocationName = "";

    // Performance
    private bool isLoadingScene = false;
    private float sceneLoadProgress = 0f;

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
        InitializeChapterScenes();
        Debug.Log("✓ Scene Manager Hazırlandı");
    }

    private void InitializeChapterScenes()
    {
        // BÖLÜM 1: Jack the Ripper - Londra 1888
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 1,
            sceneName = "Chapter1_Whitechapel",
            sceneDescription = "Jack the Ripper cinayetlerinin gerçekleştiği Whitechapel bölgesi",
            locations = new List<string>
            {
                "Whitechapel_Street",
                "Police_Station_1888",
                "Medical_School",
                "Asylum",
                "Morgue",
                "Victim_Home_1",
                "Victim_Home_2",
                "Pub_Ten_Bells"
            },
            characters = new List<string>
            {
                "Character_Druitt",
                "Character_Kosminski",
                "Character_Morse",
                "Character_Detective",
                "Character_Witness_1",
                "Character_Witness_2",
                "Character_Police_Inspector"
            },
            backgroundMusic = "Music_Victorian_London",
            isUnlocked = true,
            isCompleted = false
        });

        // BÖLÜM 2: Lizzie Borden - Amerika 1892
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 2,
            sceneName = "Chapter2_FallRiver",
            sceneDescription = "Lizzie Borden cinayetinin gerçekleştiği Fall River evi",
            locations = new List<string>
            {
                "Borden_House",
                "Borden_Kitchen",
                "Borden_Bedroom",
                "Borden_Basement",
                "Police_Station_1892",
                "Cemetery",
                "Town_Square",
                "Neighbor_House"
            },
            characters = new List<string>
            {
                "Character_Lizzie",
                "Character_Andrew_Borden",
                "Character_Abby_Borden",
                "Character_Bridget",
                "Character_Morse_1892",
                "Character_Police_Chief",
                "Character_Neighbor"
            },
            backgroundMusic = "Music_Victorian_America",
            isUnlocked = false,
            isCompleted = false
        });

        // BÖLÜM 3: Black Dahlia - Los Angeles 1947
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 3,
            sceneName = "Chapter3_Hollywood",
            sceneDescription = "Black Dahlia cinayetinin gerçekleştiği 1940s Hollywood",
            locations = new List<string>
            {
                "Hollywood_Boulevard",
                "Crime_Scene",
                "Police_Station_1947",
                "Celebrity_Club",
                "Hotel_Cortez",
                "Elizabeth_Apartment",
                "Hospital",
                "FBI_Office"
            },
            characters = new List<string>
            {
                "Character_Elizabeth_Short",
                "Character_Dr_Hodel",
                "Character_Leslie_Dillon",
                "Character_Jack_Wilson",
                "Character_Hollywood_Celebrity_1",
                "Character_Police_Detective",
                "Character_FBI_Agent"
            },
            backgroundMusic = "Music_1940s_Jazz",
            isUnlocked = false,
            isCompleted = false
        });

        // BÖLÜM 4: Axeman - New Orleans 1918
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 4,
            sceneName = "Chapter4_NewOrleans",
            sceneDescription = "Axeman'in terörü altında New Orleans",
            locations = new List<string>
            {
                "French_Quarter",
                "Crime_Scene_1",
                "Crime_Scene_2",
                "Italian_Barbershop",
                "Police_Station_1918",
                "Mafia_Hideout",
                "Victim_Home",
                "Newspaper_Office"
            },
            characters = new List<string>
            {
                "Character_Axeman",
                "Character_Joseph_Mumfre",
                "Character_Mafia_Boss",
                "Character_Police_Captain",
                "Character_Survivor",
                "Character_Newspaper_Reporter"
            },
            backgroundMusic = "Music_Jazz_Blues_1918",
            isUnlocked = false,
            isCompleted = false
        });

        // BÖLÜM 5: Hinterkaifeck - Bavyera 1921
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 5,
            sceneName = "Chapter5_Bavyera",
            sceneDescription = "Uzak Hinterkaifeck çiftliğinde gizem",
            locations = new List<string>
            {
                "Hinterkaifeck_Farm",
                "Farm_House",
                "Farm_Barn",
                "Farm_Woods",
                "Village_Center",
                "Police_Station_1921",
                "Asylum_1921",
                "Neighbors_Farm"
            },
            characters = new List<string>
            {
                "Character_Andreas_Gruber",
                "Character_Cazilia_Gruber",
                "Character_Giesel",
                "Character_Karl_Gabriel",
                "Character_Viktoria_Gabriel",
                "Character_Police_Chief_1921",
                "Character_Village_Elder"
            },
            backgroundMusic = "Music_Dark_Bavarian",
            isUnlocked = false,
            isCompleted = false
        });

        // BÖLÜM 6: Villisca - Iowa 1912
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 6,
            sceneName = "Chapter6_Villisca",
            sceneDescription = "Villisca cinayetlerinin İowa evi",
            locations = new List<string>
            {
                "Moore_House",
                "Crime_Scene_Bedroom",
                "Crime_Scene_Living_Room",
                "Police_Station_1912",
                "Train_Station",
                "Cemetery",
                "Town_Jail",
                "Church"
            },
            characters = new List<string>
            {
                "Character_Josiah_Moore",
                "Character_Sarah_Moore",
                "Character_Henry_Moore_Serial",
                "Character_Bridget_Villisca",
                "Character_Police_Detective_1912",
                "Character_Suspect_Arrest",
                "Character_Town_Mayor"
            },
            backgroundMusic = "Music_Grim_Americana",
            isUnlocked = false,
            isCompleted = false
        });

        // BÖLÜM 7: Tüm Bağlantılar Açılıyor - Multi-Location
        chapterScenes.Add(new ChapterScene
        {
            chapterId = 7,
            sceneName = "Chapter7_GlobalConnections",
            sceneDescription = "Tüm cinayetlerin bağlandığı son bölüm. Seri katil ağı ortaya çıkıyor.",
            locations = new List<string>
            {
                "FBI_Headquarters",
                "International_Archives",
                "Crime_Evidence_Vault",
                "Historical_Database",
                "Detective_Office",
                "Court_Room",
                "Final_Confrontation",
                "Secret_Chamber"
            },
            characters = new List<string>
            {
                "Character_FBI_Director",
                "Character_All_Previous_Suspects",
                "Character_Secret_Killer",
                "Character_Historian_Mentor",
                "Character_Police_Commissioner",
                "Character_News_Reporter"
            },
            backgroundMusic = "Music_Epic_Finale",
            isUnlocked = false,
            isCompleted = false
        });

        Debug.Log("✓ " + chapterScenes.Count + " Bölüm Sahne Yüklendi");
    }

    public void LoadChapter(int chapterId)
    {
        ChapterScene chapter = GetChapterScene(chapterId);
        if (chapter == null)
        {
            Debug.LogError("❌ Bölüm bulunamadı: " + chapterId);
            return;
        }

        if (!chapter.isUnlocked)
        {
            Debug.LogError("❌ Bu bölüm henüz açılmamış!");
            return;
        }

        currentChapterId = chapterId;
        isLoadingScene = true;

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("📖 BÖLÜM YÜKLENIYOR...");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Bölüm: " + chapter.chapterName);
        Debug.Log("Sahne: " + chapter.sceneName);
        Debug.Log("Açıklama: " + chapter.sceneDescription);
        Debug.Log("═══════════════════════════════════════\n");

        // Sahne yükle
        LoadSceneAsync(chapter.sceneName);

        // Lokasyonları başlat
        InitializeLocations(chapter.locations);

        // Müzik başlat
        AudioManager audioMgr = AudioManager.instance;
        if (audioMgr != null)
        {
            audioMgr.PlayBackgroundMusic(chapter.backgroundMusic);
        }
    }

    private void LoadSceneAsync(string sceneName)
    {
        // TODO: Async scene loading with progress bar
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        isLoadingScene = false;
        Debug.Log("✓ Sahne Yüklendi: " + sceneName);
    }

    private void InitializeLocations(List<string> locationNames)
    {
        currentLocations.Clear();

        foreach (string locName in locationNames)
        {
            LocationData location = new LocationData
            {
                locationName = locName,
                spawnPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)),
                cameraPosition = new Vector3(0, 5, -10),
                availableCharacters = new List<string>(),
                availableEvidence = new List<string>(),
                isVisited = false
            };

            currentLocations.Add(location);
        }

        Debug.Log("✓ " + currentLocations.Count + " Lokasyon Başlatıldı");
    }

    public void TravelToLocation(string locationName)
    {
        LocationData location = GetLocation(locationName);
        if (location == null)
        {
            Debug.LogError("❌ Lokasyon bulunamadı: " + locationName);
            return;
        }

        currentLocationName = locationName;
        location.isVisited = true;

        Debug.Log("\n🚶 Lokasyona Gidiliyor: " + locationName);
        Debug.Log("Spawn Pozisyonu: " + location.spawnPosition);
        Debug.Log("Kamera Pozisyonu: " + location.cameraPosition);

        // TODO: Player pozisyonunu güncelle
        // TODO: Kamerayı taşı
        // TODO: Ortam sesini değiştir
    }

    public void SetEnvironmentState(string environmentName, int timeOfDay, Color lighting, float intensity)
    {
        currentEnvironment = new EnvironmentState
        {
            environmentName = environmentName,
            timeOfDay = timeOfDay,
            lightingColor = lighting,
            lightingIntensity = intensity,
            objectStates = new bool[0]
        };

        Debug.Log("🌅 Ortam Durumu Değiştirildi");
        Debug.Log("Saat: " + timeOfDay + ":00");
        Debug.Log("Işık Rengi: " + lighting);
        Debug.Log("Işık Yoğunluğu: " + intensity);

        // TODO: Unity'deki lighting sistemi güncelle
    }

    public void UnlockNextChapter()
    {
        if (currentChapterId < chapterScenes.Count)
        {
            ChapterScene nextChapter = chapterScenes[currentChapterId];
            if (nextChapter != null)
            {
                nextChapter.isUnlocked = true;
                Debug.Log("🔓 Bölüm " + (currentChapterId + 1) + " Açıldı!");
            }
        }
    }

    public void CompleteChapter()
    {
        ChapterScene chapter = GetChapterScene(currentChapterId);
        if (chapter != null)
        {
            chapter.isCompleted = true;
            Debug.Log("✓ Bölüm " + currentChapterId + " Tamamlandı!");
            UnlockNextChapter();
        }
    }

    private ChapterScene GetChapterScene(int chapterId)
    {
        foreach (ChapterScene scene in chapterScenes)
        {
            if (scene.chapterId == chapterId)
                return scene;
        }
        return null;
    }

    private LocationData GetLocation(string locationName)
    {
        foreach (LocationData location in currentLocations)
        {
            if (location.locationName == locationName)
                return location;
        }
        return null;
    }

    public List<ChapterScene> GetAllChapters() => chapterScenes;
    public List<LocationData> GetCurrentLocations() => currentLocations;
    public int GetCurrentChapterId() => currentChapterId;
    public string GetCurrentLocation() => currentLocationName;
    public EnvironmentState GetEnvironmentState() => currentEnvironment;
    public bool IsLoadingScene() => isLoadingScene;
    public float GetSceneLoadProgress() => sceneLoadProgress;
}
