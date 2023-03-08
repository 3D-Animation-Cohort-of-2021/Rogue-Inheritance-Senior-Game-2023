using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 10;

    private void TakeDamage(float dmg)
    {
        if (health - dmg <= 0)
        {
            print("Enemy is Dead");
        }
        else
        {
            health -= dmg;
            print("Health is " + health + " after taking " + dmg +" damage");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            TakeDamage(other.GetComponentInParent<Player>().damage);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TestProjectile>())
        {
            TakeDamage(2);
        }
    }
}
