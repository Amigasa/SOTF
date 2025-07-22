using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class Moving : MonoBehaviour
{
    Rigidbody2D body; // ���������� ��� �������� ���� ���������
    public float speed; // ���������� ��������
    float axis; // ���������� ��� �������� ��������� ���
    Vector3 size; // ���������� ��� �������� ������� ���������
    bool noAir = true; // ����������, ������� ���������� � ������� �������� ��� ���
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // �������� ����
        size = gameObject.transform.localScale; // �������� ������
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//���������, ����������� �� �� � ������
        {
            noAir = true; // ���� �����������, ����������� �� ������ (�� �� �����!)
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//���������, ���������� �� �� ������������ � ������
        {
        }
        noAir = false; // ���� ����������, ����������� �� ���� (�� �� �� �����!)
    }
    void Update()
    {
        axis = Input.GetAxis("Horizontal"); // �������� ��������� ��� 
        // ����������� ������/�����
        if (Input.GetButton("Horizontal"))
        {
            body.velocity = new Vector2(axis * speed, body.velocity.y);
        }
        // ���� ���� ������, ������������ ��������� ������
        if (axis > 0)
        {
            gameObject.transform.localScale = size;
        }
        // ���� ���� �����, ������������ ��������� �����
        if (axis < 0)
        {
            gameObject.transform.localScale = new Vector3(-size.x, size.y, size.z);
        }
        // ������
        if (Input.GetButtonDown("Jump") && noAir) // ��������: �� ����� �� ��
        {
            body.AddForce(new Vector2(0, speed / 1.0f), ForceMode2D.Impulse);
        }
    }
}
