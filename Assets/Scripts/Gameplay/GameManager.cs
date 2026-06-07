using UnityEngine;
using Core.Utilities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private float gameDurationSeconds = 60f;

    [Header("Events Setup")]
    [SerializeField] private VoidEventChannelSO healthDecreaseEvent;
    [SerializeField] private VoidEventChannelSO gameStartedEvent;
    [SerializeField] private VoidEventChannelSO gameOverEvent;

    public int CurrentLives { get; private set; }
    public Timer GameTimer { get; private set; }
    public bool IsPlaying { get; private set; }
    public bool IsPaused { get; private set; }

    private void Awake()
    {
        GameTimer = new Timer(gameDurationSeconds);
        GameTimer.OnTimerComplete += HandleTimeOver;
        IsPlaying = false;
        IsPaused = false;
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        if (healthDecreaseEvent != null)
        {
            healthDecreaseEvent.OnEventRaised += HandleHealthDecrease;
        }
    }

    private void OnDisable()
    {
        if (healthDecreaseEvent != null)
        {
            healthDecreaseEvent.OnEventRaised -= HandleHealthDecrease;
        }
    }

    private void Update()
    {
        if (IsPlaying && !IsPaused)
        {
            GameTimer.Update(Time.deltaTime);
        }
    }

    public void StartGame()
    {
        if (IsPlaying) return;

        CurrentLives = startingLives;
        GameTimer.Reset(gameDurationSeconds);
        GameTimer.Start();
        IsPlaying = true;

        if (gameStartedEvent != null)
        {
            gameStartedEvent.RaiseEvent();
        }
        
        Debug.Log("Game Started");
    }

    public void RestartGame()
    {
        IsPaused = false;
        GameTimer.Stop();

        /*if (gameOverEvent != null)
        {
            gameOverEvent.RaiseEvent();
        }*/
        
        CurrentLives = startingLives;
        GameTimer.Reset(gameDurationSeconds);
        GameTimer.Start();
        Time.timeScale = 1f;
        IsPlaying = true;

        if (gameStartedEvent != null)
        {
            gameStartedEvent.RaiseEvent();
        }
        
        Debug.Log("Game Restarted");
    }

    private void HandleHealthDecrease()
    {
        if (!IsPlaying) return;

        CurrentLives--;
        Debug.Log($"Lost a life! Lives remaining: {CurrentLives}");

        if (CurrentLives <= 0)
        {
            GameOver();
        }
    }

    private void HandleTimeOver()
    {
        if (!IsPlaying) return;
        Debug.Log("Time Over!");
        GameOver();
    }

    public void GameOver()
    {
        if (!IsPlaying) return;

        IsPlaying = false;
        GameTimer.Stop();

        if (gameOverEvent != null)
        {
            gameOverEvent.RaiseEvent();
        }
        
        Debug.Log("Game Over");
    }

    public void PauseGame()
    {
        if (!IsPlaying || IsPaused) return;
        
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!IsPlaying || !IsPaused) return;

        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
