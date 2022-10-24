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

    public void Initialize()
    {
        AnimatedWeapon = GetComponentInChildren<Animator>();
        effectRadius = gameObject.AddComponent<CircleCollider2D>();
        timeSinceLastShot = 0.0f;
        fireDelay = 0.65f;
        fireRadius = 3.8f;
        effectRadius.radius = fireRadius;
        effectRadius.isTrigger = true;

        targets = new List<Bloon>();
        AnimatedWeapon.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Sprites/Animations/Ballista_Animator");

        tm = FindObjectOfType<TowerManager>();
    }

    private void Update()
    {
        if (targets.Count > 0)
        {
            projectileDirection = Vector3.Normalize(targets[0].transform.position - transform.position);
            rotationAngle = Vector3.SignedAngle(new Vector3(0.0f, 1.0f, 0.0f), projectileDirection, new Vector3(0.0f, 0.0f, 1.0f));
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAngle));
            if (timeSinceLastShot > fireDelay)
            {
                AnimatedWeapon.ResetTrigger("Attack");
                Shoot();
            }
        }
        timeSinceLastShot += Time.deltaTime;
    }

    private void Shoot()
    {
        AnimatedWeapon.SetTrigger("Attack");
        timeSinceLastShot = 0.0f;
        Debug.Log("Fire");
        GameObject projectile = Instantiate<GameObject>(tm.ProjectilePrefab, transform.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAngle)), this.transform);
        projectile.GetComponent<Projectile>().Initialize(projectileDirection, 16.0f, fireRadius);
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
        fireDelay = fireDelay * 0.7f;
        fireRadius = fireRadius * 1.3f;
    }
}
