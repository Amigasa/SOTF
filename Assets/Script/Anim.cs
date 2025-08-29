using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Anim : Sound
{
    Rigidbody2D body; // ���������� ��� �������� ���� ���������
    float axis; // ���������� ��� �������� ��������� ���
    Vector3 size; // ���������� ��� �������� ������� ���������

    bool isJump;//���������� ��� �������� ������
    public Animator animator;//��������
    void Start()//��� ������� ����
    {
        body = GetComponent<Rigidbody2D>(); // �������� ����
        size = gameObject.transform.localScale; // �������� ������
        isJump = false;//���������� �����������
    }
    void Update()//������ ����
    {
        axis = Input.GetAxisRaw("Horizontal"); // �������� ��������� ��� 
        if (Input.GetButtonDown("Jump") && isJump == false)//���� ������ ������ ������ � �������� ���������
        {
            isJump = true;//���������� ��� �������� ����������
            animator.SetBool("Jumpp", isJump);//� � ��������� �������� Jumpp ���������� true ��� ������������� ��������
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//������� ��� �������������� ��������
    {
        if (collision.gameObject.tag == "Ground")//���������, ����������� �� �� � ������
        {
            isJump = false;//�������� �����������
            animator.SetBool("Jumpp", isJump);//�������� ��� ��� � ��������
            PlaySound(sounds[0]);//���� �����������
        }
    }
    void FixedUpdate()//��� ����
    {
        if (Input.GetButton("Horizontal"))//���� ����� ��������
        {
            animator.SetFloat("Speed", 1f);//Speed ���������� 1
        }
        else//�����
        {
            animator.SetFloat("Speed", 0f);//���������� 0
        }
    }
}
