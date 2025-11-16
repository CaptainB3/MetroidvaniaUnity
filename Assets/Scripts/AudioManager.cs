using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // ───────────────── SINGLETON ─────────────────
    public static AudioManager Instance { get; private set; }

    [Header("Mixer (Optional but Recommended)")]
    [Tooltip("Reference to an AudioMixer with 'MasterVolume', 'MusicVolume', 'SFXVolume' parameters.")]
    public AudioMixer audioMixer;
    public string masterVolumeParam = "MasterVolume";
    public string musicVolumeParam = "MusicVolume";
    public string sfxVolumeParam = "SFXVolume";

    [Header("Music Settings")]
    [SerializeField] private AudioSource musicSourceA;
    [SerializeField] private AudioSource musicSourceB;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Tooltip("Default fade time when switching music.")]
    public float defaultMusicFadeTime = 1f;

    [Header("SFX Settings")]
    [SerializeField] private AudioSource sfxSource;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Tooltip("Number of extra AudioSources for overlapping SFX.")]
    public int sfxVoices = 8;
    private AudioSource[] sfxPool;
    private int sfxPoolIndex = 0;

    [Header("Save/Load Settings")]
    public bool saveVolumeToPlayerPrefs = true;
    private const string MasterVolumeKey = "Audio_MasterVolume";
    private const string MusicVolumeKey = "Audio_MusicVolume";
    private const string SFXVolumeKey = "Audio_SFXVolume";

    private AudioSource currentMusicSource;
    private AudioSource idleMusicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure music sources exist
        if (musicSourceA == null || musicSourceB == null)
        {
            Debug.LogError("AudioManager: Please assign musicSourceA and musicSourceB in the inspector.");
        }

        currentMusicSource = musicSourceA;
        idleMusicSource = musicSourceB;

        // Create SFX pool
        if (sfxSource != null && sfxVoices > 0)
        {
            sfxPool = new AudioSource[sfxVoices];
            for (int i = 0; i < sfxVoices; i++)
            {
                GameObject sfxObj = new GameObject("SFXVoice_" + i);
                sfxObj.transform.SetParent(transform);
                AudioSource source = sfxObj.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.spatialBlend = 0f; // 2D
                sfxPool[i] = source;
            }
        }

        // Load saved volume
        if (saveVolumeToPlayerPrefs)
        {
            LoadVolumeSettings();
        }
        ApplyVolumeToMixer();
    }

    // ───────────────── MUSIC CONTROL ─────────────────

//sic(AudioClip clip, float fadeDuration = -1f, bool loop = true)
//    {
//        if (clip == null)
//        {
//            Debug.LogWarning("AudioManager.PlayMusic called with null clip.");
//            return;
//        }

//        if (fadeDuration < 0f)
//            fadeDuration = defaultMusicFadeTime;

//        idleMusicSource.clip = clip;
//        idleMusicSource.loop = loop;
//        idleMusicSource.volume = 0f;
//        idleMusicSource.Play();

//        // Crossfade
//        StartCoroutine(CrossfadeMusic(currentMusicSource, idleMusicSource, fadeDuration));

