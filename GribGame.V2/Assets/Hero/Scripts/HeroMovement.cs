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

    public float dashDistance = 2f; // Дистанция рывка
    public float dashDuration = 0.1f; // Продолжительность рывка
    private bool isDashing = false;
    public float dashRate = 1f; // Время задержки перед повторным использованием рывка
    private float nextDashTime = 0f; // Время следующего доступного рывка
    public int dashCount = 1;

    float horizontalmove = 0f;
    bool jump = false;
    bool isAttacking = false; // Новая переменная для отслеживания атаки

    
    void Update()
    {
        // Проверка, можно ли атаковать
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

        // Отключение коллизий с тегом "enemy"
        DisableEnemyColliders(true);

        while (distanceTraveled < dashDistance)
        {
            float dashStep = runSpeed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + dashStep * transform.localScale.x, transform.position.y);
            distanceTraveled += Mathf.Abs(dashStep); // Учитываем только положительное расстояние
            yield return null;
        }

        // Включение коллизий с тегом "enemy" после рывка
        DisableEnemyColliders(false);

        isDashing = false;
    }

    void DisableEnemyColliders(bool disable)
    {
        // Получение всех объектов с тегом "enemy" в сцене
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Проход по каждому объекту и отключение/включение коллайдера
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

    // Coroutine для атаки
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f); // Подождать, пока анимация атаки завершится

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(0.5f); // Подождать немного между атаками
        isAttacking = false;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void FixedUpdate()
    {
        if (!isAttacking && !isDashing) // Проверка, можно ли двигаться во время атаки
        {
            Controller.Move(horizontalmove * Time.fixedDeltaTime, jump);
        }
        jump = false;
        dashCount = 1;
    }
}
