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
    public float movementSpeed = 2f; // Скорость перемещения персонажа
    public float rotationSpeed = 5f; // Скорость поворота персонажа

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
    public float dropDuration = 1f; // продолжительность опускания предмета
    private bool isDropping = false; // флаг, указывающий, выполняется ли сейчас опускание предмета
    public GameObject dropPosition;
    private bool isPickingUp = false; // Флаг, указывающий, выполняется ли сейчас поднятие предмета




    private GameObject hitObject;

    public Vector3 cubeSideDirection;
    public event System.Action<Vector3> OnVectorChanged;


    //Jetpack variables
    public GameObject jetpack;
    
    //Vehicle variables
    public GameObject vehicle;
    private CubeColorChanger cubeColorChanger;


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
            // itemCollider.enabled = false;
        }
          // удаляем объект для опускания из руки персонажа
        itemToPickUp.transform.parent = null;

        isDropping = false;
    }

    private IEnumerator MoveAndRotateToItem(Vector3 characterToItemDirection)
    {
        isPickingUp = true;

         // Находим ближайшую сторону куба к персонажу
        cubeSideDirection = FindClosestCubeSide(characterToItemDirection);
        if (OnVectorChanged != null) {
            OnVectorChanged.Invoke(cubeSideDirection);
        }
        
        
        // Вращаем персонаж плавно к стороне куба
        Quaternion targetRotation = Quaternion.LookRotation(cubeSideDirection);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        // Двигаем персонажа к стороне куба
        // Vector3 targetPosition = itemToPickUp.transform.position - cubeSideDirection.normalized;
        // while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
        //     yield return null;
        // }

        // Поднимаем предмет
        StartCoroutine(PickupItem());

        isPickingUp = false;
    }

    private Vector3 FindClosestCubeSide(Vector3 direction)
    {
        Vector3[] cubeSides = new Vector3[]
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
            //Vector3.up,
            //Vector3.down
        };

        float closestAngle = Mathf.Infinity;
        Vector3 closestSide = Vector3.zero;

        foreach (Vector3 side in cubeSides)
        {
            float angle = Vector3.Angle(direction, side);
            if (angle < closestAngle)
            {
                closestAngle = angle;
                closestSide = side;
            }
        }

        return closestSide;
    }

   

    private IEnumerator PickupItem()
    {
        // itemToPickUp.transform.position = holdPoint.position;
        // itemToPickUp.transform.parent = holdPoint;
        
        anim.SetTrigger("pickUp");
        float elapsed = 0.0f;
        Vector3 endPosition = itemToPickUp.transform.position;
        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            itemToPickUp.transform.position = Vector3.Lerp(endPosition, holdPoint.transform.position, elapsed / dropDuration);
            yield return null;
        }
        itemToPickUp.transform.parent = holdPoint;
        isHolding = true;
        
        // Выполняем действия по поднятию предмета
        Debug.Log("Поднят предмет: " + itemToPickUp.name);
        
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
            cubeColorChanger = itemToPickUp.GetComponent<CubeColorChanger>();
            // проверяем, находится ли объект для поднятия в заданной дистанции от игрока
            if (Vector3.Distance(transform.position, itemToPickUp.transform.position) <= pickUpDistance)
            {
            // Проверяем угол между персонажем и предметом
                Vector3 characterToItemDirection = itemToPickUp.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward, characterToItemDirection);
                StartCoroutine(MoveAndRotateToItem(characterToItemDirection));

                // Если угол не равен 90 градусам, двигаем персонажа в нужное положение и вращаем его
                // if (angle != 90f)
                // {
                //     // Quaternion targetRotation = Quaternion.LookRotation(characterToItemDirection);
                //     StartCoroutine(MoveAndRotateToItem(characterToItemDirection));
                // }
                // else
                // {
                //     // Иначе просто поднимаем предмет
                //     PickupItem();
                // }
            }
        }
        
            
         } else {

        // проверяем, нажата ли клавиша для опускания предмета и есть ли объект для опускания
            if (Input.GetKeyDown(KeyCode.E) && (anim.GetFloat("movementSpeed") < 0.01f))
            {
                    Debug.Log("Drop box");
                    StartCoroutine(DropItemCoroutine());
                    cubeColorChanger.ChangeBackColor();
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
            // gameObject.SetActive(false);
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
            itemToPickUp = other.transform.parent.gameObject; // сохраняем объект для поднятия
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
