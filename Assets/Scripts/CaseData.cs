using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Witness
{
    public int id;
    public string name;
    public string photo; // Base64 veya dosya yolu
    public string profession;
    public string location; // Nerede bulundu
    public List<string> statements; // Verdiği ifadeler
    public List<string> clues; // Verdiği ipuçları
    public bool isSuspect;
}

[System.Serializable]
public class Evidence
{
    public int id;
    public string name;
    public string description;
    public string location; // Bulunduğu yer
    public Witness relatedWitness; // İlgili tanık
    public string forensicDetails;
}

[System.Serializable]
public class CriminalProfile
{
    public int id;
    public string name;
    public string description;
    public List<int> evidenceIds; // Suçluyu gösteren kanıtlar
    public List<int> witnessIds; // İlgili tanıklar
}

[System.Serializable]
public class Case
{
    public int caseId;
    public string caseName;
    public string caseDescription;
    public string date; // Suçun tarihi
    public string location; // Suçun yeri
    public List<Witness> witnesses;
    public List<Evidence> evidences;
    public List<CriminalProfile> suspectProfiles;
    public int correctSuspectId;
    public int daysToSolve;
}

public class CaseData : MonoBehaviour
{
    public static Case currentCase;

    public static void InitializeCase(Case newCase)
    {
        currentCase = newCase;
    }

    public static Case GetCase()
    {
        return currentCase;
    }

    public static Witness GetWitness(int witnessId)
    {
        foreach (Witness w in currentCase.witnesses)
        {
            if (w.id == witnessId)
                return w;
        }
        return null;
    }

    public static Evidence GetEvidence(int evidenceId)
    {
        foreach (Evidence e in currentCase.evidences)
        {
            if (e.id == evidenceId)
                return e;
        }
        return null;
    }
}
