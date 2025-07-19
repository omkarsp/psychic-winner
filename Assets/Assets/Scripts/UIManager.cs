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
    }
    
    public void HideMainMenu()
    {
        mainMenuPanel.SetActive(false);
    }
    
    private void OnPlayButtonClicked()
    {
        Debug.Log($"Starting game with difficulty: {selectedDifficulty}");
        GameManager.Instance.StartGame();
        HideMainMenu();
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
    
    // Public methods to get current audio settings
    public bool IsMusicEnabled() => musicEnabled;
    public float GetMusicVolume() => musicVolume;
    public bool AreEffectsEnabled() => effectsEnabled;
    public float GetEffectsVolume() => effectsVolume;
}