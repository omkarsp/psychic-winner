using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Gameplay,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameState currentGameState;

    public GameState CurrentGameState
    {
        get { return currentGameState; }
        set { currentGameState = value; }
    }

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

    public void StartGame()
    {
        ChangeGameState(GameState.Gameplay);
    }

    public void ReturnToMainMenu()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void EndGame()
    {
        ChangeGameState(GameState.GameOver);
    }

    private void Start()
    {
        // Start with the main menu
        ChangeGameState(GameState.MainMenu);
    }

    private void ChangeGameState(GameState newState)
    {
        currentGameState = newState;

        switch(newState){
            case GameState.MainMenu:
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.ShowMainMenu();
                }
                break;
            case GameState.Gameplay:
                if (UIManager.Instance != null)
                {
                    UIManager.Instance.HideMainMenu();
                }
                //TODO: Implement Gameplay
                break;
            case GameState.GameOver:
                //TODO: Implement Game Over
                break;
            default:
                //TODO: Implement Default
                break;
        }
    }
}
