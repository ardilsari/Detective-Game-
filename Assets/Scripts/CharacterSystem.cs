using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData
{
    public int characterId;
    public string characterName;
    public string role; // Suspect, Witness, Police, etc
    public string profileDescription;
    public string appearance; // Fiziksel özellikler
    public int suspicionLevel; // 0-100
    public List<string> alibi;
    public List<string> secrets;
    public List<int> relatedEvidence;
    public string voiceActorName;
    public bool isEncountered;
    public bool isEliminated;
}

[System.Serializable]
public class CharacterModel3D
{
    public string modelName;
    public string prefabPath;
    public CharacterAnimationSet animations;
    public bool isLoaded;
}

[System.Serializable]
public class CharacterAnimationSet
{
    public string characterName;
    public AnimationClip idle;
    public AnimationClip talk;
    public AnimationClip suspicious;
    public AnimationClip angry;
    public AnimationClip sad;
    public AnimationClip thinking;
    public AnimationClip surprised;
    public AnimationClip gesture_point;
    public AnimationClip gesture_deny;
    public AnimationClip gesture_confess;
    public float animationSpeed = 1f;
    public bool isLoaded;
}

[System.Serializable]
public class InteractionState
{
    public int characterId;
    public int conversationIndex;
    public bool hasConfessed;
    public bool isAccused;
    public int tensionLevel; // 0-100 konuşma gerginliği
    public List<string> revealedSecrets;
    public bool isFriendly;
}

public class CharacterSystem : MonoBehaviour
{
    public static CharacterSystem instance;

    private List<CharacterData> characters = new List<CharacterData>();
    private List<CharacterModel3D> characterModels = new List<CharacterModel3D>();
    private Dictionary<int, InteractionState> interactionStates = new Dictionary<int, InteractionState>();
    private Dictionary<int, GameObject> instantiatedCharacters = new Dictionary<int, GameObject>();

