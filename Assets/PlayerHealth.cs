using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int health;
    public event Action Death;
    public event Action<int> OnAddHealth;
    public event Action<int> OnHealthInnit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        OnHealthInnit?.Invoke(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddHealth(int healthToAdd)
    {
        health += healthToAdd;
        OnAddHealth?.Invoke(health);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Death?.Invoke();
    }

    public int GetHealth()
    {
        return health;
    }
}
