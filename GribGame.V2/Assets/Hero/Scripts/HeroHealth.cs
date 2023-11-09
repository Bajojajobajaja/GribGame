using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroHealth : MonoBehaviour
{

    public int maxHealth = 100;
    int currentHeroHealth;
    void Start()
    {
        currentHeroHealth = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        currentHeroHealth -= damage;
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
        Debug.Log("Ñìýðòü");
        SceneManager.LoadScene(0);
    }
}
