using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
   public float speed = 10f;
    public float rotationSpeed = 50f;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check if in vehicle control mode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetComponent<CharacterController>().enabled = false;
            enabled = true;
        }

        // Get the input axes for movement and rotation
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float rotateY = Input.GetAxis("Mouse X");

         // Move the vehicle
        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        movement = movement.normalized * speed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + transform.TransformDirection(movement));

        // Rotate the vehicle
        transform.Rotate(Vector3.up * rotateY * rotationSpeed * Time.deltaTime);

        // Check if in third person control mode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GetComponent<CharacterController>().enabled = true;
            enabled = false;
        }
    }
}
