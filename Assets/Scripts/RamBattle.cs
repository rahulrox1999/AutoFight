using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RamBattle : MonoBehaviour
{
    public RamController ramA;
    public RamController ramB;
    public Text winnerText;
    public Slider healthBarA;
    public Slider healthBarB;

    void Start()
    {
        StartCoroutine(BattleCoroutine());
    }

    IEnumerator BattleCoroutine()
    {
        while (ramA.currentHealth > 0 && ramB.currentHealth > 0)
        {
            yield return new WaitForSeconds(Random.Range(1f, 2f)); // Attack interval

            if (Random.value > 0.5f)
                ramA.Attack(ramB);
            else
                ramB.Attack(ramA);

            UpdateHealthBars();
        }

        DeclareWinner();
    }

    void UpdateHealthBars()
    {
        healthBarA.value = ramA.currentHealth / ramA.maxHealth;
        healthBarB.value = ramB.currentHealth / ramB.maxHealth;
    }

    void DeclareWinner()
    {
        winnerText.gameObject.SetActive(true);
        winnerText.text = ramA.currentHealth > 0 ? "Winner: Ram A" : "Winner: Ram B";
    }
}

public class RamController : MonoBehaviour
{
    public Animator animator;
    public float maxHealth = 100f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void Attack(RamController opponent)
    {
        float damage = Random.Range(5f, 15f);
        if (Random.value < 0.2f) damage *= 2; // Critical hit chance

        opponent.TakeDamage(damage);
        PlayAttackAnimation();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    void PlayAttackAnimation()
    {
        animator.SetTrigger(Random.value > 0.5f ? "Charge" : "Kick");
    }
}
