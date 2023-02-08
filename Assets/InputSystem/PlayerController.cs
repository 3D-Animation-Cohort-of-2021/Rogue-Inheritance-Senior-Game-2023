using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private float verticalMove, horizontalMove;
    private Vector2 direction;
    private float speed = 6;
    private Rigidbody rb;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetMoveAxis(Vector2 axis)
    {
        verticalMove = axis.y;
        horizontalMove = axis.x;
    }

    private IEnumerator Wait(float num)
    {
        canMove = false;
        GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(num);
        canMove = true;
        GetComponentInChildren<ParticleSystem>().Stop();
    }
    public void Dash()
    {
        rb.AddForce(new Vector3(horizontalMove,0, verticalMove).normalized*30, ForceMode.Impulse);
        StartCoroutine(Wait(0.55f));
    }
    

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = new Vector3(horizontalMove, rb.velocity.y, verticalMove).normalized * speed;
        }
    }
}
