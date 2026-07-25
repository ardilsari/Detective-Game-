using System.Collections.Generic;
using UnityEngine;

public class EvidenceSystem : MonoBehaviour
{
    private List<Evidence> collectedEvidences = new List<Evidence>();
    private Dictionary<int, List<int>> evidenceConnections = new Dictionary<int, List<int>>();

    public void CollectEvidence(int evidenceId)
    {
        Evidence evidence = CaseData.GetEvidence(evidenceId);
        
        if (evidence == null)
        {
            Debug.LogError("Kanıt bulunamadı: " + evidenceId);
            return;
        }

        if (!collectedEvidences.Contains(evidence))
        {
            collectedEvidences.Add(evidence);
            Debug.Log("✓ Kanıt toplandı: " + evidence.name);
        }
        else
        {
            Debug.Log("Bu kanıt zaten toplandı.");
        }
    }

    public void DisplayEvidenceDetails(int evidenceId)
    {
        Evidence evidence = CaseData.GetEvidence(evidenceId);
        
        if (evidence == null)
        {
            Debug.LogError("Kanıt bulunamadı.");
            return;
        }

        Debug.Log("=== KANIT DETAYLARI ===");
        Debug.Log("Ad: " + evidence.name);
        Debug.Log("Açıklama: " + evidence.description);
        Debug.Log("Bulunduğu Yer: " + evidence.location);
        Debug.Log("Adli Tıp Detayları: " + evidence.forensicDetails);
        Debug.Log("");
    }

    public void LinkEvidencesToWitness(int evidenceId, int witnessId)
    {
        if (!evidenceConnections.ContainsKey(witnessId))
        {
            evidenceConnections[witnessId] = new List<int>();
        }

        if (!evidenceConnections[witnessId].Contains(evidenceId))
        {
            evidenceConnections[witnessId].Add(evidenceId);
            Debug.Log("Kanıt tanıkla bağlandı: " + evidenceId + " -> Tanık: " + witnessId);
        }
    }

    public List<int> GetEvidenceForWitness(int witnessId)
    {
        if (evidenceConnections.ContainsKey(witnessId))
            return evidenceConnections[witnessId];

        return new List<int>();
    }

    public List<Evidence> GetCollectedEvidences()
    {
        return collectedEvidences;
    }

    public int GetEvidenceCount()
    {
        return collectedEvidences.Count;
    }

    public bool HasEvidence(int evidenceId)
    {
        foreach (Evidence e in collectedEvidences)
        {
            if (e.id == evidenceId)
                return true;
        }
        return false;
    }

    public void DisplayAllEvidences()
    {
        Debug.Log("=== TOPLANAN KANITLAR ===");
        if (collectedEvidences.Count == 0)
        {
            Debug.Log("Henüz kanıt toplanmadı.");
            return;
        }

        foreach (Evidence e in collectedEvidences)
        {
            Debug.Log("• " + e.name + " (" + e.location + ")");
        }
    }
}
