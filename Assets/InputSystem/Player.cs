using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Player : MonoBehaviour
{
    private float attackLife = 0.25f;
    public float damage = 2f;
    [SerializeField] private GameObject meleeCollider;

    public void MeleeAttack()
    {
        StartCoroutine(Wait(attackLife));
    }
    public void RangedAttack()
    {
        
    }
    private IEnumerator Wait(float num)
    {
        meleeCollider.GameObject().SetActive(true);
        yield return new WaitForSeconds(num);
        meleeCollider.GameObject().SetActive(false);
    }
}
