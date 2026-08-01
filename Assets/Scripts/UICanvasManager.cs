using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UICanvasManager : MonoBehaviour
{
    public static UICanvasManager instance;

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private CanvasScaler canvasScaler;

    // PANELS
    private GameObject mainMenuPanel;
    private GameObject chapterSelectPanel;
    private GameObject gameplayPanel;
    private GameObject interrogationPanel;
    private GameObject evidencePanel;
    private GameObject leaderboardPanel;
    private GameObject settingsPanel;
    private GameObject achievementPanel;

    // UI ELEMENTS
    private Text chapterTitleText;
    private Text scoreText;
    private Text timerText;
    private Text suspectNameText;
    private Button nextSuspectBtn;
    private Button submitAnswerBtn;
    private List<Button> decisionButtons = new List<Button>();

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
        InitializeCanvas();
        InitializePanels();
        InitializeMainMenu();
    }

    private void InitializeCanvas()
    {
        mainCanvas = GetComponent<Canvas>();
        canvasScaler = GetComponent<CanvasScaler>();
        
        // Responsive tasarım
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        Debug.Log("✓ Canvas Başlatıldı (1920x1080)");
    }

    private void InitializePanels()
    {
        // MAIN MENU PANEL
        mainMenuPanel = CreatePanel("MainMenuPanel");
        AddButtonToPanel(mainMenuPanel, "Oyuna Başla", () => ShowChapterSelect());
        AddButtonToPanel(mainMenuPanel, "Leaderboard", () => ShowLeaderboard());
        AddButtonToPanel(mainMenuPanel, "Başarılar", () => ShowAchievements());
        AddButtonToPanel(mainMenuPanel, "Ayarlar", () => ShowSettings());
        AddButtonToPanel(mainMenuPanel, "Çıkış", () => Application.Quit());

        // CHAPTER SELECT PANEL
        chapterSelectPanel = CreatePanel("ChapterSelectPanel");
        CreateChapterButtons();

        // GAMEPLAY PANEL
        gameplayPanel = CreatePanel("GameplayPanel");
        AddTextToPanel(gameplayPanel, "Puan: ", "ScoreText", new Vector2(150, 50));
        AddTextToPanel(gameplayPanel, "Zaman: 00:00", "TimerText", new Vector2(150, 100));
        chapterTitleText = AddTextToPanel(gameplayPanel, "Bölüm 1: Jack the Ripper", "ChapterTitle", new Vector2(960, 1000));

        // INTERROGATION PANEL
        interrogationPanel = CreatePanel("InterrogationPanel");
        suspectNameText = AddTextToPanel(interrogationPanel, "Tanık Adı", "SuspectName", new Vector2(960, 900));
        AddTextToPanel(interrogationPanel, "Sorgulama Notları", "InterrogationText", new Vector2(960, 600));
        nextSuspectBtn = AddButtonToPanel(interrogationPanel, "Sonraki Tanık", () => NextSuspect());

        // EVIDENCE PANEL
        evidencePanel = CreatePanel("EvidencePanel");
        AddTextToPanel(evidencePanel, "Kanıt Listesi", "EvidenceTitle", new Vector2(960, 1000));
        AddButtonToPanel(evidencePanel, "Kanıt Ayrıntısı", () => ShowEvidenceDetail());

        // DECISION PANEL (BRANCHING)
        CreateDecisionButtons();

        // LEADERBOARD PANEL
        leaderboardPanel = CreatePanel("LeaderboardPanel");
        AddTextToPanel(leaderboardPanel, "GLOBAL LEADERBOARD", "LeaderboardTitle", new Vector2(960, 1000));
        CreateLeaderboardTable();

        // SETTINGS PANEL
        settingsPanel = CreatePanel("SettingsPanel");
        CreateSettingsOptions();

        // ACHIEVEMENT PANEL
        achievementPanel = CreatePanel("AchievementPanel");
        AddTextToPanel(achievementPanel, "BAŞARILAR", "AchievementTitle", new Vector2(960, 1000));
        CreateAchievementGrid();

        Debug.Log("✓ Tüm Panel'ler Başlatıldı");
    }

    private void InitializeMainMenu()
    {
        mainMenuPanel.SetActive(true);
        chapterSelectPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        interrogationPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        settingsPanel.SetActive(false);
        achievementPanel.SetActive(false);

        Debug.Log("✓ Ana Menü Gösterildi");
    }

    // PANEL CREATION
    private GameObject CreatePanel(string panelName)
    {
        GameObject panel = new GameObject(panelName);
        panel.transform.SetParent(mainCanvas.transform, false);
        
        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.15f, 0.95f); // Koyu arka plan

        return panel;
    }

    private Button AddButtonToPanel(GameObject panel, string buttonText, System.Action onClick, Vector2 position = default)
    {
        GameObject buttonObj = new GameObject("Button_" + buttonText);
        buttonObj.transform.SetParent(panel.transform, false);

        RectTransform btnRect = buttonObj.AddComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(300, 60);
        btnRect.anchoredPosition = position;

        Image btnImage = buttonObj.AddComponent<Image>();
        btnImage.color = new Color(0.2f, 0.6f, 0.8f, 1f); // Mavi

        Button btn = buttonObj.AddComponent<Button>();
        btn.onClick.AddListener(() => onClick());

        // Button Text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return btn;
    }

    private Text AddTextToPanel(GameObject panel, string textContent, string textName, Vector2 position)
    {
        GameObject textObj = new GameObject(textName);
        textObj.transform.SetParent(panel.transform, false);

        Text text = textObj.AddComponent<Text>();
        text.text = textContent;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 32;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(800, 100);
        textRect.anchoredPosition = position;

        return text;
    }

    // CHAPTER SELECTION
    private void CreateChapterButtons()
    {
        for (int i = 1; i <= 7; i++)
        {
            int chapterId = i;
            string chapterName = "Bölüm " + i;
            AddButtonToPanel(chapterSelectPanel, chapterName, () => StartChapter(chapterId), new Vector2(0, 500 - (i * 80)));
        }
    }

    private void StartChapter(int chapterId)
    {
        Debug.Log("📖 Bölüm " + chapterId + " Başlatıldı");
        mainMenuPanel.SetActive(false);
        chapterSelectPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        interrogationPanel.SetActive(true);

        ChapterManager chapterMgr = ChapterManager.instance;
        if (chapterMgr != null)
        {
            chapterMgr.LoadChapter(chapterId);
            chapterTitleText.text = chapterMgr.GetCurrentChapter().chapterName;
        }
    }

    // INTERROGATION SYSTEM
    private void NextSuspect()
    {
        Debug.Log("→ Sonraki tanığa geç");
        WitnessSystem witnessSys = WitnessSystem.instance;
        if (witnessSys != null)
        {
            // witnessSys.GetNextWitness();
        }
    }

    // DECISION BUTTONS (BRANCHING)
    private void CreateDecisionButtons()
    {
        for (int i = 0; i < 4; i++)
        {
            int decisionIndex = i;
            Button btn = AddButtonToPanel(gameplayPanel, "Seçenek " + (i + 1), () => MakeDecision(decisionIndex), new Vector2(0, -200 - (i * 80)));
            decisionButtons.Add(btn);
        }
    }

    private void MakeDecision(int decisionIndex)
    {
        Debug.Log("🔀 Karar Seçildi: #" + decisionIndex);
        StoryBranchingSystem storyBranch = StoryBranchingSystem.instance;
        if (storyBranch != null)
        {
            // storyBranch.MakeDecision(decisionIndex);
        }
    }

    // LEADERBOARD
    private void CreateLeaderboardTable()
    {
        SocialLeaderboardSystem leaderboard = SocialLeaderboardSystem.instance;
        if (leaderboard != null)
        {
            List<LeaderboardEntry> topPlayers = leaderboard.GetGlobalLeaderboard();
            
            // Sadece Top 10'u göster
            for (int i = 0; i < System.Math.Min(10, topPlayers.Count); i++)
            {
                LeaderboardEntry entry = topPlayers[i];
                string rankText = "#" + entry.rank + " | " + entry.player.username + " | " + entry.score + " puan";
                AddTextToPanel(leaderboardPanel, rankText, "Rank_" + i, new Vector2(960, 900 - (i * 60)));
            }
        }
    }

    // SETTINGS
    private void CreateSettingsOptions()
    {
        AddButtonToPanel(settingsPanel, "Müzik Ses: 100%", () => ShowVolumeSlider());
        AddButtonToPanel(settingsPanel, "Efekt Ses: 100%", () => ShowVolumeSlider());
        AddButtonToPanel(settingsPanel, "Grafik Kalitesi: Yüksek", () => ShowGraphicsMenu());
        AddButtonToPanel(settingsPanel, "Dil: Türkçe", () => ShowLanguageMenu());
        AddButtonToPanel(settingsPanel, "Geri Dön", () => ShowMainMenu());
    }

    private void ShowVolumeSlider() => Debug.Log("🔊 Ses Slider'ı Göster");
    private void ShowGraphicsMenu() => Debug.Log("🎨 Grafik Menüsü Göster");
    private void ShowLanguageMenu() => Debug.Log("🌐 Dil Menüsü Göster");

    // ACHIEVEMENTS
    private void CreateAchievementGrid()
    {
        AchievementSystem achSys = AchievementSystem.instance;
        if (achSys != null)
        {
            List<AchievementSystem.Achievement> achievements = achSys.GetAchievements();
            
            for (int i = 0; i < System.Math.Min(12, achievements.Count); i++)
            {
                var ach = achievements[i];
                string achText = (ach.isUnlocked ? "✓ " : "🔒 ") + ach.name;
                AddTextToPanel(achievementPanel, achText, "Achievement_" + i, new Vector2(300 + (i % 4) * 350, 800 - (i / 4) * 150)));
            }
        }
    }

    // PANEL NAVIGATION
    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
        Debug.Log("📍 Ana Menü");
    }

    public void ShowChapterSelect()
    {
        HideAllPanels();
        chapterSelectPanel.SetActive(true);
        Debug.Log("📍 Bölüm Seçimi");
    }

    public void ShowLeaderboard()
    {
        HideAllPanels();
        leaderboardPanel.SetActive(true);
        Debug.Log("📍 Leaderboard");
    }

    public void ShowAchievements()
    {
        HideAllPanels();
        achievementPanel.SetActive(true);
        Debug.Log("📍 Başarılar");
    }

    public void ShowSettings()
    {
        HideAllPanels();
        settingsPanel.SetActive(true);
        Debug.Log("📍 Ayarlar");
    }

    public void ShowEvidenceDetail()
    {
        HideAllPanels();
        evidencePanel.SetActive(true);
        Debug.Log("📍 Kanıt Ayrıntıları");
    }

    private void HideAllPanels()
    {
        mainMenuPanel.SetActive(false);
        chapterSelectPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        interrogationPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        settingsPanel.SetActive(false);
        achievementPanel.SetActive(false);
        evidencePanel.SetActive(false);
    }

    // RUNTIME UPDATES
    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Puan: " + score;
    }

    public void UpdateTimer(string timeString)
    {
        if (timerText != null)
            timerText.text = "Zaman: " + timeString;
    }

    public void ShowNotification(string message)
    {
        Debug.Log("💬 " + message);
        // TODO: Canvas üzerinde temporary notification göster
    }
}
