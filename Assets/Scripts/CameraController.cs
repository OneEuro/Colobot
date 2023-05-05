using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;        // Цель, за которой следит камера
    public float distance = 5.0f;   // Расстояние между камерой и целью
    public float height = 2.0f;     // Высота камеры над целью
    public float lookAtHeight = 1.5f; // Высота, на которую смотрит камера
    public float smoothSpeed = 0.5f; // Скорость, с которой движется камера
    public float rotationSpeed = 90.0f; // Скорость поворота камеры

    private float currentRotation = 0.0f; // Текущий угол поворота камеры

    // Функция LateUpdate вызывается после обновления всех объектов на сцене
    private void LateUpdate()
    {
        // Проверяем, что цель задана
        if (target != null)
        {
            // Вычисляем новый угол поворота камеры
            currentRotation += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

            // Вычисляем новую позицию камеры
            Vector3 position = target.position;
            position -= Quaternion.Euler(0.0f, currentRotation, 0.0f) * Vector3.forward * distance;
            position.y += height;

            // Перемещаем камеру
            transform.position = Vector3.Lerp(transform.position, position, smoothSpeed * Time.deltaTime);

            // Направляем камеру на цель
            transform.LookAt(target.position + Vector3.up * lookAtHeight);
        }
    }
}
