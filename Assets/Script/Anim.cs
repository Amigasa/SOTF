using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
public class Anim : MonoBehaviour
{
    Rigidbody2D body; // ���������� ��� �������� ���� ���������
    float axis; // ���������� ��� �������� ��������� ���
    Vector3 size; // ���������� ��� �������� ������� ���������

    bool isJump;
    bool RunnP;
    public Animator animator;
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // �������� ����
        size = gameObject.transform.localScale; // �������� ������
        isJump = false;
        RunnP = false;
    }
    void Update()
    {
        axis = Input.GetAxisRaw("Horizontal"); // �������� ��������� ��� 
        if(gameObject.transform.localScale.x > 0 && axis < 0)
        {
            RunnP = true;
            animator.SetBool("RunP", RunnP);
        }
        if (gameObject.transform.localScale.x < 0 && axis > 0)
        {
            RunnP = true;
            animator.SetBool("RunP", RunnP);
        }
        // ������
        if (Input.GetButtonDown("Jump") && isJump == false)
        {
            isJump = true;
            animator.SetBool("Jumpp", isJump);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//���������, ����������� �� �� � ������
        {
            isJump = false;
            animator.SetBool("Jumpp", isJump);
        }
    }
    private void FixedUpdate()
    {
        // ����������� ������/�����
        if (Input.GetButton("Horizontal"))
        {

        }
    }
}
