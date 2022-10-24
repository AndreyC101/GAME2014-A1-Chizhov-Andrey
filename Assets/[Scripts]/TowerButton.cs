using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler
{
    UIManager uim;

    [SerializeField]
    TowerType towerToSpawn;

    private void Awake()
    {
        uim = FindObjectOfType<UIManager>();
    }

    public void OnPointerDown(PointerEventData data)
    {
        Vector3 worldPosition = FindObjectOfType<Camera>().ScreenToWorldPoint(data.position);
        worldPosition.z = 0.0f;
        uim.TowerSelectButtonPressed(towerToSpawn, worldPosition);
    }
}
