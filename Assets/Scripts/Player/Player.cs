using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Player : MonoBehaviour
{
    private float attackLife = 0.25f;
    public float damage = 3f;
    [SerializeField] private GameObject meleeCollider;
    [SerializeField] private GameObject projectile;

    public void MeleeAttack()
    {
        StartCoroutine(Wait(attackLife));
    }
    public void RangedAttack()
    {
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        Instantiate(projectile, rb.position + rb.transform.forward*2, rb.rotation);
    }
    private IEnumerator Wait(float num)
    {
        meleeCollider.GameObject().SetActive(true);
        yield return new WaitForSeconds(num);
        meleeCollider.GameObject().SetActive(false);
    }
}
