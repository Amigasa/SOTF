using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
public class Anim : MonoBehaviour
{
    Rigidbody2D body; // переменна€ дл€ хранени€ тела персонажа
    float axis; // переменна€ дл€ хранени€ состо€ни€ ќси
    Vector3 size; // переменна€ дл€ хранени€ размера персонажа

    bool isJump;
    bool RunnP;
    public Animator animator;
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // получаем тело
        size = gameObject.transform.localScale; // получаем размер
        isJump = false;
        RunnP = false;
    }
    void Update()
    {
        axis = Input.GetAxisRaw("Horizontal"); // получаем состо€ние ќси 
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
        // прыжок
        if (Input.GetButtonDown("Jump") && isJump == false)
        {
            isJump = true;
            animator.SetBool("Jumpp", isJump);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//ѕровер€ем, столкнулись ли мы с землей
        {
            isJump = false;
            animator.SetBool("Jumpp", isJump);
        }
    }
    private void FixedUpdate()
    {
        // перемещение вправо/влево
        if (Input.GetButton("Horizontal"))
        {

        }
    }
}
