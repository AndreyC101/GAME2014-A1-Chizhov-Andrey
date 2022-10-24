using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameSystemsManager gsm;
    [SerializeField]
    private TowerManager tm;

    //Menu Panels
    [SerializeField]
    private GameObject UICanvas;

    [SerializeField]
    private GameObject MenuCanvas;

    [SerializeField]
    private GameObject MainMenuPanel;

    [SerializeField]
    private GameObject InstructionsMenuPanel;

    [SerializeField]
    private GameObject GameOverMenuPanel;

    [SerializeField]
    private GameObject StartWaveButton;

    [SerializeField]
    private TMP_Text PlayButtonText;

    //Gameplay
    [SerializeField]
    private TMP_Text healthDisplay;
    [SerializeField]
    private TMP_Text moneyDisplay_main;
    [SerializeField]
    private TMP_Text moneyDisplay_tower;

    [SerializeField]
    private GameObject TowerMenu;

    [SerializeField]
    private GameObject InGameCurrencyIndicator;

    [SerializeField]
    private TMP_Text[] TowerButtonCostDisplays;

    private void Awake()
    {
        gsm = FindObjectOfType<GameSystemsManager>();
    }

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            TowerButtonCostDisplays[i].text = tm.TowerCosts[i].ToString();
        }
    }

    //Gameplay State Switches

    public void OpenTowerMenu()
    {
        InGameCurrencyIndicator.SetActive(false);
        StartWaveButton.SetActive(false);
        TowerMenu.SetActive(true);
        gsm.PlayButtonSoundEffect();
    }

    public void CloseTowerMenu()
    {
        InGameCurrencyIndicator.SetActive(true);
        StartWaveButton.SetActive(true);
        TowerMenu.SetActive(false);
        gsm.PlayButtonSoundEffect();
    }

    public void UpdateHealthDisplay()
    {
        healthDisplay.text = gsm.playerLives.ToString();
    }

    public void UpdateCurrencyDisplays()
    {
        moneyDisplay_main.text = gsm.playerMoney.ToString();
        moneyDisplay_tower.text = gsm.playerMoney.ToString();
    }

    public void TowerSelectButtonPressed(TowerType type, Vector3 worldPosition)
    {
        if (gsm.playerMoney >= tm.TowerCosts[(int)type])
        {
            gsm.PlayButtonSoundEffect();
            CloseTowerMenu();
            tm.CreateDropTower(type, worldPosition);
        }
    }

    public void OnWaveStart()
    {
        StartWaveButton.SetActive(false);
        FindObjectOfType<EnemyManager>().WaveStart();
        gsm.PlayButtonSoundEffect();
    }

    public void OnWaveEnd()
    {
        StartWaveButton.SetActive(true);
    }

    //Menu State Switches
    public void OpenMainMenu()
    {
        UICanvas.SetActive(false);
        MenuCanvas.SetActive(true);

        MainMenuPanel.SetActive(true);
        InstructionsMenuPanel.SetActive(false);
        GameOverMenuPanel.SetActive(false);

        gsm.DisableGameplayObjects();
        PlayButtonText.text = gsm.gameInProgress ? "Continue" : "Start Game";
    }

    public void OpenInstructionsMenu()
    {
        UICanvas.SetActive(false);
        MenuCanvas.SetActive(true);

        MainMenuPanel.SetActive(false);
        InstructionsMenuPanel.SetActive(true);
        GameOverMenuPanel.SetActive(false);
        gsm.PlayButtonSoundEffect();
    }

    public void OpenGameOverMenu()
    {
        UICanvas.SetActive(false);
        MenuCanvas.SetActive(true);

        MainMenuPanel.SetActive(false);
        InstructionsMenuPanel.SetActive(false);
        GameOverMenuPanel.SetActive(true);
    }

    public void OpenGameUI()
    {
        UICanvas.SetActive(true);
        MenuCanvas.SetActive(false);

        MainMenuPanel.SetActive(false);
        InstructionsMenuPanel.SetActive(false);
        GameOverMenuPanel.SetActive(false);

        TowerMenu.SetActive(false);
        InGameCurrencyIndicator.SetActive(true);
        gsm.PlayButtonSoundEffect();
    }

    private void FixedUpdate()
    {
        UpdateTowerButtonCostsDisplays();
    }

    private void UpdateTowerButtonCostsDisplays()
    {
        for (int i = 0; i < 5; i++)
        {
            if (gsm.playerMoney >= tm.TowerCosts[i])
                TowerButtonCostDisplays[i].color = Color.white;
            else TowerButtonCostDisplays[i].color = Color.red;
        }
    }
}
