using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Weapon_Ice : MonoBehaviour
{
    private TowerManager tm;

    public GameObject parentTower;
    public GameObject projectilePrefab;

    private Animator AnimatedWeapon;

    CircleCollider2D effectRadius;
    private List<Bloon> targets;

    private float fireRadius;

    private bool isBoosted = false;
    private float boostedFireRadius;

    public void Initialize()
    {
        AnimatedWeapon = GetComponentInChildren<Animator>();
        effectRadius = gameObject.AddComponent<CircleCollider2D>();
        fireRadius = 3.9f;
        effectRadius.radius = fireRadius;
        effectRadius.isTrigger = true;

        targets = new List<Bloon>();
        AnimatedWeapon.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Sprites/Animations/Tower_Animator_Crystal_Ice");

        tm = FindObjectOfType<TowerManager>();

        boostedFireRadius = fireRadius * 1.3f;
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            AnimatedWeapon.SetBool("Active", true);
        }
        else
        {
            AnimatedWeapon.SetBool("Active", false);
        }
        effectRadius.radius = isBoosted ? boostedFireRadius : fireRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bloon>() && !targets.Contains(collision.GetComponent<Bloon>()))
        {
            collision.GetComponent<Bloon>().ApplySlow();
            targets.Add(collision.GetComponent<Bloon>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Bloon>() && targets.Contains(collision.GetComponent<Bloon>()))
        {
            collision.GetComponent<Bloon>().SlowExpired();
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
