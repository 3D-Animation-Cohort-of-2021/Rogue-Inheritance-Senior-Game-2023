//Created by: DJ Swiggett
//Last edited by: 
//Purpose: This script controls the players movement

using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector2 direction,move,look;
    private float speed = 8f;
    private Rigidbody rb;
    private bool canMove = true;
    private bool Mouse;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    public void SetMoveAxis(Vector2 axis)
    {
        move = axis;
    }

    public void SetLookAxis(Vector2 axis)
    {
        look = axis;
       // Mouse = false;
    }
    
    public void SetMouseLookAxis(Vector2 axis)
    {
        //var ray = cam.ScreenPointToRay(axis);
        //if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, default))
        //{
            //look = hitInfo.point;
        //}
        look = axis;
        //Mouse = true;
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
        if (canMove && move != Vector2.zero)
        {
            rb.AddForce(new Vector3(move.x,0, move.y).normalized*30, ForceMode.Impulse);
            StartCoroutine(Wait(0.55f));
            anim.SetTrigger("Dash");
        }
    }
    

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        Vector3 lookAtPos = new Vector3(look.x, 0, look.y);
        if (canMove)
        {
            rb.velocity = new Vector3(move.x, rb.velocity.y, move.y).normalized * speed;
        }
        if (lookAtPos != Vector3.zero)
        {
            /*
            if (Mouse)
            {
                direction = look - new Vector2(rb.position.x,rb.position.y);
                rb.rotation = Quaternion.LookRotation(direction);
                print("Mouse");
            }
            else */ rb.rotation = Quaternion.LookRotation(lookAtPos);
        }
    }
}
