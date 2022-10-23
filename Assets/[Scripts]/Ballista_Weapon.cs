using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista_Weapon : MonoBehaviour
{
    public GameObject parentTower;
    public GameObject projectilePrefab;

    private Animator AnimatedWeapon;

    CircleCollider2D effectRadius;
    private List<Bloon> targets;

    private float fireDelay;
    private float fireRadius;

    public void Initialize()
    {
        AnimatedWeapon = GetComponentInChildren<Animator>();
        effectRadius = GetComponent<CircleCollider2D>();
        fireDelay = 0.6f;
        fireRadius = 3.8f;
        effectRadius.radius = fireRadius;
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            transform.LookAt(targets[0].transform);
        }
    }

    private void Shoot()
    {
        AnimatedWeapon.SetTrigger(0);
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
}
