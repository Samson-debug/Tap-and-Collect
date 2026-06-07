using UnityEngine;
using TMPro;

[DefaultExecutionOrder(100)]
public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("UI Text Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Events & References")]
    [SerializeField] private VoidEventChannelSO scoreIncreaseEvent;
    [SerializeField] private VoidEventChannelSO gameStartedEvent;
    [SerializeField] private VoidEventChannelSO gameOverEvent;
    [SerializeField] private VoidEventChannelSO healthDecreaseEvent;
    
    [Tooltip("Reference to GameManager to read Timer and Lives")]
    [SerializeField] private GameManager gameManager;

    private int score;

    private void Start()
    {
        // Initial state
        ShowPanel(startPanel);
        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void OnEnable()
    {
        if (scoreIncreaseEvent != null) scoreIncreaseEvent.OnEventRaised += IncrementScore;
        if (gameStartedEvent != null) gameStartedEvent.OnEventRaised += HandleGameStarted;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised += HandleGameOver;
        if (healthDecreaseEvent != null) healthDecreaseEvent.OnEventRaised += UpdateLivesUI;
    }

    private void OnDisable()
    {
        if (scoreIncreaseEvent != null) scoreIncreaseEvent.OnEventRaised -= IncrementScore;
        if (gameStartedEvent != null) gameStartedEvent.OnEventRaised -= HandleGameStarted;
        if (gameOverEvent != null) gameOverEvent.OnEventRaised -= HandleGameOver;
        if (healthDecreaseEvent != null) healthDecreaseEvent.OnEventRaised -= UpdateLivesUI;
    }

    private void Update()
    {
        if (gameManager != null && gameManager.IsPlaying)
        {
            UpdateTimerUI();
        }
    }

    private void HandleGameStarted()
    {
        score = 0;
        UpdateScoreUI();
        UpdateLivesUI();
        ShowPanel(gamePanel);
    }

    private void HandleGameOver()
    {
        if (finalScoreText != null)
        {
            finalScoreText.text = score.ToString();
        }
        ShowPanel(gameOverPanel);
    }

    private void IncrementScore()
    {
        score++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null && gameManager != null)
        {
            livesText.text = gameManager.CurrentLives.ToString();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null && gameManager != null && gameManager.GameTimer != null)
        {
            timerText.text = $"{Mathf.CeilToInt(gameManager.GameTimer.CurrentTime)}s";
        }
    }

    public void PauseGame()
    {
        if (gameManager != null) gameManager.PauseGame();
        ShowPanel(pausePanel);
    }

    public void ResumeGame()
    {
        if (gameManager != null) gameManager.ResumeGame();
        ShowPanel(gamePanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        if (startPanel != null) startPanel.SetActive(startPanel == panelToShow);
        if (gamePanel != null) gamePanel.SetActive(gamePanel == panelToShow);
        if (gameOverPanel != null) gameOverPanel.SetActive(gameOverPanel == panelToShow);
        if (pausePanel != null) pausePanel.SetActive(pausePanel == panelToShow);
    }
}
