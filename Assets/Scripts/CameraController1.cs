using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController1 : MonoBehaviour
{
    // Цель, за которой следует камера
    public Transform target;

    // Расстояние до цели
    public float distance = 10.0f;

    // Угол поворота камеры
    public float angle = 30.0f;

    // Скорость поворота камеры
    public float turnSpeed = 5.0f;

    // Минимальный и максимальный углы поворота камеры
    public float minAngle = 0.0f;
    public float maxAngle = 80.0f;

    // Ссылка на компонент Camera
    private Camera cameraComponent;

    private void Start()
    {
        // Получаем ссылку на компонент Camera
        cameraComponent = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        // Проверяем, что цель задана
        if (target != null)
        {
            // Вычисляем новый угол поворота камеры
            angle += Input.GetAxis("Mouse Y") * turnSpeed;
            angle = Mathf.Clamp(angle, minAngle, maxAngle);

            // Вычисляем новую позицию камеры
            Vector3 position = target.position - transform.forward * distance;
            position.y += Mathf.Tan(angle * Mathf.Deg2Rad) * distance;

            // Перемещаем камеру
            transform.position = position;

            // Направляем камеру на цель
            transform.LookAt(target);
        }
    }
}
