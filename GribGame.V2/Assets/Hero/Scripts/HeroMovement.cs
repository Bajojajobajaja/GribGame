using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class HeroMovement : MonoBehaviour
{
    public CharacterController2D Controller;
    public Animator animator;
    public Transform attackPoint; // ������� �����
    public LayerMask enemyLayers;

    public float runSpeed = 20f;
    public float attackRange = 0.5f;
    public int attackDamage = 5;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    float horizontalmove = 0f;
    bool jump = false;
    bool isAttacking = false; // ����� ���������� ��� ������������ �����

    
    void Update()
    {
        // ��������, ����� �� ���������
        if (!isAttacking)
        {
            horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalmove));

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
                animator.SetBool("IsJumping", true);
            }

            /*if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                animator.SetBool("IsCrouching", true);
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
                animator.SetBool("IsCrouching", false);
            }*/

            if (Input.GetButtonDown("Attack") && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    // Coroutine ��� �����
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f); // ���������, ���� �������� ����� ����������

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(0.5f); // ��������� ������� ����� �������
        isAttacking = false;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void FixedUpdate()
    {
        if (!isAttacking) // ��������, ����� �� ��������� �� ����� �����
        {
            Controller.Move(horizontalmove * Time.fixedDeltaTime, jump);
        }
        jump = false;
    }
}
