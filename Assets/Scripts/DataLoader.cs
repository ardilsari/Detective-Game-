using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CaseDataWrapper
{
    public List<Case> cases;
}

public class DataLoader : MonoBehaviour
{
    public static Case LoadCaseFromJSON(string jsonPath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        
        if (jsonFile == null)
        {
            Debug.LogError("JSON dosyası bulunamadı: " + jsonPath);
            return null;
        }

        CaseDataWrapper wrapper = JsonUtility.FromJson<CaseDataWrapper>(jsonFile.text);
        
        if (wrapper == null || wrapper.cases == null || wrapper.cases.Count == 0)
        {
            Debug.LogError("JSON verisi yüklenemedi.");
            return null;
        }

        Debug.Log("Olay başarılı bir şekilde yüklendi: " + wrapper.cases[0].caseName);
        return wrapper.cases[0];
    }

    public static void SaveCaseToJSON(Case caseData, string filePath)
    {
        CaseDataWrapper wrapper = new CaseDataWrapper();
        wrapper.cases = new List<Case> { caseData };

        string json = JsonUtility.ToJson(wrapper, true);
        System.IO.File.WriteAllText(filePath, json);
        
        Debug.Log("Olay kaydedildi: " + filePath);
    }
}
