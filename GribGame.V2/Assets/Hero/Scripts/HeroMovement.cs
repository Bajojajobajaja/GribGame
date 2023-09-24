using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class HeroMovement : MonoBehaviour
{
    public CharacterController2D Controller;
    public Animator animator;
    public Transform attackPoint; // хитбокс атаки
    public LayerMask enemyLayers;

    public float runSpeed = 20f;
    public float attackRange = 0.5f;
    public int attackDamage = 20;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    float horizontalmove = 0f;
    bool jump = false;
    bool crouch = false;

    void Update()
    {
        horizontalmove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalmove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("IsCrouching", true);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }

        if (Input.GetButtonDown("Attack") && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 2f / attackRate;
        }
        
    }

    public async void Attack()
    {
        animator.SetTrigger("Attack");

        var t1 = Task.Delay(300);
        await t1;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies) 
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    public void FixedUpdate()
    {
        Controller.Move(horizontalmove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
