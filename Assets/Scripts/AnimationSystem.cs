using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AnimationState
{
    public string stateName;
    public AnimationClip clip;
    public float duration;
    public bool canInterrupt;
    public float transitionDuration = 0.25f;
}

[System.Serializable]
public class AnimationTransition
{
    public string fromState;
    public string toState;
    public string triggerCondition;
    public float duration = 0.3f;
}

[System.Serializable]
public class CharacterAnimator
{
    public int characterId;
    public string characterName;
    public Animator animator;
    public List<AnimationState> animationStates = new List<AnimationState>();
    public string currentState = "Idle";
    public float currentSpeed = 1f;
    public bool isAnimating;
}

[System.Serializable]
public class AnimationEvent
{
    public string eventName;
    public float triggerTime; // Animasyonun kaçıncı saniyesinde tetiklenir
    public System.Action<int> callback; // characterId ile birlikte
}

public class AnimationSystem : MonoBehaviour
{
    public static AnimationSystem instance;

    private Dictionary<int, CharacterAnimator> characterAnimators = new Dictionary<int, CharacterAnimator>();
    private Dictionary<string, List<AnimationState>> animationPresets = new Dictionary<string, List<AnimationState>>();
    private Dictionary<string, AnimationEvent> animationEvents = new Dictionary<string, AnimationEvent>();

