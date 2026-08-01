using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AudioClipData
{
    public string clipName;
    public AudioClip audioClip;
    public float volume;
    public bool loop;
    public AudioType audioType; // BGM, SFX, Voice, Ambient
}

[System.Serializable]
public class VoiceLine
{
    public string characterName;
    public string dialogueText;
    public AudioClip voiceClip;
    public float duration;
    public bool isSeen;
}

public enum AudioType { BGM, SFX, Voice, Ambient }

[System.Serializable]
public class MusicTheme
{
    public string themeName;
    public AudioClip introClip;
    public AudioClip loopClip;
    public float intensity; // 0-1
    public string mood; // Mysterious, Tense, Calm, etc
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource bgmSource;
    private AudioSource sfxSource;
    private AudioSource voiceSource;
    private AudioSource ambientSource;

    private Dictionary<string, AudioClipData> audioClips = new Dictionary<string, AudioClipData>();
    private Dictionary<string, MusicTheme> musicThemes = new Dictionary<string, MusicTheme>();
    private Dictionary<string, List<VoiceLine>> characterDialogues = new Dictionary<string, List<VoiceLine>>();

    private float masterVolume = 1f;
    private float bgmVolume = 0.7f;
    private float sfxVolume = 0.8f;
    private float voiceVolume = 0.9f;
    private float ambientVolume = 0.5f;

    private string currentBGM = "";
    private bool isMuted = false;

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
        InitializeAudioSources();
        InitializeAudioClips();
        InitializeMusicThemes();
        InitializeCharacterDialogues();
        
