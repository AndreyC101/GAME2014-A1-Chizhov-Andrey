using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerManager : MonoBehaviour
{
    private GameSystemsManager gsm;

    [SerializeField]
    private GameObject TowerPrefab;

    public GameObject ProjectilePrefab;

    public int[] TowerCosts;

    public Sprite[] TowerBaseSprites;

    public List<Tower> ActiveTowers = new List<Tower>();

    public void Awake()
    {
        gsm = FindObjectOfType<GameSystemsManager>();
        TowerPrefab = Resources.Load<GameObject>("Prefabs/Tower");
        ProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectile");
    }

    public void CreateDropTower(TowerType type, Vector3 position)
    {
        var inputPosition = position;
        inputPosition.z = 0.0f;
        GameObject newTower = Instantiate<GameObject>(TowerPrefab, inputPosition, Quaternion.identity, this.transform);
        newTower.GetComponent<Tower>().InitializeDropTower(type);
        newTower.GetComponent<SpriteRenderer>().sprite = TowerBaseSprites[(int)type];
    }

    public void ActivateTower(Tower newTower)
    {
        if (!ActiveTowers.Contains(newTower))
        {
            ActiveTowers.Add(newTower);
        }
    }

    public void DestroyAllTowers()
    {
        foreach (Tower tower in ActiveTowers)
        {
            Destroy(tower.gameObject);
        }
        ActiveTowers.Clear();
    }

    public void OnBloonDestoyed(Bloon bloon)
    {
        foreach(Tower tower in ActiveTowers)
        {
            tower.RemoveBloon(bloon);
        }
    }
}
