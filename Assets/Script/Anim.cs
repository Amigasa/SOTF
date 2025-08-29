using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Anim : Sound
{
    Rigidbody2D body; // переменная для хранения тела персонажа
    float axis; // переменная для хранения состояния Оси
    Vector3 size; // переменная для хранения размера персонажа

    bool isJump;//переменная для анимации прыжка
    public Animator animator;//аниматор
    void Start()//при запуске игры
    {
        body = GetComponent<Rigidbody2D>(); // получаем тело
        size = gameObject.transform.localScale; // получаем размер
        isJump = false;//переменная отключается
    }
    void Update()//каждый кадр
    {
        axis = Input.GetAxisRaw("Horizontal"); // получаем состояние Оси 
        if (Input.GetButtonDown("Jump") && isJump == false)//если нажата кнопка прыжка и анимация выключена
        {
            isJump = true;//переменная для анимации включается
            animator.SetBool("Jumpp", isJump);//и в аниматоре параметр Jumpp становится true что воспроизводит анимацию
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)//функция для взаимодействия объектов
    {
        if (collision.gameObject.tag == "Ground")//Проверяем, столкнулись ли мы с землей
        {
            isJump = false;//анимация выключается
            animator.SetBool("Jumpp", isJump);//передает все это в аниматор
            PlaySound(sounds[0]);//звук приземления
        }
    }
    void FixedUpdate()//для бега
    {
        if (Input.GetButton("Horizontal"))//если игрок движется
        {
            animator.SetFloat("Speed", 1f);//Speed становится 1
        }
        else//иначе
        {
            animator.SetFloat("Speed", 0f);//становится 0
        }
    }
}