        Debug.Log("✓ Audio Manager Hazırlandı");
    }

    private void InitializeAudioSources()
    {
        // BGM Audio Source
        GameObject bgmObj = new GameObject("BGM_Source");
        bgmObj.transform.SetParent(transform);
        bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmSource.volume = bgmVolume * masterVolume;
        bgmSource.loop = true;
        bgmSource.priority = 0;

        // SFX Audio Source
        GameObject sfxObj = new GameObject("SFX_Source");
        sfxObj.transform.SetParent(transform);
        sfxSource = sfxObj.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume * masterVolume;
        sfxSource.loop = false;
        sfxSource.priority = 50;

        // Voice Audio Source
        GameObject voiceObj = new GameObject("Voice_Source");
        voiceObj.transform.SetParent(transform);
        voiceSource = voiceObj.AddComponent<AudioSource>();
        voiceSource.volume = voiceVolume * masterVolume;
        voiceSource.loop = false;
        voiceSource.priority = 25;

        // Ambient Audio Source
        GameObject ambientObj = new GameObject("Ambient_Source");
        ambientObj.transform.SetParent(transform);
        ambientSource = ambientObj.AddComponent<AudioSource>();
        ambientSource.volume = ambientVolume * masterVolume;
        ambientSource.loop = true;
        ambientSource.priority = 75;

        Debug.Log("✓ 4 Audio Source Oluşturuldu");
    }

    private void InitializeAudioClips()
    {
        // VICTORIAN ERA SOUNDS
        audioClips.Add("sfx_footsteps_cobblestone", new AudioClipData
        {
            clipName = "Cobblestone Footsteps",
            volume = 0.7f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("sfx_door_knock", new AudioClipData
        {
            clipName = "Door Knock",
            volume = 0.8f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("sfx_evidence_pickup", new AudioClipData
        {
            clipName = "Evidence Pickup",
            volume = 0.6f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("sfx_achievement_unlock", new AudioClipData
        {
            clipName = "Achievement Unlock",
            volume = 0.9f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("sfx_decision_made", new AudioClipData
        {
            clipName = "Decision Made",
            volume = 0.7f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("sfx_mystery_solved", new AudioClipData
        {
            clipName = "Mystery Solved",
            volume = 1f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("sfx_notification_ping", new AudioClipData
        {
            clipName = "Notification Ping",
            volume = 0.5f,
            loop = false,
            audioType = AudioType.SFX
        });

        audioClips.Add("ambient_rain", new AudioClipData
        {
            clipName = "Rain Ambience",
            volume = 0.4f,
            loop = true,
            audioType = AudioType.Ambient
        });

        audioClips.Add("ambient_wind", new AudioClipData
        {
            clipName = "Wind Ambience",
            volume = 0.3f,
            loop = true,
            audioType = AudioType.Ambient
        });

        audioClips.Add("ambient_clock_ticking", new AudioClipData
        {
            clipName = "Clock Ticking",
            volume = 0.5f,
            loop = true,
            audioType = AudioType.Ambient
        });

        Debug.Log("✓ " + audioClips.Count + " Ses Efekti Yüklendi");
    }

    private void InitializeMusicThemes()
    {
        // CHAPTER 1: Jack the Ripper
        musicThemes.Add("Music_Victorian_London", new MusicTheme
        {
            themeName = "Victorian London Theme",
            intensity = 0.8f,
            mood = "Mysterious",
            introClip = null, // TODO: AudioClip ata
            loopClip = null
        });

        // CHAPTER 2: Lizzie Borden
        musicThemes.Add("Music_Victorian_America", new MusicTheme
        {
            themeName = "Victorian America Theme",
            intensity = 0.7f,
            mood = "Suspenseful",
            introClip = null,
            loopClip = null
        });

        // CHAPTER 3: Black Dahlia
        musicThemes.Add("Music_1940s_Jazz", new MusicTheme
        {
            themeName = "1940s Hollywood Jazz",
            intensity = 0.6f,
            mood = "Dark Jazz",
            introClip = null,
            loopClip = null
        });

        // CHAPTER 4: Axeman
        musicThemes.Add("Music_Jazz_Blues_1918", new MusicTheme
        {
            themeName = "Jazz Blues 1918",
            intensity = 0.75f,
            mood = "Tense",
            introClip = null,
            loopClip = null
        });

        // CHAPTER 5: Hinterkaifeck
        musicThemes.Add("Music_Dark_Bavarian", new MusicTheme
        {
            themeName = "Dark Bavarian Folk",
            intensity = 0.9f,
            mood = "Ominous",
            introClip = null,
            loopClip = null
        });

        // CHAPTER 6: Villisca
        musicThemes.Add("Music_Grim_Americana", new MusicTheme
        {
            themeName = "Grim Americana",
            intensity = 0.85f,
            mood = "Grim",
            introClip = null,
            loopClip = null
        });

        // CHAPTER 7: Finale
        musicThemes.Add("Music_Epic_Finale", new MusicTheme
        {
            themeName = "Epic Finale",
            intensity = 1f,
            mood = "Epic",
            introClip = null,
            loopClip = null
        });

        // MENU
        musicThemes.Add("Music_Main_Menu", new MusicTheme
        {
            themeName = "Main Menu Theme",
            intensity = 0.5f,
            mood = "Calm",
            introClip = null,
            loopClip = null
        });

        Debug.Log("✓ " + musicThemes.Count + " Müzik Tema Yüklendi");
    }

    private void InitializeCharacterDialogues()
    {
        // CHAPTER 1 DIALOGUES
        List<VoiceLine> druittDialogues = new List<VoiceLine>
        {
            new VoiceLine
            {
                characterName = "Dr. Druitt",
                dialogueText = "Whitechapel'de neler olduğunu sormaktan korkuyorum.",
                duration = 4.5f
            },
            new VoiceLine
            {
                characterName = "Dr. Druitt",
                dialogueText = "Tıp fakültesi... Evet, orada çalışıyordum.",
                duration = 3.8f
            }
        };
        characterDialogues.Add("Dr.Druitt", druittDialogues);

        List<VoiceLine> kosminskilDialogues = new List<VoiceLine>
        {
            new VoiceLine
            {
                characterName = "Aaron Kosminski",
                dialogueText = "Londra... Bu şehir çok kötü, çok kötü...",
                duration = 3.2f
            },
            new VoiceLine
            {
                characterName = "Aaron Kosminski",
                dialogueText = "Beni sorgulama! Benim bir suçum yok!",
                duration = 4.0f
            }
        };
        characterDialogues.Add("Aaron_Kosminski", kosminskilDialogues);

        // CHAPTER 2 DIALOGUES
        List<VoiceLine> lizzieDialogues = new List<VoiceLine>
        {
            new VoiceLine
            {
                characterName = "Lizzie Borden",
                dialogueText = "Babam çok sert bir adamdı. Ona söyleyecek çok şeyim vardı.",
                duration = 5.2f
            },
            new VoiceLine
            {
                characterName = "Lizzie Borden",
                dialogueText = "Ama ben bunu yapmadım! İnanmanız gerekiyor!",
                duration = 4.1f
            }
        };
        characterDialogues.Add("Lizzie_Borden", lizzieDialogues);

        // CHAPTER 3 DIALOGUES
        List<VoiceLine> elizabethDialogues = new List<VoiceLine>
        {
            new VoiceLine
            {
                characterName = "Elizabeth Short",
                dialogueText = "Hollywood hayali bir yerdir. Herkes yalan söyler.",
                duration = 4.0f
            },
            new VoiceLine
            {
                characterName = "Elizabeth Short",
                dialogueText = "Benim kimler tanıyabileceğini bilemezsiniz.",
                duration = 4.3f
            }
        };
        characterDialogues.Add("Elizabeth_Short", elizabethDialogues);

        Debug.Log("✓ Karakter Diyalogları Yüklendi");
    }

    // MUSIC CONTROL
    public void PlayBackgroundMusic(string themeName)
    {
        if (currentBGM == themeName)
            return; // Zaten çalınıyor

        if (!musicThemes.ContainsKey(themeName))
        {
            Debug.LogWarning("⚠️ Müzik tema bulunamadı: " + themeName);
            return;
        }

        MusicTheme theme = musicThemes[themeName];
        
        // Fade out mevcut müzik
        if (bgmSource.isPlaying)
        {
            StartCoroutine(FadeOutBGM());
        }

        // Yeni müzik başlat
        if (theme.loopClip != null)
        {
            bgmSource.clip = theme.loopClip;
            bgmSource.Play();
            StartCoroutine(FadeInBGM());
            currentBGM = themeName;

            Debug.Log("🎵 Müzik Çalınıyor: " + theme.themeName);
            Debug.Log("Mod: " + theme.mood + " | Yoğunluk: " + theme.intensity);
        }
    }

    public void StopBackgroundMusic()
    {
        StartCoroutine(FadeOutBGM());
        currentBGM = "";
    }

    private System.Collections.IEnumerator FadeInBGM()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        float targetVolume = bgmVolume * masterVolume;

        bgmSource.volume = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVolume, elapsed / duration);
            yield return null;
        }

        bgmSource.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeOutBGM()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        float startVolume = bgmSource.volume;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }

    // SFX CONTROL
    public void PlaySFX(string sfxName)
    {
        if (!audioClips.ContainsKey(sfxName))
        {
            Debug.LogWarning("⚠️ Ses efekti bulunamadı: " + sfxName);
            return;
        }

        AudioClipData sfxData = audioClips[sfxName];
        
        if (sfxData.audioClip != null)
        {
            sfxSource.PlayOneShot(sfxData.audioClip, sfxData.volume * masterVolume);
            Debug.Log("🔊 SFX: " + sfxData.clipName);
        }
    }

    public void PlayAmbience(string ambienceName)
    {
        if (!audioClips.ContainsKey(ambienceName))
        {
            Debug.LogWarning("⚠️ Ortam sesi bulunamadı: " + ambienceName);
            return;
        }

        AudioClipData ambienceData = audioClips[ambienceName];
        
        if (ambienceData.audioClip != null)
        {
            ambientSource.clip = ambienceData.audioClip;
            ambientSource.loop = true;
            ambientSource.volume = ambienceData.volume * masterVolume;
            ambientSource.Play();

            Debug.Log("🌫️ Ortam Sesi: " + ambienceData.clipName);
        }
    }

    public void StopAmbience()
    {
        ambientSource.Stop();
    }

    // VOICE LINES
    public void PlayCharacterDialogue(string characterName, int dialogueIndex = 0)
    {
        if (!characterDialogues.ContainsKey(characterName))
        {
            Debug.LogWarning("⚠️ Karakter bulunamadı: " + characterName);
            return;
        }

        List<VoiceLine> dialogues = characterDialogues[characterName];
        
        if (dialogueIndex >= dialogues.Count)
        {
            Debug.LogWarning("⚠️ Diyalog indeksi geçersiz");
            return;
        }

        VoiceLine voiceLine = dialogues[dialogueIndex];
        
        Debug.Log("\n═══════════════════════════════════════");
        Debug.Log("🎤 " + characterName);
        Debug.Log("═══════════════════════════════════════");
        Debug.Log("\"" + voiceLine.dialogueText + "\"");
        Debug.Log("Süre: " + voiceLine.duration + "s");
        Debug.Log("═══════════════════════════════════════\n");

        if (voiceLine.voiceClip != null)
        {
            voiceSource.PlayOneShot(voiceLine.voiceClip, voiceVolume * masterVolume);
            voiceLine.isSeen = true;
        }
    }

    // VOLUME CONTROL
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
        Debug.Log("🔊 Ana Ses: " + (masterVolume * 100) + "%");
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        bgmSource.volume = bgmVolume * masterVolume;
        Debug.Log("🎵 Müzik Ses: " + (bgmVolume * 100) + "%");
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume * masterVolume;
        Debug.Log("🔊 Efekt Ses: " + (sfxVolume * 100) + "%");
    }

    public void SetVoiceVolume(float volume)
    {
        voiceVolume = Mathf.Clamp01(volume);
        voiceSource.volume = voiceVolume * masterVolume;
        Debug.Log("🎤 Konuşma Ses: " + (voiceVolume * 100) + "%");
    }

    private void UpdateAllVolumes()
    {
        bgmSource.volume = bgmVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
        voiceSource.volume = voiceVolume * masterVolume;
        ambientSource.volume = ambientVolume * masterVolume;
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;
        Debug.Log(isMuted ? "🔇 Sessiz Mod AÇ" : "🔊 Sessiz Mod KAPAT");
    }

    public float GetMasterVolume() => masterVolume;
    public float GetBGMVolume() => bgmVolume;
    public bool IsMuted() => isMuted;
}
