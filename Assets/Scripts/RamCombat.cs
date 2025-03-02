using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RamCombat : MonoBehaviour
{
    public int health = 100;
    public Slider healthBar;
    public GameObject damageTextPrefab;
    public TextMeshProUGUI winnerText; // Assign in Inspector

    private RamAnimationController animationController;
    private RamMovement movement;
    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animationController = GetComponent<RamAnimationController>();
        movement = GetComponent<RamMovement>();
        animator = GetComponent<Animator>();

        if (healthBar != null)
            healthBar.maxValue = health;

        UpdateHealthUI();
    }

    public void Attack(string attackType)
    {
        if (isDead) return;

        int damage = attackType switch
        {
            "Attack1" => 10,
            "Attack2" => 15,
            "PowerAttack" => 20,
            _ => 0
        };

        GameObject enemyRam = FindClosestEnemy();
        if (enemyRam != null)
        {
            RamCombat enemyCombat = enemyRam.GetComponent<RamCombat>();

            if (!enemyCombat.isDead)
            {
                enemyCombat.TakeDamage(damage);
                enemyCombat.ShowDamageNumber(enemyRam.transform.position + Vector3.up * 2, damage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        UpdateHealthUI();
        animationController.PlayHitReaction();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        health = 0;
        UpdateHealthUI();

        animationController.PlayDeathAnimation();
        movement.StopMoving();

        Debug.Log(gameObject.name + " has been defeated!");

        GetComponent<RamCombat>().enabled = false;
        if (movement != null) movement.enabled = false;

        DeclareWinner();
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

    private void ShowDamageNumber(Vector3 spawnPosition, int damage)
    {
        if (damageTextPrefab != null && !isDead)
        {
            GameObject damageText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity);
            TextMeshPro textMesh = damageText.GetComponent<TextMeshPro>();
            textMesh.text = damage.ToString();

            Destroy(damageText, 0.5f);
        }
    }

    private void DeclareWinner()
    {
        GameObject[] rams = GameObject.FindGameObjectsWithTag("Ram");
        foreach (GameObject ram in rams)
        {
            RamCombat ramCombat = ram.GetComponent<RamCombat>();
            if (!ramCombat.isDead)
            {
                if (winnerText != null)
                {
                    winnerText.text = ram.name + " Wins!";
                    winnerText.gameObject.SetActive(true); // Show the text
                }
                Debug.Log(ram.name + " Wins!");

                // Restart match after 3 seconds
                Invoke("RestartMatch", 3f);
            }
        }
    }

    private void RestartMatch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
