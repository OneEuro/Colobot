using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Character movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float turnSpeed = 200f;
    public float boxLiftForce = 5f;
    public bool isGrounded = false;
    public bool isFlying = false;
    public bool isLiftingBox = false;

    private Rigidbody rig;
    
    //Character animation variables
    public Animator anim;
    public AnimationClip jumpStart;
    public AnimationClip jumpLoop;
    public AnimationClip jumpEnd;
    public AnimationClip idle;
    public AnimationClip run;
    public AnimationClip pickUp;
    public AnimationClip walk;
    
    //Box lifting variables
    public GameObject box;
    public Transform boxLiftPosition;
    
    //Jetpack variables
    public GameObject jetpack;
    
    //Vehicle variables
    public GameObject vehicle;

    private Vector3 moveDirection = Vector3.zero; // Направление движения персонажа

    
    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

    }

    void Update()
    {
        //Character movement
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(hAxis, 0f, vAxis) * moveSpeed * Time.deltaTime;
        movement.Normalize();
        anim.SetFloat("movementSpeed", Mathf.Abs(hAxis) + Mathf.Abs(vAxis) );
        
        //Character rotation
        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            rig.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
        }
        
        //Character jumping
        if(Input.GetKeyDown(KeyCode.Space) /*&& isGrounded*/ && !isFlying)
        {
            anim.SetBool("isGrounded", false);
            // isGrounded = false;
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        //Character landing
        // if(isGrounded)
        // {
        //     // anim.Play(jumpEnd.name);
        // }

        //Character falling
        if(transform.position.y < -2.0f)
        {
            anim.Play(jumpLoop.name);
            Debug.Log("Falling");
        }
        
        //Character box lifting
        if(Input.GetKeyDown(KeyCode.E) && !isFlying)
        {
            isLiftingBox = true;
            box.transform.position = boxLiftPosition.position;
            box.transform.parent = boxLiftPosition;
            anim.Play(pickUp.name);
        }
        else if(Input.GetKeyUp(KeyCode.E))
        {
            isLiftingBox = false;
            box.transform.parent = null;
        }
        
        //Character jetpack flying
        if(Input.GetKey(KeyCode.J) && isFlying)
        {
            rig.AddForce(Vector3.up * jetpack.GetComponent<Jetpack>().jetpackForce, ForceMode.Impulse);
        }
        
        //Switch to vehicle
        if(Input.GetKeyDown(KeyCode.V) && !isFlying)
        {
            vehicle.SetActive(true);
            gameObject.SetActive(false);
        }
        
        //Character animation
        if(isGrounded)
        {
            if(movement.magnitude == 0f && !isLiftingBox)
            {
                // anim.Play(idle.name);
            }
            else if(movement.magnitude > 0f && !isLiftingBox)
            {
                // anim.Play(run.name);
            }
            else if(isLiftingBox)
            {
                anim.Play(pickUp.name);
            }
        }
        else if(!isGrounded)
        {
            // anim.Play(jumpLoop.name);
        }
    }
    
    //Character collision with other objects
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
                // isGrounded = true;
                anim.SetBool("isGrounded", true);
                Debug.Log("tag = Ground");
        }
        if(collision.gameObject.CompareTag("Box"))
        {
                // isGrounded = true;
                anim.SetBool("isGrounded", true);
                Debug.Log("tag = Box");
        }
    }

    // private void OnCollisionExit(Collision collision) {
    //     if(collision.gameObject.CompareTag("Ground"))
    //     {
    //             isGrounded = false;
    //             anim.Play(jumpLoop.name);
    //     }
    // }

    //  void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, 0.2f);
    // }

}
