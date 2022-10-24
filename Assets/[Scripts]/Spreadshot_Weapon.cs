using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spreadshot_Weapon : MonoBehaviour
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
        fireDelay = 1.2f;
        fireRadius = 1.85f;
        effectRadius.radius = fireRadius;
        effectRadius.isTrigger = true;

        boostedFireDelay = fireDelay * 0.7f;
        boostedFireRadius = fireRadius * 1.3f;

        targets = new List<Bloon>();
        AnimatedWeapon.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Sprites/Animations/Spreadshot_Animator");

        tm = FindObjectOfType<TowerManager>();
    }

    private void FixedUpdate()
    {
        DumpNullTargets();
        if (targets.Count > 0 && targets[0] != null)
        {
            projectileDirection = Vector3.Normalize(targets[0].transform.position - transform.position);
            float rotationAngle = Vector3.SignedAngle(new Vector3(0.0f, 1.0f, 0.0f), projectileDirection, new Vector3(0.0f, 0.0f, 1.0f));
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
        for (int i = -20; i < 41; i += 10)
        {
            GameObject projectile = Instantiate<GameObject>(tm.ProjectilePrefab, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAngle + i)), this.transform);
            Vector3 currentProjectileDirection = Quaternion.Euler(0.0f, 0.0f, rotationAngle + i) * projectileDirection;
            projectile.GetComponent<Projectile>().Initialize(currentProjectileDirection, 12.0f, isBoosted ? boostedFireRadius : fireRadius);
        }
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
