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
    public int attackDamage = 20;

    public float dashDistance = 2f; // ��������� �����
    public float dashDuration = 0.1f; // ����������������� �����
    private bool isDashing = false;
    public float dashRate = 1f; // ����� �������� ����� ��������� �������������� �����
    private float nextDashTime = 0f; // ����� ���������� ���������� �����
    public int dashCount = 1;

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

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime && dashCount >= 1)
            {
                StartCoroutine(Dash());
                nextDashTime = Time.time + dashRate;
                dashCount--;
            }

            if (Input.GetButtonDown("Attack"))
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        float dashStart = transform.position.x;
        float distanceTraveled = 0f;

        // ���������� �������� � ����� "enemy"
        DisableEnemyColliders(true);

        while (distanceTraveled < dashDistance)
        {
            float dashStep = runSpeed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + dashStep * transform.localScale.x, transform.position.y);
            distanceTraveled += Mathf.Abs(dashStep); // ��������� ������ ������������� ����������
            yield return null;
        }

        // ��������� �������� � ����� "enemy" ����� �����
        DisableEnemyColliders(false);

        isDashing = false;
    }

    void DisableEnemyColliders(bool disable)
    {
        // ��������� ���� �������� � ����� "enemy" � �����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // ������ �� ������� ������� � ����������/��������� ����������
        foreach (GameObject enemy in enemies)
        {
            BoxCollider2D collider = enemy.GetComponent<BoxCollider2D>();
            CircleCollider2D collider2 = enemy.GetComponent<CircleCollider2D>();
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

            if (collider != null && collider2 != null)
            {
                collider.enabled = !disable;
                collider2.enabled = !disable;
                rb.gravityScale = disable ? 0f : 3f;
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
        if (!isAttacking && !isDashing) // ��������, ����� �� ��������� �� ����� �����
        {
            Controller.Move(horizontalmove * Time.fixedDeltaTime, jump);
        }
        jump = false;
        dashCount = 1;
    }
}
