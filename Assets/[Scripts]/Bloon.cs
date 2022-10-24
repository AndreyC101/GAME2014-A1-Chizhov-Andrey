using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloon : MonoBehaviour
{
    public int level;
    private float speed;
    private int currentPointIndex;
    public Vector3 targetPoint;


    private SpriteRenderer sr;
    private EnemyManager em;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int level, float speed, Vector3 targetPoint, EnemyManager manager)
    {
        this.level = level;
        this.speed = speed;
        currentPointIndex = 0;
        this.targetPoint = targetPoint;
        em = manager;
        SetSpriteByLevel();
    }

    private void SetSpriteByLevel()
    {
        sr.sprite = em.BloonSprites[level];
    }


    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 direction = targetPoint - transform.position;
        transform.position += Vector3.Normalize(direction) * speed * Time.fixedDeltaTime;

        float distanceFromTarget = direction.magnitude;
        if (distanceFromTarget < 0.2f)
        {
            currentPointIndex++;
            if (currentPointIndex == 41)
            {
                em.DestroyBloon(this.gameObject, true);
            }
            else
            {
                targetPoint = em.BloonPath[currentPointIndex];
            }
        }
    }

    public void Pop()
    {
        level--;
        if (level < 0)
        {
            em.DestroyBloon(this.gameObject, false);
        }
        else
        {
            SetSpriteByLevel();
        }
    }
}
