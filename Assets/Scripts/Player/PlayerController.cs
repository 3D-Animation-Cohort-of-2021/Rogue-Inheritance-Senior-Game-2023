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
    [SerializeField] private LayerMask mouseLayer;
    private Vector2 move;
    private Vector3 look, direction;
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
        Mouse = false;
    }
    
    public void SetMouseLookAxis(Vector2 axis)
    {
        //Vector3 mousepos = axis;
        //mousepos.z = 100f;
        //mousepos = cam.ScreenToWorldPoint(mousepos);
        Ray ray = cam.ScreenPointToRay(axis);
        //Debug.DrawRay(cam.transform.position, mousepos-transform.position, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mouseLayer))
        {
            look = hit.point;
        }
        Mouse = true;
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
            
            if (Mouse)
            {
                look.y = rb.position.y;
                direction = look - rb.position;
                rb.rotation = Quaternion.LookRotation(direction);
            }
            else  rb.rotation = Quaternion.LookRotation(lookAtPos);
        }
    }
}
