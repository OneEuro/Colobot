using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Character movement variables
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float turnSpeed = 200f;
    // public float boxLiftForce = 5f;
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
    private bool isHolding; // флаг, указывающий на то, что персонаж держит объект
    public float raycastDistance = 1.5f; // дистанция для выявления объектов
    public GameObject itemToPickUp; // объект, который игрок может поднять
    public Transform holdPoint; // точка, где держится кубический объект
    public float pickUpDistance = 2f; // дистанция, на которой игрок может поднять предмет
    private GameObject itemToDrop; // объект, который игрок может опустить
    public float dropDistance = 2f; // дистанция, на которой игрок может опустить предмет
    public float dropDuration = 1.0f; // продолжительность опускания предмета
    private bool isDropping = false; // флаг, указывающий, выполняется ли сейчас опускание предмета
    public GameObject dropPosition;
    



    private GameObject hitObject;


    //Jetpack variables
    public GameObject jetpack;
    
    //Vehicle variables
    public GameObject vehicle;


    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

    }

    private IEnumerator DropItemCoroutine()
    {
        isDropping = true;

        // проигрываем анимацию опускания предмета
        anim.SetTrigger("pickUp");

        // вычисляем конечную позицию, на которой должен находиться предмет после опускания
        // Vector3 dropPosition = transform.position - transform.forward * 1.5f;

        // перемещаем объект предмета плавно вниз на конечную позицию
        float elapsed = 0.0f;
        Vector3 startPosition = itemToPickUp.transform.position;
        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            itemToPickUp.transform.position = Vector3.Lerp(startPosition, dropPosition.transform.position, elapsed / dropDuration);
            yield return null;
        }

        // отключаем коллайдер объекта для опускания, чтобы персонаж мог взаимодействовать с предметом, который он опустил
        Collider itemCollider = itemToPickUp.GetComponent<Collider>();
        if (itemCollider != null)
        
        {
            itemCollider.enabled = false;
        }
          // удаляем объект для опускания из руки персонажа
        itemToPickUp.transform.parent = null;

        isDropping = false;
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
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded && !isFlying)
        {
            anim.SetBool("isGrounded", false);
            isGrounded = false;
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        //Character landing
        // if(isGrounded)
        // {
        //     // anim.Play(jumpEnd.name);
        // }

        //Character falling
        if(transform.position.y < -2.0f || transform.position.y > 2.0f)
        {
            // anim.Play(jumpLoop.name);
            anim.SetBool("isGrounded", false);
            Debug.Log("Falling");
        }

        if (!isHolding)
        {
         // проверяем, нажата ли клавиша для поднятия предмета и есть ли объект для поднятия
        if (Input.GetKeyDown(KeyCode.E) && itemToPickUp != null)
        {
            // проверяем, находится ли объект для поднятия в заданной дистанции от игрока
            if (Vector3.Distance(transform.position, itemToPickUp.transform.position) <= pickUpDistance)
            {
                Debug.Log("PickUp box");
                  // поднимаем объект
                // itemToPickUp = hit.collider.gameObject;
                itemToPickUp.transform.position = holdPoint.position;
                itemToPickUp.transform.parent = holdPoint;
                isHolding = true;

                anim.SetTrigger("pickUp");
            }
        }
         } else {

        // проверяем, нажата ли клавиша для опускания предмета и есть ли объект для опускания
            if (Input.GetKeyDown(KeyCode.E))
            {
                    Debug.Log("Drop box");
                    StartCoroutine(DropItemCoroutine());
                    isHolding = false;
            }
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
                isGrounded = true;
                anim.SetBool("isGrounded", true);
                // Debug.Log("tag = Ground");
        }
        if(collision.gameObject.CompareTag("Box"))
        {
                isGrounded = true;
                anim.SetBool("isGrounded", true);
                // Debug.Log("tag = Box");
        }
    }

    // private void OnCollisionExit(Collision collision) {
    //     if(collision.gameObject.CompareTag("Ground"))
    //     {
    //             isGrounded = false;
    //             anim.Play(jumpLoop.name);
    //     }
    // }
   
    private void OnTriggerEnter(Collider other)
    {
        // проверяем, входит ли другой объект в триггер-коллайдер
        if (other.CompareTag("BoxTrigger"))
        {
            Debug.Log("it is BOX");
            itemToPickUp = other.gameObject; // сохраняем объект для поднятия
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // проверяем, выходит ли другой объект из триггер-коллайдера
        if (other.CompareTag("BoxTrigger"))
        {
            itemToPickUp = null; // сбрасываем объект для поднятия
        }
    }

}
