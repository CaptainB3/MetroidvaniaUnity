using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Combo Logic")]
    [SerializeField] public float comboWindow = 1.0f; // Time allowed between combo attacks
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private bool canAttack = true;
    private bool attackBuffered = false;

    [Header("Damage Hitbox")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private LayerMask enemyLayers;

    private Animator anim;
    private PlayerMovement playerMovement;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Time.time - lastAttackTime > comboWindow)
        {
            comboStep = 0;
            attackBuffered = false;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            if (canAttack)
            {
                Attack();
            }
            else
            {
                attackBuffered = true;
            }
        }

        if (playerMovement != null && attackPoint != null)
        {
            // Flip attackPoint X offset when facing left
            if (playerMovement.GetComponent<SpriteRenderer>().flipX)
                attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
            else
                attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
    }

    private void Attack()
    {
        attackBuffered = false;

        canAttack = false;
        playerMovement.canMove = false;

        comboStep++;

        if (comboStep > 3)
        {
            comboStep = 1;
        }

        switch (comboStep)
        {
            case 1:
                anim.SetTrigger("Attack1");
                break;
            case 2:
                anim.SetTrigger("Attack2");
                break;
            case 3:
                anim.SetTrigger("Attack3");
                break;
        }
    }

    public void DoDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit " + enemy.name);
        }
    }

    // Called by 
    public void ResetAttack()
    {
        lastAttackTime = Time.time;
        canAttack = true;
        playerMovement.canMove = true;

        // If player clicked WHILE attack was being played
        if (attackBuffered)
        {
            Attack();
        }
    }

    // Helper to see attack range (in Editor)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}