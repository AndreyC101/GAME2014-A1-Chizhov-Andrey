using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Weapon_Boost : MonoBehaviour
{
    private TowerManager tm;

    public GameObject parentTower;
    public GameObject projectilePrefab;

    private Animator AnimatedWeapon;

    private List<Tower> targets;

    private float fireRadius;

    public void Initialize()
    {
        AnimatedWeapon = GetComponentInChildren<Animator>();
        fireRadius = 5.6f;

        targets = new List<Tower>();
        AnimatedWeapon.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Sprites/Animations/Tower_Animator_Crystal_Boost");

        tm = FindObjectOfType<TowerManager>();
    }

    private void FixedUpdate()
    {
        foreach(Tower target in tm.ActiveTowers)
        {
            if ((target.transform.position - transform.position).magnitude <= fireRadius)
            {
                target.Boost();
                AnimatedWeapon.SetBool("Active", true);
            }
        }
    }
}
