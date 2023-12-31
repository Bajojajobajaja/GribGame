using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HeroHealth : MonoBehaviour
{
    public Image HpBarHero;

    public float maxHealth = 100f;
    float currentHeroHealth;
    void Start()
    {
        currentHeroHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHeroHealth -= damage;
        HpBarHero.fillAmount = currentHeroHealth / maxHealth;

        Debug.Log("Ïèçäþëü");
        //anim

        if (currentHeroHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //anim and delit
        Destroy(gameObject);

        SceneManager.LoadScene(0);
    }
}
