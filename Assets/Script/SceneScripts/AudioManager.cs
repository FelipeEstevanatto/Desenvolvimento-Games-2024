using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool IsMuted => isMuted;
    [SerializeField] private bool isMuted = false;

    [Header("Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]

    public AudioClip explosionClip;
    public AudioClip bombClip;
    public AudioClip rifleSingleClip;

    public AudioClip rifleBurstClip;
    public AudioClip errorSoundClip;
    public AudioClip HealSoundClip;
    public AudioClip menuMusic;
    public AudioClip inGameMusic;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PlayMusic(inGameMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            ToggleMute();
        }

    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
    }

    public void PlayMusic(AudioClip musicClip, float volume = 1f)
    {
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.volume = musicVolume * volume;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip sfxClip, float volume = 1f)
    {
        if (sfxSource != null && sfxClip != null)
        {
            // This allows sounds to overlap
            sfxSource.PlayOneShot(sfxClip, sfxVolume * volume);
        }
    }

    public void PlaySFXAtPosition(AudioClip sfxClip, Vector3 position, float volume = 1f)
    {
        if (sfxSource != null && sfxClip != null)
        {
            AudioSource.PlayClipAtPoint(sfxClip, position, sfxVolume * volume);
        }
        // if (clip == null) return;
    
        // // Create a temporary game object
        // GameObject tempGO = new GameObject("TempAudio");
        // tempGO.transform.position = position;

        // // Create AudioSource and configure 3D settings
        // AudioSource aSource = tempGO.AddComponent<AudioSource>();
        // aSource.clip = clip;

        // // 1. Spatial Blend = 1 means fully 3D
        // aSource.spatialBlend = 1f;

        // // 2. Adjust distance rolloff
        // aSource.rolloffMode = AudioRolloffMode.Logarithmic;
        // aSource.minDistance = 1f;   // Start attenuating beyond 1 unit away
        // aSource.maxDistance = 20f;  // Volume fades out fully at 20 units
        // // ^ tweak these to match your 2D world scale

        // // 3. Volume (include your global sfxVolume if desired)
        // aSource.volume = sfxVolume * volume;

        // // Play and destroy
        // aSource.Play();
        // Destroy(tempGO, clip.length);
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (musicSource != null)
        {
            if (musicSource.isPlaying)
            {
                musicSource.Pause();
            }
        }
    }

    public void UnpauseMusic()
    {
        if (musicSource != null)
        {
            if (!musicSource.isPlaying)
            {
                musicSource.UnPause();
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;
    }
}
