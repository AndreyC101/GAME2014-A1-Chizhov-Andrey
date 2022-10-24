using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMusic
{
    MENU_THEME,
    GAME_THEME,
    GAME_OVER_THEME
}

public enum GameSFX
{
    SELECT,
    PLACE_TOWER,
    POP
}

public class GameSystemsManager : MonoBehaviour
{
    public bool gameInProgress = false;

    //Gameplay members
    UIManager uiManager;
    [SerializeField]
    EnemyManager enemyManager;
    [SerializeField]
    TowerManager towerManager;

    [SerializeField]
    private AudioSource GameSoundtrack;
    [SerializeField]
    private AudioSource GameAudio;

    [SerializeField]
    private AudioClip[] gameMusic;

    [SerializeField]
    private AudioClip[] gameSFX;

    [SerializeField]
    private GameObject GameplayObjects;


    //Player members
    public int playerLives;
    public int playerMoney;


    //Game state functions
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    void Start()
    {
        uiManager.OpenMainMenu();
        PlayMenuTheme();
    }

    public void GameStart()
    {
        if (gameInProgress) GameContinue();
        else
        {
            uiManager.OpenGameUI();
            EnableGameplayObjects();
            enemyManager.Initialize();
            GameSoundtrack.Stop();
            GameSoundtrack.clip = gameMusic[(int)GameMusic.GAME_THEME];
            GameSoundtrack.loop = true;
            GameSoundtrack.volume = 0.285f;
            GameSoundtrack.Play();
            gameInProgress = true;
            playerLives = 150;
            uiManager.UpdateHealthDisplay();
            playerMoney = 250;
            uiManager.UpdateCurrencyDisplays();
            towerManager.DestroyAllTowers();
            enemyManager.DestroyAllBloons();
        }
    }

    public void GameContinue()
    {
        uiManager.OpenGameUI();
        EnableGameplayObjects();
    }

    public void ReturnToMainMenu()
    {
        DisableGameplayObjects();
        uiManager.OpenMainMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EnableGameplayObjects()
    {
        GameplayObjects.SetActive(true);
    }

    public void DisableGameplayObjects()
    {
        GameplayObjects.SetActive(false);
    }


    //Bloon Related Functions
    public void OnBloonGotThrough(int bloonLevel)
    {
        playerLives-=(bloonLevel+1);
        uiManager.UpdateHealthDisplay();
        if (playerLives <= 0)
        {
            enemyManager.DestroyAllBloons();
            gameInProgress = false;
            DisableGameplayObjects();
            PlayGameOverTheme();
            uiManager.OpenGameOverMenu();
        }
    }

    public void OnBloonPopped()
    {
        playerMoney++;
        uiManager.UpdateCurrencyDisplays();
        GameAudio.PlayOneShot(gameSFX[Random.Range(2, 6)]);
    }

    public void OnTowerPlaced(int type)
    {
        ChargePlayerMoney(towerManager.TowerCosts[type]);
        GameAudio.PlayOneShot(gameSFX[(int)GameSFX.PLACE_TOWER]);
    }

    //Money Related Functions
    public void ChargePlayerMoney(int price)
    {
        playerMoney -= price;
        uiManager.UpdateCurrencyDisplays();
    }

    public void RewardPlayerMoney(int reward)
    {
        playerMoney += reward;
        uiManager.UpdateCurrencyDisplays();
    }

    //Audio functions
    public void PlayMenuTheme()
    {
        GameSoundtrack.Stop();
        GameSoundtrack.clip = gameMusic[(int)GameMusic.MENU_THEME];
        GameSoundtrack.loop = true;
        GameSoundtrack.volume = 0.35f;
        GameSoundtrack.Play();
    }

    public void PlayGameOverTheme()
    {
        GameSoundtrack.Stop();
        GameSoundtrack.clip = gameMusic[(int)GameMusic.GAME_OVER_THEME];
        GameSoundtrack.loop = true;
        GameSoundtrack.volume = 0.35f;
        GameSoundtrack.Play();
    }

    public void PlayButtonSoundEffect()
    {
        GameAudio.PlayOneShot(gameSFX[(int)GameSFX.SELECT]);
    }
}   
