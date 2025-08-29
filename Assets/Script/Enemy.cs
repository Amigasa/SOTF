using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health;//хп врага
    public float speed;//скорость
    bool A=true;//переменная 
    public Transform groundCheck;//проверка для столкновения со стенами
    bool isGrounded = false;//переменная для смены направления бега
    public float groundDistance;//дистанция проверки стен
    public LayerMask whatIsGround;//сами стены

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, whatIsGround);//С помощью радиуса проверяется есть ли в нем нужный объект, то есть стена
        if (health <= 0)//если хп врага становится 0
        {
            Destroy(gameObject);//он уничтожается
        }
        if (isGrounded)//если определенное значение переменной
        {
            A = !A;//другая переменная меняет значение
        }
        if (A==true)//если это значение true
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);//враг бежит влево
        }
        else if (A==false)//иначе
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);//вправо
        }
    }
    public void TakeDamage(int damage)//функция для получения урона
    {
        health -= damage;//от хп отнимается дамаг нанесенный игроком
    }
}
