using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip uiButtonSound;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip catchEffectSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private AudioClip specialEffectSound;

    [Header("Events Setup")]
    [SerializeField] private VoidEventChannelSO scoreIncreaseEvent; // Catch Effect Trigger
    [SerializeField] private VoidEventChannelSO gameOverEvent; // Game Over Trigger
    [SerializeField] private VoidEventChannelSO gameStartedEvent; // Music Start Trigger
    [SerializeField] private VoidEventChannelSO healthDecreaseEvent; // Damage Trigger
    [SerializeField] private VoidEventChannelSO sizeIncreaseEvent; // Special Effect Trigger

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic);
        }
    }

    private void OnEnable()
    {
        if (scoreIncreaseEvent != null) scoreIncreaseEvent.OnEventRaised += PlayCatchEffect;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised += PlayGameOver;
        if (healthDecreaseEvent != null) healthDecreaseEvent.OnEventRaised += PlayDamageSound;
        if (sizeIncreaseEvent != null) sizeIncreaseEvent.OnEventRaised += PlaySpecialEffectSound;
    }

    private void OnDisable()
    {
        if (scoreIncreaseEvent != null) scoreIncreaseEvent.OnEventRaised -= PlayCatchEffect;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised -= PlayGameOver;
        if (healthDecreaseEvent != null) healthDecreaseEvent.OnEventRaised -= PlayDamageSound;
        if (sizeIncreaseEvent != null) sizeIncreaseEvent.OnEventRaised -= PlaySpecialEffectSound;
    }

    public void PlayUIButton()
    {
        if (uiButtonSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(uiButtonSound);
        }
    }

    public void PlayCatchEffect()
    {
        if (catchEffectSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(catchEffectSound);
        }
    }

    public void PlayDamageSound()
    {
        if (damageSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(damageSound);
        }
    }

    public void PlaySpecialEffectSound()
    {
        if (specialEffectSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(specialEffectSound);
        }
    }

    public void PlayGameOver()
    {
        if (gameOverSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(gameOverSound);
        }
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    private void PlayMusic(AudioClip clip)
    {
        if (musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
