using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Sounds
{
    //перезарядка
    private float timeBtwAttack;
    public float startTimeBtwAttack; 
    
    public Transform attackPos;//позиция атаки
    public LayerMask enemy;//враги получающие урон
    public float attackRange;
    public int damage;
    public Animator anim;
    bool attack;

    private void Start()
    {
        attack = false;
    }
    public void Attack()
    {
        attack = false;
    }

    private void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                attack = true;
                anim.SetBool("attack", attack);
                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
                PlaySound(sounds[0]);
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
