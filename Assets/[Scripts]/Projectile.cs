using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float range;

    private Vector3 Direction;

    private float speed;

    private float totalDistanceTravelled;

    public void Initialize(Vector3 direction, float speed, float range)
    {
        Direction = direction;
        this.speed = speed;
        this.range = range;
        float rotationAngle = Vector3.SignedAngle(new Vector3(0.0f, 1.0f, 0.0f), Direction, new Vector3(0.0f, 0.0f, 1.0f));
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, rotationAngle));
    }

    private void Update()
    {
        if (totalDistanceTravelled > range)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 deltaPosition = Direction * speed * Time.deltaTime;
            transform.position += deltaPosition;
            totalDistanceTravelled += deltaPosition.magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bloon targetHit = collision.GetComponent<Bloon>();
        if (targetHit)
        {
            targetHit.Pop();
            Destroy(this.gameObject);
        }
    }
}
