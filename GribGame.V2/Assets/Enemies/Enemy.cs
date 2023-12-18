using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    float currentHealth;
    public Animator animator;
    public Image Bar;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Bar.fillAmount = currentHealth / maxHealth;
        //anim

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        //anim and delit
        //GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        //this.enabled = false;
    }
    
    void Update()
    {
        
    }
}
