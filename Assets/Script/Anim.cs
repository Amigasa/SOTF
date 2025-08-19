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
    [SerializeField] private GameObject NaPanel;

    bool isJump;
    public Animator animator;
    bool AAA;
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // �������� ����
        size = gameObject.transform.localScale; // �������� ������
        isJump = false;
        AAA = false;
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
            NaPanel.SetActive(!AAA);
            AAA = !AAA;
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
        if (Input.GetButton("Horizontal"))
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }
}
