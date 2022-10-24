using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista_Weapon : MonoBehaviour
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

    private float rotationAngle;
    private Vector3 projectileDirection;

    private bool isBoosted = false;
    private float boostedFireDelay;
    private float boostedFireRadius;

    public void Initialize()
    {
        AnimatedWeapon = GetComponentInChildren<Animator>();
        effectRadius = gameObject.AddComponent<CircleCollider2D>();
        timeSinceLastShot = 0.0f;
        fireDelay = 0.7f;
        fireRadius = 2.8f;
        effectRadius.radius = fireRadius;
        effectRadius.isTrigger = true;

        targets = new List<Bloon>();
        AnimatedWeapon.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Sprites/Animations/Ballista_Animator");

        tm = FindObjectOfType<TowerManager>();

        boostedFireDelay = fireDelay * 0.7f;
        boostedFireRadius = fireRadius * 1.3f;
    }

    private void FixedUpdate()
    {
        DumpNullTargets();
        if (targets.Count > 0 && targets[0] != null)
        {
            projectileDirection = Vector3.Normalize(targets[0].transform.position - transform.position);
            rotationAngle = Vector3.SignedAngle(new Vector3(0.0f, 1.0f, 0.0f), projectileDirection, new Vector3(0.0f, 0.0f, 1.0f));
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAngle));
            if (timeSinceLastShot > (isBoosted ? boostedFireDelay : fireDelay))
            {
                AnimatedWeapon.ResetTrigger("Attack");
                Shoot();
            }
        }
        timeSinceLastShot += Time.fixedDeltaTime;
        effectRadius.radius = isBoosted ? boostedFireRadius : fireRadius;
    }

    private void Shoot()
    {
        AnimatedWeapon.SetTrigger("Attack");
        timeSinceLastShot = 0.0f;
        GameObject projectile = Instantiate<GameObject>(tm.ProjectilePrefab, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAngle)), this.transform);
        projectile.GetComponent<Projectile>().Initialize(projectileDirection, 16.0f, isBoosted ? boostedFireRadius : fireRadius);
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

    private void DumpNullTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
            {
                targets.Remove(targets[i]);
            }
        }
    }
}
