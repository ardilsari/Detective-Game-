using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    // Detective Desk UI
    [SerializeField] private Transform witnessPhotosContainer;
    [SerializeField] private Transform evidenceListContainer;
    [SerializeField] private Text caseInfoText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text daysText;

    // Witness Interrogation UI
    [SerializeField] private GameObject interrogationPanel;
    [SerializeField] private Text witnessNameText;
    [SerializeField] private Text witnessInfoText;
    [SerializeField] private Text statementText;
    [SerializeField] private Button nextStatementButton;

    // Evidence Analysis UI
    [SerializeField] private GameObject evidencePanel;
    [SerializeField] private Text evidenceNameText;
    [SerializeField] private Text evidenceDetailsText;

    // Suspect Selection UI
    [SerializeField] private GameObject suspectPanel;
    [SerializeField] private Transform suspectButtonsContainer;
    [SerializeField] private Button accuseButton;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        InitializeUI();
    }

    private void InitializeUI()
    {
        DisplayCaseInfo();
        DisplayWitnesses();
        DisplayEvidences();
        UpdateScoreAndDays();
    }

    public void DisplayCaseInfo()
    {
        Case currentCase = CaseData.GetCase();
        if (currentCase == null) return;

        string caseInfo = "Dava: " + currentCase.caseName + "\n";
        caseInfo += "Açıklama: " + currentCase.caseDescription + "\n";
        caseInfo += "Tarih: " + currentCase.date + "\n";
        caseInfo += "Yer: " + currentCase.location;

        if (caseInfoText != null)
            caseInfoText.text = caseInfo;
    }

    public void DisplayWitnesses()
    {
        Case currentCase = CaseData.GetCase();
        if (currentCase == null || witnessPhotosContainer == null) return;

        // Clear previous buttons
        foreach (Transform child in witnessPhotosContainer)
        {
            Destroy(child.gameObject);
        }

        // Create witness buttons
        foreach (Witness witness in currentCase.witnesses)
        {
            Button witnessButton = CreateButton(witness.name, () => SelectWitness(witness.id));
            witnessButton.transform.SetParent(witnessPhotosContainer);
        }
    }

    public void DisplayEvidences()
    {
        Case currentCase = CaseData.GetCase();
        if (currentCase == null || evidenceListContainer == null) return;

        // Clear previous buttons
        foreach (Transform child in evidenceListContainer)
        {
            Destroy(child.gameObject);
        }

        // Create evidence buttons
        foreach (Evidence evidence in currentCase.evidences)
        {
            Button evidenceButton = CreateButton(evidence.name, () => SelectEvidence(evidence.id));
            evidenceButton.transform.SetParent(evidenceListContainer);
        }
    }

    public void SelectWitness(int witnessId)
    {
        if (interrogationPanel == null) return;

        Witness witness = CaseData.GetWitness(witnessId);
        if (witness == null) return;

        interrogationPanel.SetActive(true);
        witnessNameText.text = witness.name;
        witnessInfoText.text = "Meslek: " + witness.profession + "\nBulunduğu Yer: " + witness.location;
        statementText.text = witness.statements.Count > 0 ? witness.statements[0] : "İfade yok";

        gameManager.InterrogateWitness(witnessId);
    }

    public void SelectEvidence(int evidenceId)
    {
        if (evidencePanel == null) return;

        Evidence evidence = CaseData.GetEvidence(evidenceId);
        if (evidence == null) return;

        evidencePanel.SetActive(true);
        evidenceNameText.text = evidence.name;
        evidenceDetailsText.text = "Açıklama: " + evidence.description + "\n" +
                                   "Yer: " + evidence.location + "\n" +
                                   "Adli Tıp: " + evidence.forensicDetails;

        gameManager.CollectEvidence(evidenceId);
        UpdateScoreAndDays();
    }

    public void DisplaySuspects()
    {
        Case currentCase = CaseData.GetCase();
        if (currentCase == null || suspectButtonsContainer == null) return;

        // Clear previous buttons
        foreach (Transform child in suspectButtonsContainer)
        {
            Destroy(child.gameObject);
        }

        // Create suspect buttons
        foreach (CriminalProfile suspect in currentCase.suspectProfiles)
        {
            Button suspectButton = CreateButton(suspect.name, () => AccuseSuspect(suspect.id));
            suspectButton.transform.SetParent(suspectButtonsContainer);
        }
    }

    public void AccuseSuspect(int suspectId)
    {
        gameManager.AccuseSuspect(suspectId);
        suspectPanel.SetActive(false);
    }

    public void UpdateScoreAndDays()
    {
        if (scoreText != null)
            scoreText.text = "Puan: " + gameManager.GetScore();

        if (daysText != null)
            daysText.text = "Kalan Gün: " + gameManager.GetDaysRemaining();
    }

    private Button CreateButton(string label, UnityEngine.Events.UnityAction callback)
    {
        GameObject buttonObj = new GameObject(label);
        Button button = buttonObj.AddComponent<Button>();
        Text text = buttonObj.AddComponent<Text>();

        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;

        button.onClick.AddListener(callback);

        return button;
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
