using UnityEngine;
using UnityEngine.UI; // Import UI namespace

public class RamCombat : MonoBehaviour
{
    public int health = 100;
    public Slider healthBar; // Health bar UI

    private RamAnimationController animationController;
    private RamMovement movement;
    private bool isDead = false;

    void Start()
    {
        animationController = GetComponent<RamAnimationController>();
        movement = GetComponent<RamMovement>();

        if (healthBar != null)
            healthBar.maxValue = health; // Set max value
        UpdateHealthUI(); // Initialize health bar
    }

    public void Attack(string attackType)
    {
        if (isDead) return; // Stop attacking if dead

        int damage = attackType switch
        {
            "Attack1" => 10,
            "Attack2" => 20,
            "PowerAttack" => 30,
            _ => 0
        };

        GameObject enemyRam = FindClosestEnemy();
        if (enemyRam != null)
        {
            enemyRam.GetComponent<RamCombat>().TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ignore if already dead

        health -= damage;
        UpdateHealthUI(); // Update health bar

        animationController.PlayHitReaction(); // Play hit animation

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        health = 0;
        UpdateHealthUI(); // Set health bar to 0
        animationController.PlayDeathAnimation();
        movement.StopMoving(); // Stop movement
        Debug.Log(gameObject.name + " has been defeated!");
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] rams = GameObject.FindGameObjectsWithTag("Ram");
        foreach (GameObject ram in rams)
        {
            if (ram != gameObject)
            {
                return ram;
            }
        }
        return null;
    }
}
