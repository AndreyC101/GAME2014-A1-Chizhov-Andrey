using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject GameplayObjects;


    //Player members
    public int playerLives;
    public int playerMoney;


    //Game state functions
    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        //enemyManager = FindObjectOfType<EnemyManager>();
        //towerManager = FindObjectOfType<TowerManager>();
    }
    void Start()
    {
        uiManager.OpenMainMenu();
    }

    public void GameStart()
    {
        if (gameInProgress) GameContinue();
        else
        {
            uiManager.OpenGameUI();
            EnableGameplayObjects();
            playerLives = 150;
            uiManager.UpdateHealthDisplay();
            playerMoney = 300;
            uiManager.UpdateCurrencyDisplays();
            enemyManager.Initialize();
            gameInProgress = true;
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
        if (playerLives == 0)
        {
            enemyManager.DestroyAllBloons();
            gameInProgress = false;
            DisableGameplayObjects();
            uiManager.OpenGameOverMenu();
        }
    }

    public void OnBloonPopped()
    {
        playerMoney++;
        uiManager.UpdateCurrencyDisplays();
    }

    //Money Related Functions
    public void ChargePlayerMoney(int price)
    {
        playerMoney -= price;
        uiManager.UpdateCurrencyDisplays();
    }
}   
