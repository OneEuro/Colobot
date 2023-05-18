using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    // public float pickUpDistance = 2f; // дистанция, на которой игрок может поднять предмет
    // public GameObject itemToPickUp; // объект, который игрок может поднять
    // public Transform holdPoint; // точка, где держится кубический объект

    // private void OnTriggerEnter(Collider other)
    // {
    //     // проверяем, входит ли другой объект в триггер-коллайдер
    //     if (other.CompareTag("Box"))
    //     {
    //         Debug.Log("it is BOX");
    //         itemToPickUp = other.gameObject; // сохраняем объект для поднятия
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     // проверяем, выходит ли другой объект из триггер-коллайдера
    //     if (other.CompareTag("Box"))
    //     {
    //         itemToPickUp = null; // сбрасываем объект для поднятия
    //     }
    // }

    // private void Update()
    // {
    //     // проверяем, нажата ли клавиша для поднятия предмета и есть ли объект для поднятия
    //     if (Input.GetKeyDown(KeyCode.E) && itemToPickUp != null)
    //     {
    //         // проверяем, находится ли объект для поднятия в заданной дистанции от игрока
    //         if (Vector3.Distance(transform.position, itemToPickUp.transform.position) <= pickUpDistance)
    //         {
    //               // поднимаем объект
    //             itemToPickUp = hit.collider.gameObject;
    //             itemToPickUp.transform.position = holdPoint.position;
    //             itemToPickUp.transform.parent = holdPoint;
    //             // heldObject.GetComponent<Rigidbody>().isKinematic = true;
    //             isHolding = true;

    //             anim.SetTrigger("PickUp");
    //         }
    //     }
    // }
}
