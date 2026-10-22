using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Sounds
{
    public Transform attackPos;//позиция атаки
    public LayerMask enemy;//враги получающие урон
    public float attackRange;//радиус атаки
    public int damage;//урон
    public Animator anim;//для анимаций

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))//и нажата ЛКМ
        {
            Attack();//функция анимации атаки
        }
    }

    public void Attack()//функция
    {
        anim.SetTrigger("attack");//сама анимация атаки
    }

    public void OnAttack()//эта функция создана для ивента который мы добавляли на определенный кадр атаки
    {
        // Ищем всех врагов в радиусе атаки
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < enemies.Length; i++)//цикл
        {
            // ИСПРАВЛЕНО: ищем новый EnemyController вместо старого Enemy
            EnemyController enemyScript = enemies[i].GetComponent<EnemyController>();

            // Если скрипт найден, наносим урон
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);//каждый враг который нашелся в радиусе атаки получает урон
                PlaySound(sounds[0]);//звук удара
            }
        }
    }

    private void OnDrawGizmosSelected()//это просто для отображения радиуса атаки
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}