    // Animation Blend Parameters
    private float emotionIntensity = 0.5f; // 0-1
    private float stressLevel = 0f; // 0-1
    private float suspicionIndicator = 0f; // 0-1

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
        InitializeAnimationPresets();
        InitializeAnimationTransitions();
        Debug.Log("✓ Animation System Başlatıldı");
    }

    private void InitializeAnimationPresets()
    {
        // İDLE ANIMASYONLAR
        List<AnimationState> idleAnimations = new List<AnimationState>
        {
            new AnimationState
            {
                stateName = "Idle_Calm",
                duration = 2f,
                canInterrupt = true,
                transitionDuration = 0.5f
            },
            new AnimationState
            {
                stateName = "Idle_Nervous",
                duration = 2f,
                canInterrupt = true,
                transitionDuration = 0.3f
            },
            new AnimationState
            {
                stateName = "Idle_Suspicious",
                duration = 2f,
                canInterrupt = true,
                transitionDuration = 0.2f
            }
        };
        animationPresets.Add("Idle", idleAnimations);

        // KONUŞMA ANIMASYONLARI
        List<AnimationState> talkAnimations = new List<AnimationState>
        {
            new AnimationState
            {
                stateName = "Talk_Normal",
                duration = 3f,
                canInterrupt = true,
                transitionDuration = 0.2f
            },
            new AnimationState
            {
                stateName = "Talk_Excited",
                duration = 2.5f,
                canInterrupt = true,
                transitionDuration = 0.15f
            },
            new AnimationState
            {
                stateName = "Talk_Defensive",
                duration = 3.5f,
                canInterrupt = true,
                transitionDuration = 0.25f
            },
            new AnimationState
            {
                stateName = "Talk_Angry",
                duration = 2f,
                canInterrupt = false, // Kesmek zor
                transitionDuration = 0.1f
            }
        };
        animationPresets.Add("Talk", talkAnimations);

        // TEDİRGİN ANIMASYONLARI
        List<AnimationState> suspiciousAnimations = new List<AnimationState>
        {
            new AnimationState
            {
                stateName = "Suspicious_Fidget",
                duration = 2f,
                canInterrupt = true,
                transitionDuration = 0.15f
            },
            new AnimationState
            {
                stateName = "Suspicious_Sweat",
                duration = 2.5f,
                canInterrupt = true,
                transitionDuration = 0.1f
            },
            new AnimationState
            {
                stateName = "Suspicious_Avoid_Eye",
                duration = 3f,
                canInterrupt = true,
                transitionDuration = 0.2f
            }
        };
        animationPresets.Add("Suspicious", suspiciousAnimations);

        // DUYGUSAL ANIMASYONLAR
        List<AnimationState> emotionAnimations = new List<AnimationState>
        {
            new AnimationState
            {
                stateName = "Emotion_Sad",
                duration = 3f,
                canInterrupt = true,
                transitionDuration = 0.5f
            },
            new AnimationState
            {
                stateName = "Emotion_Angry",
                duration = 2f,
                canInterrupt = false,
                transitionDuration = 0.1f
            },
            new AnimationState
            {
                stateName = "Emotion_Surprised",
                duration = 1.5f,
                canInterrupt = true,
                transitionDuration = 0.1f
            },
            new AnimationState
            {
                stateName = "Emotion_Scared",
                duration = 2.5f,
                canInterrupt = true,
                transitionDuration = 0.2f
            },
            new AnimationState
            {
                stateName = "Emotion_Confused",
                duration = 2.5f,
                canInterrupt = true,
                transitionDuration = 0.3f
            }
        };
        animationPresets.Add("Emotion", emotionAnimations);

        // HAREKET ANIMASYONLARI
        List<AnimationState> gestureAnimations = new List<AnimationState>
        {
            new AnimationState
            {
                stateName = "Gesture_Point",
                duration = 1.5f,
                canInterrupt = true,
                transitionDuration = 0.2f
            },
            new AnimationState
            {
                stateName = "Gesture_Deny",
                duration = 2f,
                canInterrupt = true,
                transitionDuration = 0.2f
            },
            new AnimationState
            {
                stateName = "Gesture_Confess",
                duration = 3f,
                canInterrupt = false,
                transitionDuration = 0.5f
            },
            new AnimationState
            {
                stateName = "Gesture_Plead",
                duration = 2.5f,
                canInterrupt = true,
                transitionDuration = 0.3f
            },
            new AnimationState
            {
                stateName = "Gesture_Shrug",
                duration = 1.5f,
                canInterrupt = true,
                transitionDuration = 0.15f
            }
        };
        animationPresets.Add("Gesture", gestureAnimations);

        // İTİRAF ANIMASYONLARI
        List<AnimationState> confessAnimations = new List<AnimationState>
        {
            new AnimationState
            {
                stateName = "Confess_Breakdown",
                duration = 4f,
                canInterrupt = false,
                transitionDuration = 0.5f
            },
            new AnimationState
            {
                stateName = "Confess_Reluctant",
                duration = 3.5f,
                canInterrupt = false,
                transitionDuration = 0.4f
            },
            new AnimationState
            {
                stateName = "Confess_Relief",
                duration = 3f,
                canInterrupt = false,
                transitionDuration = 0.3f
            }
        };
        animationPresets.Add("Confess", confessAnimations);

        Debug.Log("✓ " + animationPresets.Count + " Animation Preset Kategori Yüklendi");
    }

    private void InitializeAnimationTransitions()
    {
        // Geçişler daha sonra oyun sırasında otomatik şekilde oluşturulur
        // Blend States kullanarak smooth transitions sağlanır
        Debug.Log("✓ Animation Transitions Hazırlandı");
    }

    public void RegisterCharacterAnimator(int characterId, string characterName, Animator animator)
    {
        if (characterAnimators.ContainsKey(characterId))
        {
            Debug.LogWarning("⚠️ Animator zaten tescillenmiş: " + characterId);
            return;
        }

        CharacterAnimator charAnimator = new CharacterAnimator
        {
            characterId = characterId,
            characterName = characterName,
            animator = animator,
            currentState = "Idle",
            currentSpeed = 1f,
            isAnimating = false
        };

        characterAnimators[characterId] = charAnimator;
        Debug.Log("✓ Animator Tescillenmiş: " + characterName);
    }

    public void PlayAnimation(int characterId, string animationType, string specificState = null)
    {
        if (!characterAnimators.ContainsKey(characterId))
        {
            Debug.LogWarning("⚠️ Animator bulunamadı: " + characterId);
            return;
        }

        CharacterAnimator charAnimator = characterAnimators[characterId];
        
        // Animasyon türüne göre uygun state seç
        if (!animationPresets.ContainsKey(animationType))
        {
            Debug.LogWarning("⚠️ Animasyon tipi bulunamadı: " + animationType);
            return;
        }

        List<AnimationState> states = animationPresets[animationType];
        AnimationState targetState = null;

        if (specificState != null)
        {
            targetState = states.Find(s => s.stateName == specificState);
        }
        else
        {
            targetState = states[Random.Range(0, states.Count)];
        }

        if (targetState == null)
        {
            Debug.LogWarning("⚠️ State bulunamadı: " + specificState);
            return;
        }

        // Geçiş kontrol et
        if (!charAnimator.isAnimating || (charAnimator.isAnimating && targetState.canInterrupt))
        {
            charAnimator.animator.CrossFadeInFixedTime(targetState.stateName, targetState.transitionDuration);
            charAnimator.currentState = targetState.stateName;
            charAnimator.isAnimating = true;

            // Animasyon bitince tetiklenecek event
            StartCoroutine(AnimationCoroutine(characterId, targetState.duration));

            Debug.Log("🎬 " + charAnimator.characterName + " → " + targetState.stateName);
        }
        else
        {
            Debug.Log("⏸️ " + charAnimator.characterName + " animasyonu kesintiye uğramıyor");
        }
    }

    private System.Collections.IEnumerator AnimationCoroutine(int characterId, float duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (characterAnimators.ContainsKey(characterId))
        {
            characterAnimators[characterId].isAnimating = false;
        }
    }

    public void BlendAnimations(int characterId, float emotionBlend, float stressBlend)
    {
        if (!characterAnimators.ContainsKey(characterId))
            return;

        CharacterAnimator charAnimator = characterAnimators[characterId];
        
        // Animator parametrelerini ayarla
        charAnimator.animator.SetFloat("Emotion", Mathf.Clamp01(emotionBlend));
        charAnimator.animator.SetFloat("Stress", Mathf.Clamp01(stressBlend));
        charAnimator.animator.SetFloat("Suspicion", Mathf.Clamp01(suspicionIndicator));

        Debug.Log("📊 Blend Parameters: Emotion=" + emotionBlend + ", Stress=" + stressBlend);
    }

    public void PlayInterrogationSequence(int characterId, string questionSeverity)
    {
        // questionSeverity: Mild, Normal, Harsh, Accusation

        if (!characterAnimators.ContainsKey(characterId))
            return;

        CharacterAnimator charAnimator = characterAnimators[characterId];

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🎬 SORGULAMA SEKENSİ");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Şüpheli: " + charAnimator.characterName);
        Debug.Log("Soru Ciddiyeti: " + questionSeverity);

        switch (questionSeverity)
        {
            case "Mild":
                PlayAnimation(characterId, "Idle", "Idle_Calm");
                StartCoroutine(InterrogationSequenceCoroutine(characterId, new List<string> 
                { 
                    "Talk_Normal", "Idle_Calm", "Gesture_Shrug" 
                }));
                break;

            case "Normal":
                PlayAnimation(characterId, "Talk", "Talk_Normal");
                StartCoroutine(InterrogationSequenceCoroutine(characterId, new List<string>
                {
                    "Talk_Normal", "Suspicious_Fidget", "Gesture_Deny"
                }));
                break;

            case "Harsh":
                PlayAnimation(characterId, "Talk", "Talk_Defensive");
                StartCoroutine(InterrogationSequenceCoroutine(characterId, new List<string>
                {
                    "Talk_Defensive", "Emotion_Angry", "Suspicious_Avoid_Eye"
                }));
                break;

            case "Accusation":
                PlayAnimation(characterId, "Emotion", "Emotion_Surprised");
                StartCoroutine(InterrogationSequenceCoroutine(characterId, new List<string>
                {
                    "Emotion_Surprised", "Talk_Angry", "Emotion_Scared"
                }));
                break;
        }

        Debug.Log("═══════════════════════════════════════\n");
    }

    private System.Collections.IEnumerator InterrogationSequenceCoroutine(int characterId, List<string> sequence)
    {
        foreach (string animState in sequence)
        {
            string animType = animState.Split('_')[0]; // "Talk" or "Emotion"
            PlayAnimation(characterId, animType, animState);
            yield return new WaitForSeconds(2f);
        }
    }

    public void PlayEmotionalReaction(int characterId, string emotion)
    {
        // emotion: Sad, Angry, Surprised, Scared, Confused, Happy, Relieved

        if (!characterAnimators.ContainsKey(characterId))
            return;

        string emotionState = "Emotion_" + emotion;
        PlayAnimation(characterId, "Emotion", emotionState);

        Debug.Log("😢 Duygusal Tepki: " + emotion);
    }

    public void PlayConfessionAnimation(int characterId, string confessionType)
    {
        // confessionType: Breakdown, Reluctant, Relief

        if (!characterAnimators.ContainsKey(characterId))
            return;

        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🔓 İTİRAF ANİMASYONU");
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("Şüpheli: " + characterAnimators[characterId].characterName);
        Debug.Log("İtiraf Tipi: " + confessionType);
        Debug.Log("═══════════════════════════════════════\n");

        string confessionState = "Confess_" + confessionType;
        PlayAnimation(characterId, "Confess", confessionState);
    }

    public void SetAnimationSpeed(int characterId, float speed)
    {
        if (!characterAnimators.ContainsKey(characterId))
            return;

        CharacterAnimator charAnimator = characterAnimators[characterId];
        charAnimator.animator.speed = speed;
        charAnimator.currentSpeed = speed;

        Debug.Log("⏱️ Animasyon Hızı: " + speed + "x");
    }

    public void StopAnimation(int characterId)
    {
        if (!characterAnimators.ContainsKey(characterId))
            return;

        CharacterAnimator charAnimator = characterAnimators[characterId];
        charAnimator.animator.speed = 0f;
        charAnimator.isAnimating = false;

        Debug.Log("⏸️ Animasyon Durduruldu");
    }

    public void ResumeAnimation(int characterId)
    {
        if (!characterAnimators.ContainsKey(characterId))
            return;

        CharacterAnimator charAnimator = characterAnimators[characterId];
        charAnimator.animator.speed = charAnimator.currentSpeed;

        Debug.Log("▶️ Animasyon Devam Ettirildi");
    }

    public string GetCurrentAnimationState(int characterId)
    {
        if (characterAnimators.ContainsKey(characterId))
            return characterAnimators[characterId].currentState;
        return "Unknown";
    }

    public bool IsCharacterAnimating(int characterId)
    {
        if (characterAnimators.ContainsKey(characterId))
            return characterAnimators[characterId].isAnimating;
        return false;
    }

    public Dictionary<int, CharacterAnimator> GetAllCharacterAnimators() => characterAnimators;
}
