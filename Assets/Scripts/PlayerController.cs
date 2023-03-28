using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour

{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float jetpackForce = 20f;
    public float boxLiftingDistance = 1.5f;
    public LayerMask boxLayer;

    private Rigidbody rb;
    private Animator anim;
    public bool isGrounded;
    private bool isLiftingBox;
    private GameObject box;
    private Vector3 boxOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Get input from the user
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        bool jetpack = Input.GetButton("Fire1");
        bool lift = Input.GetButtonDown("Fire2");


       
        // Calculate movement and apply it to the character
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Check if the character is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        // Apply jump force if the character is grounded and the jump button is pressed
        if (jump && isGrounded)
        {
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Apply jetpack force if the jetpack button is pressed
        if (jetpack)
        {
            rb.AddForce(Vector3.up * jetpackForce);
        }

        // Lift box if the lift button is pressed and a box is nearby
        if (lift && !isLiftingBox)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, boxLiftingDistance, boxLayer))
            {
                isLiftingBox = true;
                box = hit.collider.gameObject;
                boxOffset = box.transform.position - transform.position;
                anim.SetBool("isLifting", true);
            }
        }

        // Move the lifted box with the character
        if (isLiftingBox)
        {
            box.transform.position = transform.position + boxOffset;
        }
    }

    void LateUpdate()
    {
        // Play animations based on the character's movement
        float moveMagnitude = rb.velocity.magnitude;
        anim.SetFloat("moveSpeed", moveMagnitude);

        if (moveMagnitude > 0f)
        {
            transform.forward = rb.velocity.normalized;
        }

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a ray to show the box lifting distance
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * boxLiftingDistance);
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log("it is Collision");
    }

     private void OnTriggerEnter(Collider other) {
         Debug.Log("it is Collider");
    }

}