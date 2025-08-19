using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Anim : MonoBehaviour
{
    Rigidbody2D body; // переменная для хранения тела персонажа
    float axis; // переменная для хранения состояния Оси
    Vector3 size; // переменная для хранения размера персонажа
    [SerializeField] private GameObject NaPanel;

    bool isJump;
    public Animator animator;
    bool AAA;
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // получаем тело
        size = gameObject.transform.localScale; // получаем размер
        isJump = false;
        AAA = false;
    }
    void Update()
    {
        axis = Input.GetAxisRaw("Horizontal"); // получаем состояние Оси 
        // прыжок
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
        if (collision.gameObject.tag == "Ground")//Проверяем, столкнулись ли мы с землей
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
