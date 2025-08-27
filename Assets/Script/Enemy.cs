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
    public int health;
    public float speed;
    bool A=true;
    public Transform groundCheck;
    bool isGrounded = false;
    public float groundDistance;
    public LayerMask whatIsGround;

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, whatIsGround);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (isGrounded)
        {
            A = !A;
        }
        if (A==true)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (A==false)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