//        // Swap references
//        AudioSource temp = currentMusicSource;
//        currentMusicSource = idleMusicSource;
//        idleMusicSource = temp;
//    }

    public void StopMusic(float fadeDuration = -1f)
    {
        if (fadeDuration < 0f)
            fadeDuration = defaultMusicFadeTime;

        if (currentMusicSource != null && currentMusicSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop(currentMusicSource, fadeDuration));
        }
    }

    private IEnumerator CrossfadeMusic(AudioSource fromSource, AudioSource toSource, float duration)
    {
        float time = 0f;
        float startFrom = fromSource != null ? fromSource.volume : 0f;
        float targetTo = musicVolume;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = duration > 0f ? time / duration : 1f;
            t = Mathf.Clamp01(t);

            if (fromSource != null)
                fromSource.volume = Mathf.Lerp(startFrom, 0f, t);

            if (toSource != null)
                toSource.volume = Mathf.Lerp(0f, targetTo, t);

            yield return null;
        }

        if (fromSource != null)
        {
            fromSource.Stop();
            fromSource.volume = 0f;
        }

        if (toSource != null)
        {
            toSource.volume = targetTo;
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration)
    {
        float time = 0f;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float t = duration > 0f ? time / duration : 1f;
            t = Mathf.Clamp01(t);

            source.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        source.Stop();
        source.volume = 0f;
    }

    // ───────────────── SFX CONTROL ─────────────────

  
    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f, float pitchMin = 1f, float pitchMax = 1f)
    {
        if (clip == null) return;

        AudioSource src = GetNextSFXSource();
        if (src == null)
        {
            // Fallback to main SFX source
            if (sfxSource == null) return;
            src = sfxSource;
        }

        src.pitch = Random.Range(pitchMin, pitchMax);
        float finalVolume = sfxVolume * Mathf.Clamp01(volumeMultiplier);
        src.PlayOneShot(clip, finalVolume);
    }


    public void PlaySFXAtPoint(AudioClip clip, Vector3 position, float volumeMultiplier = 1f, float spatialBlend = 1f)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("SFX_3D_" + clip.name);
        tempGO.transform.position = position;
        AudioSource src = tempGO.AddComponent<AudioSource>();
        src.clip = clip;
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        src.volume = sfxVolume * Mathf.Clamp01(volumeMultiplier);
        src.Play();
        Destroy(tempGO, clip.length + 0.1f);
    }

    private AudioSource GetNextSFXSource()
    {
        if (sfxPool == null || sfxPool.Length == 0)
            return null;

        AudioSource src = sfxPool[sfxPoolIndex];
        sfxPoolIndex = (sfxPoolIndex + 1) % sfxPool.Length;
        return src;
    }

    // ───────────────── VOLUME CONTROL ─────────────────

    public void SetMasterVolume(float normalizedVolume)
    {
        normalizedVolume = Mathf.Clamp01(normalizedVolume);
        if (audioMixer != null && !string.IsNullOrEmpty(masterVolumeParam))
        {
            audioMixer.SetFloat(masterVolumeParam, LinearToDecibels(normalizedVolume));
        }

        if (saveVolumeToPlayerPrefs)
        {
            PlayerPrefs.SetFloat(MasterVolumeKey, normalizedVolume);
        }
    }

    public void SetMusicVolume(float normalizedVolume)
    {
        musicVolume = Mathf.Clamp01(normalizedVolume);

        if (audioMixer != null && !string.IsNullOrEmpty(musicVolumeParam))
        {
            audioMixer.SetFloat(musicVolumeParam, LinearToDecibels(musicVolume));
        }

        if (currentMusicSource != null)
            currentMusicSource.volume = musicVolume;

        if (saveVolumeToPlayerPrefs)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        }
    }

    public void SetSFXVolume(float normalizedVolume)
    {
        sfxVolume = Mathf.Clamp01(normalizedVolume);

        if (audioMixer != null && !string.IsNullOrEmpty(sfxVolumeParam))
        {
            audioMixer.SetFloat(sfxVolumeParam, LinearToDecibels(sfxVolume));
        }

        if (saveVolumeToPlayerPrefs)
        {
            PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolume);
        }
    }

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(MasterVolumeKey))
        {
            float master = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
            SetMasterVolume(master);
        }

        if (PlayerPrefs.HasKey(MusicVolumeKey))
        {
            float music = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            musicVolume = music;
        }

        if (PlayerPrefs.HasKey(SFXVolumeKey))
        {
            float sfx = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
            sfxVolume = sfx;
        }
    }

    private void ApplyVolumeToMixer()
    {
        if (audioMixer != null)
        {
            if (!string.IsNullOrEmpty(musicVolumeParam))
                audioMixer.SetFloat(musicVolumeParam, LinearToDecibels(musicVolume));

            if (!string.IsNullOrEmpty(sfxVolumeParam))
                audioMixer.SetFloat(sfxVolumeParam, LinearToDecibels(sfxVolume));
        }
    }

    // Convert 0–1 slider value to decibels (for AudioMixer)
    private float LinearToDecibels(float linear)
    {
        if (linear <= 0.0001f)
            return -80f; // practically silent

        return Mathf.Log10(linear) * 20f;
    }
}