    private CharacterData currentInterviewCharacter;
    private int currentConversationLine = 0;

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
        InitializeCharacters();
        InitializeCharacterAnimations();
        Debug.Log("✓ Character System Başlatıldı");
    }

    private void InitializeCharacters()
    {
        // BÖLÜM 1: Jack the Ripper
        characters.Add(new CharacterData
        {
            characterId = 101,
            characterName = "Dr. Montague Druitt",
            role = "Suspect",
            profileDescription = "Tıbbi geçmiş olan ve Whitechapel'de çalışan şüpheli doktor",
            appearance = "Yaşlı, gözlüklü, tıbbi elbiseler giyiyor",
            suspicionLevel = 65,
            alibi = new List<string> { "Tıbbi okulda ders vermişim", "Hastaları ziyaret ettim" },
            secrets = new List<string> { "Tıbbi deneylerden korkuyor", "Gizli not defteri" },
            relatedEvidence = new List<int> { 1001, 1002, 1003 },
            voiceActorName = "Actor_Druitt",
            isEncountered = false,
            isEliminated = false
        });

        characters.Add(new CharacterData
        {
            characterId = 102,
            characterName = "Aaron Kosminski",
            role = "Suspect",
            profileDescription = "Zihinsel hastalıkları olan Polonyalı göçmen",
            appearance = "Genç, temiz olmayan kıyafetler, kaygılı görüş",
            suspicionLevel = 75,
            alibi = new List<string> { "Evimde yalnız hata mı?", "Beni suçlamayın!" },
            secrets = new List<string> { "Ruh hastalığı", "Kravattaki lekedekiler?" },
            relatedEvidence = new List<int> { 1004, 1005, 1006 },
            voiceActorName = "Actor_Kosminski",
            isEncountered = false,
            isEliminated = false
        });

        characters.Add(new CharacterData
        {
            characterId = 103,
            characterName = "Detective Inspector Morse",
            role = "Police",
            profileDescription = "Davaya liderlik eden deneyimli polis müdürü",
            appearance = "Orta yaş, ciddi ifade, polis üniforması",
            suspicionLevel = 0,
            alibi = new List<string> { "Polislerin başında" },
            secrets = new List<string> { "Kasa müsaddiları ezdi", "Kanıtları gizliyor mu?" },
            relatedEvidence = new List<int> { 1007, 1008 },
            voiceActorName = "Actor_Morse",
            isEncountered = true,
            isEliminated = false
        });

        characters.Add(new CharacterData
        {
            characterId = 104,
            characterName = "Mary Ann Nichols (Victim)",
            role = "Victim",
            profileDescription = "İlk kurban. Sokak işçisi olarak yaşamını kazanıyordu.",
            appearance = "Orta yaş, yıpranmış elbiseler",
            suspicionLevel = 0,
            alibi = new List<string> { },
            secrets = new List<string> { },
            relatedEvidence = new List<int> { 1009, 1010 },
            voiceActorName = "",
            isEncountered = false,
            isEliminated = true
        });

        characters.Add(new CharacterData
        {
            characterId = 105,
            characterName = "Catherine Eddowes (Victim)",
            role = "Victim",
            profileDescription = "Dördüncü kurban. Kurtulan bir cinayete tanık.",
            appearance = "Yaşlı, hastalıklı görünüş",
            suspicionLevel = 0,
            alibi = new List<string> { },
            secrets = new List<string> { },
            relatedEvidence = new List<int> { 1011, 1012, 1013 },
            voiceActorName = "",
            isEncountered = false,
            isEliminated = true
        });

        // BÖLÜM 2: Lizzie Borden
        characters.Add(new CharacterData
        {
            characterId = 201,
            characterName = "Lizzie Borden",
            role = "Suspect",
            profileDescription = "Kısa bir keçi başına sahip olan müstakil genç kız",
            appearance = "Genç, oyuncu hali, sakin görünüş",
            suspicionLevel = 80,
            alibi = new List<string> { "Bahçede papağan arıyordum", "Anneme yardımcı oldum" },
            secrets = new List<string> { "Ailevi gerilim", "Baba ile anlaşmazlık", "Gizli ilişki?" },
            relatedEvidence = new List<int> { 2001, 2002, 2003, 2004 },
            voiceActorName = "Actor_Lizzie",
            isEncountered = false,
            isEliminated = false
        });

        characters.Add(new CharacterData
        {
            characterId = 202,
            characterName = "Bridget Sullivan",
            role = "Witness",
            profileDescription = "Borden evinin hizmetçisi. Önemli tanık.",
            appearance = "Genç, hizmetçi kıyafeti",
            suspicionLevel = 45,
            alibi = new List<string> { "Mutfakta çalışıyordum", "Yatak odasında uyku sersemeliğim vardı" },
            secrets = new List<string> { "Lizzie ile gizli anlaşma", "İşimi kaybedip kalacak mı?" },
            relatedEvidence = new List<int> { 2005, 2006 },
            voiceActorName = "Actor_Bridget",
            isEncountered = false,
            isEliminated = false
        });

        // BÖLÜM 3: Black Dahlia
        characters.Add(new CharacterData
        {
            characterId = 301,
            characterName = "Elizabeth Short",
            role = "Victim",
            profileDescription = "Hollywood'un akîka kızı. Gizli hayatı var mı?",
            appearance = "Güzel, siyah elbiseler, Hollywood tarzı",
            suspicionLevel = 0,
            alibi = new List<string> { },
            secrets = new List<string> { },
            relatedEvidence = new List<int> { 3001, 3002, 3003, 3004 },
            voiceActorName = "",
            isEncountered = false,
            isEliminated = true
        });

        characters.Add(new CharacterData
        {
            characterId = 302,
            characterName = "Dr. George Hodel",
            role = "Suspect",
            profileDescription = "Zengin doktor. Hollywood üst tabakasında yaşlı.",
            appearance = "Orta yaş, sofistike, ısmarlama takımlar",
            suspicionLevel = 70,
            alibi = new List<string> { "Kulübde vardım", "Arkadaşlarla yemekteyiz" },
            secrets = new List<string> { "Gizli ilişkiler", "Tıbbi deney", "Suçu saklama" },
            relatedEvidence = new List<int> { 3005, 3006, 3007 },
            voiceActorName = "Actor_Hodel",
            isEncountered = false,
            isEliminated = false
        });

        Debug.Log("✓ " + characters.Count + " Karakter Yüklendi");
    }

    private void InitializeCharacterAnimations()
    {
        // Dr. Druitt Animasyonları
        characterModels.Add(new CharacterModel3D
        {
            modelName = "Dr_Druitt_Model",
            prefabPath = "Prefabs/Characters/Druitt",
            animations = new CharacterAnimationSet
            {
                characterName = "Dr. Druitt",
                idle = null, // TODO: AnimationClip atanacak
                talk = null,
                suspicious = null,
                angry = null,
                sad = null,
                thinking = null,
                surprised = null,
                gesture_point = null,
                gesture_deny = null,
                gesture_confess = null,
                animationSpeed = 1f,
                isLoaded = false
            }
        });

        // Lizzie Borden Animasyonları
        characterModels.Add(new CharacterModel3D
        {
            modelName = "Lizzie_Borden_Model",
            prefabPath = "Prefabs/Characters/Lizzie",
            animations = new CharacterAnimationSet
            {
                characterName = "Lizzie Borden",
                idle = null,
                talk = null,
                suspicious = null,
                angry = null,
                sad = null,
                thinking = null,
                surprised = null,
                gesture_point = null,
                gesture_deny = null,
                gesture_confess = null,
                animationSpeed = 1f,
                isLoaded = false
            }
        });

        Debug.Log("✓ " + characterModels.Count + " Karakter Modeli Hazırlandı");
    }

    public void SpawnCharacter(int characterId, Vector3 position)
    {
        CharacterData character = GetCharacterById(characterId);
        if (character == null)
        {
            Debug.LogError("❌ Karakter bulunamadı: " + characterId);
            return;
        }

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🎭 KARAKTER GÖRÜNTÜLENDI!");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Ad: " + character.characterName);
        Debug.Log("Rol: " + character.role);
        Debug.Log("Şüphe Oranı: " + character.suspicionLevel + "%");
        Debug.Log("═══════════════════════════════════════\n");

        // TODO: 3D model instantiate et
        // GameObject characterObj = Instantiate(prefab, position, Quaternion.identity);
        // instantiatedCharacters[characterId] = characterObj;

        character.isEncountered = true;
    }

    public void InterviewCharacter(int characterId)
    {
        CharacterData character = GetCharacterById(characterId);
        if (character == null)
        {
            Debug.LogError("❌ Karakter bulunamadı: " + characterId);
            return;
        }

        currentInterviewCharacter = character;
        currentConversationLine = 0;

        if (!interactionStates.ContainsKey(characterId))
        {
            interactionStates[characterId] = new InteractionState
            {
                characterId = characterId,
                conversationIndex = 0,
                hasConfessed = false,
                isAccused = false,
                tensionLevel = 30,
                revealedSecrets = new List<string>(),
                isFriendly = false
            };
        }

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🎤 SORGULAMA BAŞLADI!");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Şüpheli: " + character.characterName);
        Debug.Log("Alibi: " + string.Join(", ", character.alibi));
        Debug.Log("═══════════════════════════════════════\n");
    }

    public void AskQuestion(string questionText)
    {
        if (currentInterviewCharacter == null)
        {
            Debug.LogWarning("⚠️ Hiçbir karaktere sorulmadı");
            return;
        }

        InteractionState state = interactionStates[currentInterviewCharacter.characterId];
        state.tensionLevel = Mathf.Clamp(state.tensionLevel + Random.Range(5, 20), 0, 100);

        Debug.Log("\n🎤 Sorgulayan: \"" + questionText + "\"");
        Debug.Log(currentInterviewCharacter.characterName + ": [Gerilim: " + state.tensionLevel + "%]");

        // TODO: Karakterin cevabını AI ile belirle
        if (state.tensionLevel > 80)
        {
            Debug.Log("⚠️ Karakter çok sinirli! Itiraf edebilir...");
        }
        else if (state.tensionLevel > 60)
        {
            Debug.Log("💭 Karakter düşünüyor...");
        }
    }

    public void AccuseCharacter(int characterId)
    {
        CharacterData character = GetCharacterById(characterId);
        if (character == null)
        {
            Debug.LogError("❌ Karakter bulunamadı: " + characterId);
            return;
        }

        InteractionState state = interactionStates[characterId];
        state.isAccused = true;

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🚨 KARAKTER SUÇLANDILAR!");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Suçlanan: " + character.characterName);
        Debug.Log("Rol: " + character.role);
        Debug.Log("Şüphe Oranı: " + character.suspicionLevel + "%");
        Debug.Log("═══════════════════════════════════════\n");

        // TODO: Sonuç belirle (doğru/yanlış)
    }

    public void EliminateCharacter(int characterId)
    {
        CharacterData character = GetCharacterById(characterId);
        if (character != null)
        {
            character.isEliminated = true;
            Debug.Log("✗ " + character.characterName + " şüphelilerden çıkarıldı");
        }
    }

    public void PlayCharacterAnimation(int characterId, string animationType)
    {
        CharacterModel3D model = GetCharacterModel(characterId);
        if (model == null)
        {
            Debug.LogWarning("⚠️ Karakter modeli bulunamadı: " + characterId);
            return;
        }

        Debug.Log("🎬 Animasyon: " + model.characterName + " → " + animationType);

        // TODO: Animator ile animasyonu çalıştır
        // animator.SetTrigger(animationType);
    }

    public void RevealSecret(int characterId, string secret)
    {
        if (!interactionStates.ContainsKey(characterId))
            return;

        InteractionState state = interactionStates[characterId];
        if (!state.revealedSecrets.Contains(secret))
        {
            state.revealedSecrets.Add(secret);
            Debug.Log("🔐 Gizli Bilgi Açıldı: " + secret);
        }
    }

    public CharacterData GetCharacterById(int characterId)
    {
        foreach (CharacterData character in characters)
        {
            if (character.characterId == characterId)
                return character;
        }
        return null;
    }

    public CharacterModel3D GetCharacterModel(int characterId)
    {
        CharacterData character = GetCharacterById(characterId);
        if (character == null)
            return null;

        foreach (CharacterModel3D model in characterModels)
        {
            if (model.modelName.Contains(character.characterName.Replace(" ", "")))
                return model;
        }
        return null;
    }

    public List<CharacterData> GetAllCharacters() => characters;
    public List<CharacterData> GetSuspects()
    {
        List<CharacterData> suspects = new List<CharacterData>();
        foreach (CharacterData character in characters)
        {
            if (character.role == "Suspect" && !character.isEliminated)
                suspects.Add(character);
        }
        return suspects;
    }

    public List<CharacterData> GetWitnesses()
    {
        List<CharacterData> witnesses = new List<CharacterData>();
        foreach (CharacterData character in characters)
        {
            if (character.role == "Witness" && character.isEncountered)
                witnesses.Add(character);
        }
        return witnesses;
    }

    public InteractionState GetInteractionState(int characterId)
    {
        if (interactionStates.ContainsKey(characterId))
            return interactionStates[characterId];
        return null;
    }
}
