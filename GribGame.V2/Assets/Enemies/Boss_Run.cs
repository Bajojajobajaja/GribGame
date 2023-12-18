using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
    public float speed = 0.5f;
    public float attackRange = 1f;
    public LayerMask groundLayer; // Set this in the Inspector to the layer(s) representing the ground.

    Transform player;
    Rigidbody2D rb;
    Boss boss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            boss.LookAtPlayer();

            // Raycast to check if there is ground below the enemy
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 1f, groundLayer);
            if (hit.collider == null)
            {
                // No ground below, stop movement
                return ;
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

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
