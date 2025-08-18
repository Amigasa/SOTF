using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
public class Moving : MonoBehaviour
{
    Rigidbody2D body; // переменна€ дл€ хранени€ тела персонажа
    public float speed; // переменна€ скорости
    public float jump; // переменна€ прыжка
    float axis; // переменна€ дл€ хранени€ состо€ни€ ќси
    Vector3 size; // переменна€ дл€ хранени€ размера персонажа
    bool noAir = true; // ѕеременна€, котора€ определ€ет в воздухе персонаж или нет
    void Start()
    {
        body = GetComponent<Rigidbody2D>(); // получаем тело
        size = gameObject.transform.localScale; // получаем размер
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//ѕровер€ем, столкнулись ли мы с землей
        {
            noAir = true; // ≈сли столкнулись, переключаем на истину (мы на земле!)
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")//ѕровер€ем, прекратили ли мы столкновение с землей
        {
            noAir = false; // ≈сли прекратили, переключаем на ложь (мы не на земле!)
        }
    }
    void Update()
    {
        axis = Input.GetAxisRaw("Horizontal"); // получаем состо€ние ќси 
        // если идем вправо, поворачиваем персонажа вправо
        if (axis > 0)
        {
            gameObject.transform.localScale = size;
        }
        // если идем влево, поворачиваем персонажа влево
        if (axis < 0)
        {
            gameObject.transform.localScale = new Vector3(-size.x, size.y, size.z);
        }
        // прыжок
        if (Input.GetButtonDown("Jump") && noAir) // ѕроверка: на земле ли мы
        {
            body.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        }
    }
    private void FixedUpdate()
    {
        // перемещение вправо/влево
        Input.GetButton("Horizontal");
        body.velocity = new Vector2(axis * speed, body.velocity.y);
    }
}

