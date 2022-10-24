using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TowerType
{
    BALLISTA = 0,
    SPREADSHOT = 1,
    ICE = 2,
    LIGHTNING = 3,
    BOOST = 4,
    TOWER_TYPE_COUNT = 5
}

public class Tower : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
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
        WeaponTransform = transform.GetChild(0).transform;
        PlacementCollider = GetComponent<BoxCollider2D>();
        active = false;
        WeaponBaseSR.color = Color.red;
    }

    private void InitializeTower()
    {
        WeaponBaseSR.color = Color.white;
        InitializeWeapon();
        active = true;
    }

    private void InitializeWeapon()
    {
        switch (type)
        {
            case TowerType.BALLISTA:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
                WeaponTransform.gameObject.AddComponent<Ballista_Weapon>();
                WeaponTransform.gameObject.GetComponent<Ballista_Weapon>().Initialize();
                break;

            case TowerType.SPREADSHOT:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 0.8f, 0.0f);
                WeaponTransform.gameObject.AddComponent<Spreadshot_Weapon>();
                WeaponTransform.gameObject.GetComponent<Spreadshot_Weapon>().Initialize();
                break;

            case TowerType.ICE:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 0.9f, 0.0f);
                break;

            case TowerType.LIGHTNING:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 1.1f, 0.0f);
                break;

            case TowerType.BOOST:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 1.2f, 0.0f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PlacementContacts.Contains(collision.gameObject) && !collision.transform.tag.Contains("TowerWeapon"))
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
    public void OnPointerDown(PointerEventData data)
    {
        if (!active)
        {
            Debug.Log("begin tower drag");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!active)
        {
            Vector3 worldPosition = FindObjectOfType<Camera>().ScreenToWorldPoint(eventData.position);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, 0.0f);
            if (PlacementContacts.Count == 0)
            {
                WeaponBaseSR.color = Color.green;
            }
            else WeaponBaseSR.color = Color.red;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!active)
        {
            if (PlacementContacts.Count == 0)
            {
                InitializeTower();
            }
        }
    }

    public void RemoveBloon(Bloon bloon)
    {
        switch (type)
        {
            case TowerType.BALLISTA:
                WeaponTransform.gameObject.GetComponent<Ballista_Weapon>().RemoveBloonTarget(bloon);
                break;

            case TowerType.SPREADSHOT:
                WeaponTransform.gameObject.GetComponent<Spreadshot_Weapon>().RemoveBloonTarget(bloon);
                break;

            case TowerType.ICE:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 0.9f, 0.0f);
                break;

            case TowerType.LIGHTNING:
                WeaponTransform.position = transform.position + new Vector3(0.0f, 1.1f, 0.0f);
                break;

            default:
                break;
        }
    }
}
