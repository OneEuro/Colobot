using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float rotationSpeed = 50f;

    private Animator animator;
    private Rigidbody rigidbody;

    public bool isGrounded = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get the input axes for movement and rotation
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float rotateY = Input.GetAxis("Mouse X");

        // Move the character
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        movement = movement.normalized * speed * Time.deltaTime;
        transform.Translate(movement, Space.Self);
        // rigidbody.MovePosition(transform.position + transform.TransformDirection(movement));


        // Rotate the character
        transform.Rotate(Vector3.up * rotateY * rotationSpeed * Time.deltaTime);

        // Update the animator
        animator.SetFloat("MoveSpeed", movement.magnitude);

        // Check if the character is on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetTrigger("Jump");
        }

    }

     void OnCollisionEnter(Collision collision)
            {
                Debug.Log("Collision enter");
                // Check if player has landed on ground
                if (collision.gameObject.CompareTag("Ground"))
                {
                    isGrounded = true;
                    animator.SetTrigger("land");
                }
            }

    void OnCollisionExit(Collision collision)
            {
                Debug.Log("Collision exit");
                // Check if player has left the ground
                if (collision.gameObject.CompareTag("Ground"))
                {
                    isGrounded = false;
                }
            }

}
