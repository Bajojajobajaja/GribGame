using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 0.5f;
    public float attackRange = 1f;
    public float wakeUpDistance = 3f; // Дистанция, на которой противник "просыпается"
    public bool wakeUp = false;
    public LayerMask groundLayer;

    Transform player;
    Rigidbody2D rb;
    Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Проверяем дистанцию до игрока при входе в состояние
        if (player != null && Vector2.Distance(player.position, rb.position) < wakeUpDistance)
        {
            wakeUp = true;
        }
        if ((player != null) && wakeUp)
        {
            boss.LookAtPlayer();

            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1f, groundLayer);
            if (hit.collider == null)
            {
                return;
            }

            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            if (Vector2.Distance(player.position, rb.position) <= attackRange - 2)
            {
                animator.SetTrigger("Attack");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
