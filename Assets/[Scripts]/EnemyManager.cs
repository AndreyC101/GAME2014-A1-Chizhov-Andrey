using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyManager : MonoBehaviour
{
    private GameSystemsManager gsm;
    private TowerManager tm;

    public GameObject BloonPrefab;

    public Sprite[] BloonSprites;

    public float[] BloonSpeeds = new float[5];

    public Vector3[] BloonPath;

    public List<GameObject> Bloons = new List<GameObject>();

    public void Initialize()
    {
        gsm = FindObjectOfType<GameSystemsManager>();
        tm = FindObjectOfType<TowerManager>();
        BloonPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        BloonPath = new Vector3[41];
        for (int i = 0; i < 41; i++)
        {
            BloonPath[i] = GameObject.Find($"Path_Point_{i}").transform.position;
        }
        DestroyAllBloons();
    }

    private GameObject CreateBloon(int level)
    {
        GameObject bloon = Instantiate<GameObject>(BloonPrefab, BloonPath[0], Quaternion.identity, GetComponent<Transform>());
        if (bloon.GetComponent<Bloon>())
        {
            bloon.GetComponent<Bloon>().Initialize(level, BloonSpeeds[level], BloonPath[1], this);
        }
        else
        {
            Debug.Log("No bloon component found on bloon");
            bloon.AddComponent<Bloon>();
            bloon.GetComponent<Bloon>().Initialize(level, BloonSpeeds[level], BloonPath[1], this);
        }
        Bloons.Add(bloon);
        return bloon;
    }

    public void DestroyBloon(GameObject bloon, bool gotThrough)
    {
        int bloonLevel = bloon.GetComponent<Bloon>().level;
        if (Bloons.Contains(bloon))
        {
            Bloons.Remove(bloon);
            tm.OnBloonDestoyed(bloon.GetComponent<Bloon>());
            Destroy(bloon);
        }
        if (gotThrough) gsm.OnBloonGotThrough(bloonLevel);
        else gsm.OnBloonPopped();
    }

    public void DestroyAllBloons()
    {
        foreach (GameObject bloon in Bloons)
        {
            Destroy(bloon);
        }
        Bloons.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateBloon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CreateBloon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CreateBloon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CreateBloon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CreateBloon(4);
        }
    }

    public void WaveStart(int waveIndex)
    {

    }
}
