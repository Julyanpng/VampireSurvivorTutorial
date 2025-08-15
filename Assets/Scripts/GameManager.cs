using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    public GameState currentState;

    public GameState previousState;

    [Header("Screens")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;
    public GameObject levelUpScreen;

    [Header("Current Stat Display")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentReoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;

    [Header("Results Screen Display")]
    public Image chosenCharacterImage;
    public TMP_Text chosenCharacterName;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurivedDisplay;
    public List<Image> chosenWeaponsUI = new List<Image>(6);
    public List<Image> chosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit; //in seconds
    float stopwatchTime;
    public TMP_Text stopwatchDisplay;

    //flag to check if game is over
    public bool isGameOver = false;

    public bool choosingUpgrade;

    public GameObject playerObject;

    void Awake()
    {
        //Warining check to see if there is another singleton of this kind in the game
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " +  this + " DELETED");
            Destroy(gameObject);
        }

        DisableScreens();
    }

    void Update()
    {
        switch(currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("GAME IS OVER");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if(!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    Debug.Log("upgrades shown");
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST");
                break;
        }    
    }

    //define the method to change the state of the game
    public void ChangeState(GameState newState)
    {
        Debug.Log("Changing state from " + currentState + " to " + newState);
        currentState = newState;
    }

    public void PauseGame()
    {
        if(currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("game is oaused");
        }
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
           ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    void CheckForPauseAndResume()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }    
    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObjects chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.Name;
    }
    
    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignWeaponsAndPassiveItemsUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemData)
    {
        if(chosenWeaponData.Count != chosenWeaponsUI.Count || chosenPassiveItemData.Count != chosenWeaponData.Count)
        {
            Debug.Log("Chosen weapons and passive Item data lists have different lengths");
            return; 
        }

        for(int i = 0; i < chosenWeaponsUI.Count; i++)
        {
            if (chosenWeaponData[i].sprite)
            {
                chosenWeaponsUI[i].enabled = true;
                chosenWeaponsUI[i].sprite = chosenWeaponData[i].sprite;
            }
            else
            {
                chosenWeaponsUI[i].enabled = false;
            }
        }

        for (int i = 0; i < chosenPassiveItemsUI.Count; i++)
        {
            if (chosenPassiveItemData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                chosenPassiveItemsUI[i].enabled = false;
            }
        }

    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if(stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelup()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }
}
