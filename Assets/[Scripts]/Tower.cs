using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    BALLISTA = 0,
    SPREADSHOT = 1,
    ICE = 2,
    LIGHTNING = 3,
    BOOST = 4,
    TOWER_TYPE_COUNT = 5
}

public class Tower : MonoBehaviour
{
    TowerType type;
    private bool active = false;

    private SpriteRenderer WeaponBaseSR;

    private Transform WeaponTransform;

    private BoxCollider2D PlacementCollider;
    private List<GameObject> PlacementContacts = new List<GameObject>();

    public void InitializeDropTower(TowerType type)
    {
        this.type = type;
        WeaponBaseSR = GetComponent<SpriteRenderer>();
        WeaponTransform = GetComponentInChildren<Transform>();
        PlacementCollider = GetComponent<BoxCollider2D>();
    }

    private void InitializeTower()
    {
        WeaponBaseSR.color = Color.white;
        //InitializeWeapon();
        active = true;
    }

    private void InitializeWeapon()
    {
        switch (type)
        {
            case TowerType.BALLISTA:
                WeaponTransform.position = new Vector3(0.0f, 1.0f, 0.0f);
                WeaponTransform.gameObject.AddComponent<Ballista_Weapon>();
                break;

            case TowerType.SPREADSHOT:
                WeaponTransform.position = new Vector3(0.0f, 0.8f, 0.0f);
                break;

            case TowerType.ICE:
                break;

            case TowerType.LIGHTNING:
                break;

            case TowerType.BOOST:
                break;
        }
    }

    void Update()
    {
        if (!active)
        {
            UpdateDropProperties();
        }
    }

    void UpdateDropProperties()
    {
        if (Input.touchCount > 0)
        {
            var inputPosition = FindObjectOfType<Camera>().ScreenToWorldPoint(Input.GetTouch(0).position);
            transform.position = new Vector3(inputPosition.x, inputPosition.y, 0.0f);
            if (PlacementContacts.Count == 0)
            {
                WeaponBaseSR.color = Color.green;
            }
            else WeaponBaseSR.color = Color.red;
        }
        else
        {
            if (PlacementContacts.Count == 0)
            {
                
                InitializeTower();
            }
            else
            {
                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlacementContacts.Contains(collision.gameObject))
        {
            PlacementContacts.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (PlacementContacts.Contains(collision.gameObject))
        {
            PlacementContacts.Remove(collision.gameObject);
        }
    }
}
