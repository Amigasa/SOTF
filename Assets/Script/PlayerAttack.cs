using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //перезарядка
    private float timeBtwAttack;
    public float startTimeBtwAttack; 
    
    public Transform attackPos;//позиция атаки
    public LayerMask enemy;//враги получающие урон
    public float attackRange;
    public int damage;
    public Animator anim;

    private void Update()
    {
        if(timeBtwAttack <= 0)
        {
            if(Input.GetMouseButton(0))
            {
                anim.SetTrigger("attack");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);
                for(int i = 0; i < enemies.Length; i++)
                {
                    
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
