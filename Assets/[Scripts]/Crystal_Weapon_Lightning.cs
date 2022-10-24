using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Weapon_Lightning : MonoBehaviour
{
    private TowerManager tm;

    public GameObject parentTower;
    public GameObject projectilePrefab;

    private Animator AnimatedWeapon;

    CircleCollider2D effectRadius;
    private List<Bloon> targets;

    private float timeSinceLastShot;
    private float fireDelay;
    private float fireRadius;

    private bool isBoosted = false;
    private float boostedFireDelay;
    private float boostedFireRadius;

    public void Initialize()
    {
        AnimatedWeapon = GetComponentInChildren<Animator>();
        effectRadius = gameObject.AddComponent<CircleCollider2D>();
        timeSinceLastShot = 0.0f;
        fireDelay = 1.85f;
        fireRadius = 3.4f;
        effectRadius.radius = fireRadius;
        effectRadius.isTrigger = true;

        boostedFireDelay = fireDelay * 0.7f;
        boostedFireRadius = fireRadius * 1.3f;

        targets = new List<Bloon>();
        AnimatedWeapon.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Sprites/Animations/Tower_Animator_Crystal_Lightning");

        tm = FindObjectOfType<TowerManager>();
    }

    private void FixedUpdate()
    {
        timeSinceLastShot += Time.fixedDeltaTime;
        if (targets.Count > 0)
        {
            if (timeSinceLastShot > (isBoosted ? boostedFireDelay : fireDelay))
            {
                AnimatedWeapon.ResetTrigger("Attack");
                AnimatedWeapon.SetTrigger("Attack");
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i])
                    {
                        targets[i].Pop();
                    }
                }
                timeSinceLastShot = 0.0f;
            }
        }
        effectRadius.radius = isBoosted ? boostedFireRadius : fireRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bloon>() && !targets.Contains(collision.GetComponent<Bloon>()))
        {
            targets.Add(collision.GetComponent<Bloon>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Bloon>() && targets.Contains(collision.GetComponent<Bloon>()))
        {
            targets.Remove(collision.GetComponent<Bloon>());
        }
    }

    public void RemoveBloonTarget(Bloon bloon)
    {
        if (targets.Contains(bloon))
        {
            targets.Remove(bloon);
        }
    }

    public void Boost()
    {
        isBoosted = true;
    }
}
