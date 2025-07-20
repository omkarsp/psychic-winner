using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Difficulty
{
    VeryEasy,
    Easy,
    Medium,
    Hard,
    VeryHard
}

public class UIManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    
    [Header("Gameplay UI")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMPro.TextMeshProUGUI difficultyText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private TMPro.TextMeshProUGUI turnsText;
    
    [Header("Level Complete UI")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private TMPro.TextMeshProUGUI levelCompleteText;
    
    [Header("Difficulty Selection")]
    [SerializeField] private ToggleGroup difficultyToggleGroup;
    [SerializeField] private Toggle veryEasyToggle;
    [SerializeField] private Toggle easyToggle;
    [SerializeField] private Toggle mediumToggle;
    [SerializeField] private Toggle hardToggle;
    [SerializeField] private Toggle veryHardToggle;
    
    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button closeSettingsButton;
    
    [Header("Audio Settings")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Toggle effectsToggle;
    [SerializeField] private Slider effectsVolumeSlider;
    
    private Difficulty selectedDifficulty = Difficulty.Medium;
    
    // Audio settings
    private bool musicEnabled = true;
    private float musicVolume = 0.8f;
    private bool effectsEnabled = true;
    private float effectsVolume = 0.8f;
    
    public static UIManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        SetupUI();
    }
    
    private void SetupUI()
    {
        // Setup button listeners
        playButton.onClick.AddListener(OnPlayButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        closeSettingsButton.onClick.AddListener(OnCloseSettingsClicked);
        
        // Gameplay UI listeners
        if (homeButton != null) homeButton.onClick.AddListener(OnHomeButtonClicked);
        if (restartButton != null) restartButton.onClick.AddListener(OnRestartButtonClicked);
        if (continueButton != null) continueButton.onClick.AddListener(OnContinueButtonClicked);
        if (menuButton != null) menuButton.onClick.AddListener(OnHomeButtonClicked);
        
        // Setup audio controls
        musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        effectsToggle.onValueChanged.AddListener(OnEffectsToggleChanged);
        effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
        
        // Setup difficulty toggles
        veryEasyToggle.onValueChanged.AddListener((isOn) => { if (isOn) OnDifficultyChanged(Difficulty.VeryEasy); });
        easyToggle.onValueChanged.AddListener((isOn) => { if (isOn) OnDifficultyChanged(Difficulty.Easy); });
        mediumToggle.onValueChanged.AddListener((isOn) => { if (isOn) OnDifficultyChanged(Difficulty.Medium); });
        hardToggle.onValueChanged.AddListener((isOn) => { if (isOn) OnDifficultyChanged(Difficulty.Hard); });
        veryHardToggle.onValueChanged.AddListener((isOn) => { if (isOn) OnDifficultyChanged(Difficulty.VeryHard); });
        
        // Set default difficulty
        mediumToggle.isOn = true;
        
        // Initialize audio settings
        InitializeAudioSettings();
        
        // Initialize UI state
        ShowMainMenu();
    }
    
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        levelCompletePanel?.SetActive(false);
    }
    
    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void ShowGameplayUI()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        levelCompletePanel?.SetActive(false);
        
        if (gameplayPanel != null)
        {
            gameplayPanel.SetActive(true);
        }
        
        // Subscribe to game events
        SubscribeToGameEvents();
        
        // Immediately update UI with current values
        InitializeGameplayUI();
    }
    
    private void InitializeGameplayUI()
    {
        // Set initial difficulty display
        if (difficultyText != null)
        {
            difficultyText.text = $"Difficulty: {selectedDifficulty}";
        }
        
        // Set initial score and turns (always show 0 at start)
        UpdateScoreUI(0);
        UpdateTurnsUI(0);
        
        Debug.Log($"Gameplay UI initialized - Difficulty: {selectedDifficulty}");
    }

    public void ShowDifficultyCompleteUI()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            
            // Update completion text based on current difficulty
            if (levelCompleteText != null)
            {
                levelCompleteText.text = $"{selectedDifficulty} Complete!";
            }
        }
    }

    public void ShowGameOverUI()
    {
        // For now, treat game over same as difficulty complete
        ShowDifficultyCompleteUI();
    }

    public void ShowPauseUI()
    {
        // Implementation for pause UI if needed
        Debug.Log("Game paused");
    }
    
    private void OnPlayButtonClicked()
    {
        Debug.Log($"Starting game with difficulty: {selectedDifficulty}");
        GameManager.Instance.StartGameWithDifficulty(selectedDifficulty);
    }
    
    private void OnSettingsButtonClicked()
    {
        settingsPanel.SetActive(true);
    }
    
    private void OnCloseSettingsClicked()
    {
        settingsPanel.SetActive(false);
    }
    
    private void OnDifficultyChanged(Difficulty difficulty)
    {
        selectedDifficulty = difficulty;
        Debug.Log($"Difficulty changed to: {difficulty}");
    }
    
    public Difficulty GetSelectedDifficulty()
    {
        return selectedDifficulty;
    }
    
    private void InitializeAudioSettings()
    {
        // Load saved settings (you can implement PlayerPrefs later)
        musicToggle.isOn = musicEnabled;
        musicVolumeSlider.value = musicVolume;
        effectsToggle.isOn = effectsEnabled;
        effectsVolumeSlider.value = effectsVolume;
        
        // Apply initial settings
        UpdateMusicSettings();
        UpdateEffectsSettings();
    }
    
    private void OnMusicToggleChanged(bool isEnabled)
    {
        musicEnabled = isEnabled;
        UpdateMusicSettings();
    }
    
    private void OnMusicVolumeChanged(float volume)
    {
        musicVolume = volume;
        UpdateMusicSettings();
    }
    
    private void OnEffectsToggleChanged(bool isEnabled)
    {
        effectsEnabled = isEnabled;
        UpdateEffectsSettings();
    }
    
    private void OnEffectsVolumeChanged(float volume)
    {
        effectsVolume = volume;
        UpdateEffectsSettings();
    }
    
    private void UpdateMusicSettings()
    {
        float finalVolume = musicEnabled ? musicVolume : 0f;
        // Apply to audio system - you can implement AudioManager later
        Debug.Log($"Music: Enabled={musicEnabled}, Volume={finalVolume:F2}");
        
        // Save settings (implement PlayerPrefs later if needed)
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }
    
    private void UpdateEffectsSettings()
    {
        float finalVolume = effectsEnabled ? effectsVolume : 0f;
        // Apply to audio system - you can implement AudioManager later
        Debug.Log($"Effects: Enabled={effectsEnabled}, Volume={finalVolume:F2}");
        
        // Save settings (implement PlayerPrefs later if needed)
        PlayerPrefs.SetInt("EffectsEnabled", effectsEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("EffectsVolume", effectsVolume);
    }
    
    // Gameplay button handlers
    private void OnHomeButtonClicked()
    {
        UnsubscribeFromGameEvents();
        GameManager.Instance.ReturnToMainMenu();
    }

    private void OnRestartButtonClicked()
    {
        if (GameManager.Instance.MemoryGameManager != null)
        {
            GameManager.Instance.MemoryGameManager.RestartGame();
        }
    }

    private void OnContinueButtonClicked()
    {
        // Progress to next difficulty
        Difficulty nextDifficulty = GetNextDifficulty(selectedDifficulty);
        
        if (nextDifficulty != selectedDifficulty)
        {
            // Move to next difficulty
            selectedDifficulty = nextDifficulty;
            UpdateDifficultyToggle();
            levelCompletePanel?.SetActive(false);
            GameManager.Instance.StartGameWithDifficulty(selectedDifficulty);
        }
        else
        {
            // All difficulties complete - show play again option
            if (levelCompleteText != null)
            {
                levelCompleteText.text = "All Difficulties Complete!";
            }
            
            if (continueButton != null)
            {
                continueButton.gameObject.SetActive(false);
            }
            
            // Add Play Again button functionality (reuse continue button)
            var buttonText = continueButton?.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "Play Again";
            }
        }
    }

    private Difficulty GetNextDifficulty(Difficulty current)
    {
        return current switch
        {
            Difficulty.VeryEasy => Difficulty.Easy,
            Difficulty.Easy => Difficulty.Medium,
            Difficulty.Medium => Difficulty.Hard,
            Difficulty.Hard => Difficulty.VeryHard,
            Difficulty.VeryHard => Difficulty.VeryHard, // Stay at max
            _ => Difficulty.VeryEasy
        };
    }

    private void UpdateDifficultyToggle()
    {
        switch (selectedDifficulty)
        {
            case Difficulty.VeryEasy:
                veryEasyToggle.isOn = true;
                break;
            case Difficulty.Easy:
                easyToggle.isOn = true;
                break;
            case Difficulty.Medium:
                mediumToggle.isOn = true;
                break;
            case Difficulty.Hard:
                hardToggle.isOn = true;
                break;
            case Difficulty.VeryHard:
                veryHardToggle.isOn = true;
                break;
        }
    }

    // Game event subscription
    private void SubscribeToGameEvents()
    {
        var gameManager = GameManager.Instance?.MemoryGameManager;
        if (gameManager != null)
        {
            gameManager.OnScoreChanged += UpdateScoreUI;
            gameManager.OnTurnsChanged += UpdateTurnsUI;
            gameManager.OnGameWon += OnGameWon;
            gameManager.OnGameStarted += OnGameStarted;
        }
    }

    private void UnsubscribeFromGameEvents()
    {
        var gameManager = GameManager.Instance?.MemoryGameManager;
        if (gameManager != null)
        {
            gameManager.OnScoreChanged -= UpdateScoreUI;
            gameManager.OnTurnsChanged -= UpdateTurnsUI;
            gameManager.OnGameWon -= OnGameWon;
            gameManager.OnGameStarted -= OnGameStarted;
        }
    }

    // Game event handlers
    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
            Debug.Log($"Score UI updated: {score}");
        }
        else
        {
            Debug.LogWarning("Score text component is null!");
        }
    }

    private void UpdateTurnsUI(int turns)
    {
        if (turnsText != null)
        {
            turnsText.text = $"Turns: {turns}";
            Debug.Log($"Turns UI updated: {turns}");
        }
        else
        {
            Debug.LogWarning("Turns text component is null!");
        }
    }

    private void OnGameWon()
    {
        GameManager.Instance.CompleteDifficulty();
    }

    private void OnGameStarted()
    {
        if (difficultyText != null)
        {
            difficultyText.text = $"Difficulty: {selectedDifficulty}";
        }
        
        // Reset UI values
        UpdateScoreUI(0);
        UpdateTurnsUI(0);
        
        Debug.Log($"Game started UI updated - Difficulty: {selectedDifficulty}");
    }

    // Public methods to get current audio settings
    public bool IsMusicEnabled() => musicEnabled;
    public float GetMusicVolume() => musicVolume;
    public bool AreEffectsEnabled() => effectsEnabled;
    public float GetEffectsVolume() => effectsVolume;

    private void OnDestroy()
    {
        UnsubscribeFromGameEvents();
    }
}