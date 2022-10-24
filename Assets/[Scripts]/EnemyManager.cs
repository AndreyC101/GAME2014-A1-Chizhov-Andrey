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

    public int currentWave;
    private int[] bloonsInWave = new int[5];
    private float bloonSpawnDelay;
    private float timeSinceLastBloonSpawn;
    private bool waveInProgress;

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
        for (int i = 0; i < 5; i++)
        {
            bloonsInWave[i] = 0;
        }
        currentWave = 0;
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
        timeSinceLastBloonSpawn += Time.deltaTime;
        if (waveInProgress && timeSinceLastBloonSpawn > bloonSpawnDelay)
        {
            timeSinceLastBloonSpawn = 0.0f;
            for (int i = 0; i < 5; i++)
            {
                if (bloonsInWave[i] > 0)
                {
                    CreateBloon(i);
                    bloonsInWave[i] -= 1;
                    return;
                }
            }
            WaveEnd();
        }
    }

    public void WaveStart()
    {
        currentWave++;
        timeSinceLastBloonSpawn = 0.0f;
        InitializeWave();
        waveInProgress = true;
    }

    private void InitializeWave()
    {
        bloonsInWave[0] = (currentWave > 50 ? Mathf.Max(60 - (int)(currentWave * 0.2f), 0) : (int)(-0.012f * Mathf.Pow((currentWave - 50), 2) + 60));
        bloonsInWave[1] = currentWave > 40 ? Mathf.Max(70 - (int)(currentWave * 0.2f), 0) : (int)(-0.049f * Mathf.Pow((currentWave - 40), 2) + 70);
        bloonsInWave[2] = currentWave > 50 ? Mathf.Max(80 - (int)(currentWave * 0.2f), 0) : (int)(-0.04f * Mathf.Pow((currentWave - 50), 2) + 80);
        bloonsInWave[3] = currentWave > 80 ? Mathf.Max(90 - (int)(currentWave * 0.2f), 0) : (int)(-0.017f * Mathf.Pow((currentWave - 80), 2) + 90);
        bloonsInWave[4] = currentWave > 100 ? 100 + (int)(currentWave * 1.2f) : (int)(-0.015f * Mathf.Pow((currentWave - 100), 2) + 100);

        bloonSpawnDelay = 0.65f - (currentWave * 0.04f);
        if (bloonSpawnDelay < 0.15f) bloonSpawnDelay = 0.15f;
    }

    public void WaveEnd()
    {
        waveInProgress = false;
        if (gsm.gameInProgress) gsm.RewardPlayerMoney(100 + currentWave);
        FindObjectOfType<UIManager>().OnWaveEnd();
    }
}
