using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Sounds
{ 
    
    public Transform attackPos;//������� �����
    public LayerMask enemy;//����� ���������� ����
    public float attackRange;//������ �����
    public int damage;//����
    public Animator anim;//��� ��������

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse0))//� ������ ���
        {
            Attack();//������� �������� �����
        }
    }
    public void Attack()//�������
    {
        anim.SetTrigger("attack");//���� �������� �����
    }
    public void OnAttack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);//������� ���� ������ � ������� ����� � ������� Physics2D.OverlapCircle
        for (int i = 0; i < enemies.Length; i++)//����
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(damage);//������ ���� ������� ������� � ������� ����� �������� ����
            PlaySound(sounds[0]);//���� �����
        }
    }
    private void OnDrawGizmosSelected()//��� ������ ��� ����������� ������� �����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
