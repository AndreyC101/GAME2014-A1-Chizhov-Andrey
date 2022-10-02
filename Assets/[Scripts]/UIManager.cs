using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameSystemsManager GameManager;

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

    //Gameplay
    

    [SerializeField]
    private GameObject TowerMenu;

    [SerializeField]
    private GameObject InGameCurrencyIndicator;

    private void Awake()
    {
        GameManager = FindObjectOfType<GameSystemsManager>();
    }

    //Gameplay State Switched

    public void OpenTowerMenu()
    {
        InGameCurrencyIndicator.SetActive(false);
        TowerMenu.SetActive(true);
    }

    public void CloseTowerMenu()
    {
        InGameCurrencyIndicator.SetActive(true);
        TowerMenu.SetActive(false);
    }

    //Menu State Switches
    public void OpenMainMenu()
    {
        UICanvas.SetActive(false);
        MenuCanvas.SetActive(true);

        MainMenuPanel.SetActive(true);
        InstructionsMenuPanel.SetActive(false);
        GameOverMenuPanel.SetActive(false);

        GameManager.DisableGameplayObjects();
    }

    public void OpenInstructionsMenu()
    {
        UICanvas.SetActive(false);
        MenuCanvas.SetActive(true);

        MainMenuPanel.SetActive(false);
        InstructionsMenuPanel.SetActive(true);
        GameOverMenuPanel.SetActive(false);
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
    }
}
