using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private WitnessSystem witnessSystem;
    private EvidenceSystem evidenceSystem;
    private UIManager uiManager;
    
    private int daysRemaining;
    private int playerScore = 0;
    private bool caseResolved = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        witnessSystem = GetComponent<WitnessSystem>();
        evidenceSystem = GetComponent<EvidenceSystem>();
        uiManager = GetComponent<UIManager>();

        if (witnessSystem == null)
            gameObject.AddComponent<WitnessSystem>();
        if (evidenceSystem == null)
            gameObject.AddComponent<EvidenceSystem>();
        if (uiManager == null)
            gameObject.AddComponent<UIManager>();

        witnessSystem = GetComponent<WitnessSystem>();
        evidenceSystem = GetComponent<EvidenceSystem>();
        uiManager = GetComponent<UIManager>();

        InitializeGame();
    }

    private void InitializeGame()
    {
        // Örnek olay başlat
        Case sampleCase = CreateSampleCase();
        CaseData.InitializeCase(sampleCase);
        
        daysRemaining = sampleCase.daysToSolve;
        Debug.Log("Oyun Başladı! Çözülecek Gün Sayısı: " + daysRemaining);
    }

    private Case CreateSampleCase()
    {
        Case newCase = new Case
        {
            caseId = 1,
            caseName = "Gizli Cinayet",
            caseDescription = "Londra'da bir cinayet meydana geldi. Tanıkları sorgula ve suçluyu bul.",
            date = "1888",
            location = "Londra",
            daysToSolve = 7,
            correctSuspectId = 1
        };

        // Tanıklar
        newCase.witnesses = new System.Collections.Generic.List<Witness>
        {
            new Witness
            {
                id = 1,
                name = "Ahmet Bey",
                profession = "Esnaf",
                location = "Sokak",
                isSuspect = false,
                statements = new System.Collections.Generic.List<string>
                {
                    "Ben o gece sokakta idi.",
                    "Şüpheli birini gördüm.",
                    "Hızlı bir şekilde kaçtı."
                },
                clues = new System.Collections.Generic.List<string>
                {
                    "Kırmızı ceket giyiyordu",
                    "Koyu renk saçlı"
                }
            },
            new Witness
            {
                id = 2,
                name = "Fatma Hanım",
                profession = "Hemşire",
                location = "Hastane",
                isSuspect = true,
                statements = new System.Collections.Generic.List<string>
                {
                    "Ben o saatte uykudaydım.",
                    "Hiçbir şey görmedim."
                },
                clues = new System.Collections.Generic.List<string>
                {
                    "Garip davranışlar sergiledi",
                    "Tıbbi bilgisi vardı"
                }
            },
            new Witness
            {
                id = 3,
                name = "Mehmet Ağa",
                profession = "Polis",
                location = "Karakol",
                isSuspect = false,
                statements = new System.Collections.Generic.List<string>
                {
                    "Olay yerine ilk ben gittim.",
                    "İzler vardı."
                },
                clues = new System.Collections.Generic.List<string>
                {
                    "Ayakkabı izleri buldum",
                    "Kan damlacıkları tespit ettim"
                }
            }
        };

        // Kanıtlar
        newCase.evidences = new System.Collections.Generic.List<Evidence>
        {
            new Evidence
            {
                id = 1,
                name = "Kırmızı Ceket",
                description = "Olay yerinde bulunan kırmızı ceket",
                location = "Olay Yeri",
                forensicDetails = "Kurbanın kanı tespit edildi"
            },
            new Evidence
            {
                id = 2,
                name = "Ayakkabı İzleri",
                description = "Olay yerindeki ayakkabı izleri",
                location = "Olay Yeri",
                forensicDetails = "Hemşirenin ayakkabısıyla eşleşti"
            },
            new Evidence
            {
                id = 3,
                name = "Tıbbi Enstrüman",
                description = "Hemşirenin tıbbi enstrümanı",
                location = "Hastane",
                forensicDetails = "Kurbanın yarasıyla eşleşen keskin nesne"
            }
        };

        // Şüpheliler
        newCase.suspectProfiles = new System.Collections.Generic.List<CriminalProfile>
        {
            new CriminalProfile
            {
                id = 1,
                name = "Fatma Hanım",
                description = "Tıbbi bilgiye sahip, garip davranışlar",
                evidenceIds = new System.Collections.Generic.List<int> { 2, 3 },
                witnessIds = new System.Collections.Generic.List<int> { 1, 3 }
            },
            new CriminalProfile
            {
                id = 2,
                name = "Ahmet Bey",
                description = "Olay yerinde görüldü",
                evidenceIds = new System.Collections.Generic.List<int> { 1 },
                witnessIds = new System.Collections.Generic.List<int> { 1 }
            }
        };

        return newCase;
    }

    public void InterrogateWitness(int witnessId)
    {
        witnessSystem.InterrogateWitness(witnessId);
    }

    public void CollectEvidence(int evidenceId)
    {
        evidenceSystem.CollectEvidence(evidenceId);
        playerScore += 10;
    }

    public void AccuseSuspect(int suspectId)
    {
        if (caseResolved)
        {
            Debug.Log("Dava zaten çözüldü!");
            return;
        }

        Case currentCase = CaseData.GetCase();
        if (currentCase.correctSuspectId == suspectId)
        {
            CaseSolved(true);
        }
        else
        {
            CaseSolved(false);
        }
    }

    private void CaseSolved(bool isCorrect)
    {
        caseResolved = true;
        
        if (isCorrect)
        {
            playerScore += 100;
            Debug.Log("✓ DOĞRU! Suçluyu buldunuz!");
            Debug.Log("Final Puanı: " + playerScore);
        }
        else
        {
            playerScore = Mathf.Max(0, playerScore - 50);
            Debug.Log("✗ YANLIŞ! Yanlış kişiyi suçladınız.");
            Debug.Log("Puanınız: " + playerScore);
        }
    }

    public void PassDay()
    {
        daysRemaining--;
        Debug.Log("Günler Geçti. Kalan Gün: " + daysRemaining);

        if (daysRemaining <= 0)
        {
            Debug.Log("ZAMAN BİTTİ! Davayı çözemediz.");
            caseResolved = true;
        }
    }

    public int GetScore()
    {
        return playerScore;
    }

    public int GetDaysRemaining()
    {
        return daysRemaining;
    }

    public bool IsCaseResolved()
    {
        return caseResolved;
    }
}
