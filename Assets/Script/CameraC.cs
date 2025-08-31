using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraC : MonoBehaviour
{
    public Transform target;//������ �� ������� ����� ��������� ������

    public float minX, maxX;//������������ � ����������� ��������� ������ �� �����������
    public float minY, maxY;//������������ � ����������� ��������� ������ �� ���������

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minY, maxY), transform.position.z); //������� ������� ������������� ������ � ������������ � ����������� ����������
    }
}
