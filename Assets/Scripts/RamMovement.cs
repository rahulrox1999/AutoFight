using UnityEngine;

public class RamMovement : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 2f; // Stop moving and attack within this range
    public Animator animator;
    private bool isAttacking = false;
    private Transform targetRam;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FindEnemyRam();
        animator.SetBool("Walk", true); // Start walking
    }

    void Update()
    {
        if (targetRam == null || isAttacking) return; // Stop movement if attacking

        float distance = Vector3.Distance(transform.position, targetRam.position);

        if (distance > attackRange) // If too far, chase
        {
            animator.SetBool("Walk", true);
            MoveTowardsEnemy();
        }
        else // Stop movement and attack
        {
            isAttacking = true;
            animator.SetBool("Walk", false);
            rb.velocity = Vector3.zero; // Stop Rigidbody movement
            GetComponent<RamAnimationController>().StartAttack();
        }
    }

    public void StopMoving()
    {
        speed = 0; // Stop movement
        animator.SetBool("Walk", false);
    }



    private void MoveTowardsEnemy()
    {
        Vector3 direction = (targetRam.position - transform.position).normalized;
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        transform.LookAt(targetRam);
    }

    private void FindEnemyRam()
    {
        GameObject[] rams = GameObject.FindGameObjectsWithTag("Ram"); // Find all rams
        foreach (GameObject ram in rams)
        {
            if (ram != gameObject) // Ignore itself
            {
                targetRam = ram.transform;
                break;
            }
        }
    }

    public void ResetAttackState()
    {
        isAttacking = false; // Allow movement again if needed
    }
}
