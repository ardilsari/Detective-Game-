using System.Collections.Generic;
using UnityEngine;

public class WitnessSystem : MonoBehaviour
{
    private List<Witness> interrogatedWitnesses = new List<Witness>();
    private Witness currentWitness;
    private int currentStatementIndex = 0;

    public void InterrogateWitness(int witnessId)
    {
        currentWitness = CaseData.GetWitness(witnessId);
        
        if (currentWitness == null)
        {
            Debug.LogError("Tanık bulunamadı: " + witnessId);
            return;
        }

        if (!interrogatedWitnesses.Contains(currentWitness))
        {
            interrogatedWitnesses.Add(currentWitness);
        }

        currentStatementIndex = 0;
        DisplayWitnessInfo();
    }

    public void DisplayWitnessInfo()
    {
        if (currentWitness == null) return;

        Debug.Log("=== TANIK BİLGİLERİ ===");
        Debug.Log("Ad: " + currentWitness.name);
        Debug.Log("Meslek: " + currentWitness.profession);
        Debug.Log("Bulunduğu Yer: " + currentWitness.location);
        Debug.Log("Şüpheli: " + (currentWitness.isSuspect ? "EVET" : "HAYIR"));
        Debug.Log("");
    }

    public void GetWitnessStatement()
    {
        if (currentWitness == null || currentWitness.statements.Count == 0)
        {
            Debug.Log("Tanıktan ifade alınamadı");
            return;
        }

        if (currentStatementIndex < currentWitness.statements.Count)
        {
            Debug.Log("İFADE: " + currentWitness.statements[currentStatementIndex]);
            currentStatementIndex++;
        }
        else
        {
            Debug.Log("Tanık başka bilgi vermiyor.");
        }
    }

    public List<string> GetWitnessClues()
    {
        if (currentWitness == null)
            return new List<string>();

        return currentWitness.clues;
    }

    public List<Witness> GetAllWitnesses()
    {
        return CaseData.GetCase().witnesses;
    }

    public List<Witness> GetInterrogatedWitnesses()
    {
        return interrogatedWitnesses;
    }

    public bool IsWitnessInterrogated(int witnessId)
    {
        foreach (Witness w in interrogatedWitnesses)
        {
            if (w.id == witnessId)
                return true;
        }
        return false;
    }
}
