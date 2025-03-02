using UnityEngine;
using System.Collections;

public class RamAnimationController : MonoBehaviour
{
    public Animator animator;
    private RamCombat combat;
    private RamMovement movement;

    void Start()
    {
        combat = GetComponent<RamCombat>();
        movement = GetComponent<RamMovement>();
    }

    public void StartAttack()
    {
        if (combat.health <= 0) return; // Don't attack if dead

        float rand = Random.value;
        string attackType;

        if (rand < 0.4f)
            attackType = "Attack1";
        else if (rand < 0.8f)
            attackType = "Attack2";
        else
            attackType = "PowerAttack";

        animator.SetTrigger(attackType);
        combat.Attack(attackType); // Deal damage to enemy

        StartCoroutine(ResetAfterAttack());
    }

    public void PlayHitReaction()
    {
        animator.SetTrigger("Hit"); // Play hit animation
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death"); // Play death animation
    }

    IEnumerator ResetAfterAttack()
    {
        yield return new WaitForSeconds(1.5f);
        movement.ResetAttackState();
    }
}
