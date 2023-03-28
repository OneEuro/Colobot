using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
     public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    public Animator anim;
    public GameObject landingEffect;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Check if the character is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Get input from the mobile device
        float moveInput = Input.GetAxis("Horizontal");

        // Move the character in the desired direction
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.fixedDeltaTime);

        // Update the animation
        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        // Jump if the jump button is pressed and the character is grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("collision enter");
            Instantiate(landingEffect, transform.position, Quaternion.identity);
            anim.SetTrigger("Land");
        }
    }

    void OnCollisionStay() {
        Debug.Log("collision stay");
    }
}

