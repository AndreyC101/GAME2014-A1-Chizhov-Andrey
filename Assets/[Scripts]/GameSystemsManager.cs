using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemsManager : MonoBehaviour
{
    UIManager uiManager;

    [SerializeField]
    private GameObject GameplayObjects;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    void Start()
    {
        uiManager.OpenMainMenu();
    }

    public void GameStart()
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
}
