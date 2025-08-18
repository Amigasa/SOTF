using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Anim : MonoBehaviour
{
    Rigidbody2D body; // ���������� ��� �������� ���� ���������
    float axis; // ���������� ��� �������� ��������� ���
    Vector3 size; // ���������� ��� �������� ������� ���������

    bool isJump;
    bool RunnP;
    bool isRunning;
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
        // ������
        if (Input.GetButtonDown("Jump") && isJump == false)
        {
            isJump = true;
            animator.SetBool("Jumpp", isJump);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
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
    void FixedUpdate()
    {
        if (gameObject.transform.localScale.x > 0 && axis < 0)
        {
            RunnP = true;
            animator.SetBool("RunP", RunnP);
        }
        else if (gameObject.transform.localScale.x < 0 && axis > 0)
        {
            RunnP = true;
            animator.SetBool("RunP", RunnP);
        }
        else if (Input.GetButton("Horizontal"))
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            RunnP = false;
        }
    }
}
