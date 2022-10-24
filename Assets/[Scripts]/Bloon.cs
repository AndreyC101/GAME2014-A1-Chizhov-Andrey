using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloon : MonoBehaviour
{
    public int level;
    private float speed;
    private float slowedSpeed;
    private int currentPointIndex;
    public Vector3 targetPoint;


    private SpriteRenderer sr;
    private EnemyManager em;

    private bool isSlowed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int level, float speed, Vector3 targetPoint, EnemyManager manager)
    {
        this.level = level;
        this.speed = speed;
        slowedSpeed = this.speed * 0.65f;
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
        transform.position += Vector3.Normalize(direction) * (isSlowed ? slowedSpeed : speed) * Time.fixedDeltaTime;

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

    public void ApplySlow()
    {
        isSlowed = true;
    }

    public void SlowExpired()
    {
        isSlowed = false;
    }
}
