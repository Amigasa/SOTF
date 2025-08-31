using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    public Transform target;//ќбъект за которым будет следовать камера

    public float minX, maxX;//ћаксимальное и минимальное положение камеры по горизонтали
    public float minY, maxY;//ћаксимальное и минимальное положение камеры по вектикали

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minY, maxY), transform.position.z); //функци€ котора€ останавливает камеру в максимальных и минимальных положени€х
    }
}
