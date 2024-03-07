using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HeroHealth : MonoBehaviour
{
    public Animator animator;
    public Image HpBarHero;
    public GameObject deathMessageText;

    public float maxHealth = 40f;
    float currentHeroHealth;
    void Start()
    {
        currentHeroHealth = maxHealth;
        deathMessageText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHeroHealth -= damage;
        HpBarHero.fillAmount = currentHeroHealth / maxHealth;
        //anim

        if (currentHeroHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        StartCoroutine(DeathText());
    }

    IEnumerator DeathText()
    {
        //Destroy(gameObject);
        //anim and delit

        animator.SetBool("IsDeath", true);
        yield return new WaitForSeconds(1f);
        deathMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);

        deathMessageText.gameObject.SetActive(false);
        animator.SetBool("IsDeath", false);
    }
}