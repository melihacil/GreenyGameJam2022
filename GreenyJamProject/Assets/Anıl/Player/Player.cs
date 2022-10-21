using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 playerDirection;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackDamage = 0.5f;
    public LayerMask enemyLayers;
    public float maxHealth = 10f;
    public float health = 10f;
    public float attackSpeed = 2f;
    public float invincibilityCooldown;
    public float nextAttackTime = 0f;
    public float nextInvincibilityTime = 0f;
    public float iFrameDuration;
    public int numberOfFlashes;
    private SpriteRenderer spriteRend;

    void Start()
    {
     rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
         
        playerDirection = new Vector2(directionX, directionY).normalized;
        if (directionX != 0)
        {
            if (directionX < 0)
            {
                Vector3 center = rb.position;
                Vector3 add = new Vector3(-1,0,0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add,Quaternion.identity);
            }
            if (directionX > 0)
            {
                Vector3 center = rb.position;
                Vector3 add = new Vector3(+1, 0, 0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add, Quaternion.identity);
            }
        }
        if(directionY != 0)
        {
            if (directionY < 0)
            {
                Vector3 center = rb.position;
                Vector3 add = new Vector3(0, -1, 0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add, Quaternion.identity);
            }
            if (directionY > 0)
            {
                Vector3 center = rb.position;
                Vector3 add = new Vector3(0, 1, 0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add, Quaternion.identity);
            }
            
        }
        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;
            }
        }
        if (Time.time >= nextInvincibilityTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Invincibility());
                Invincibility();
                nextInvincibilityTime = Time.time + 1f / invincibilityCooldown;
            }
        }
    }


    void Attack()
    {
        //animasyon gelecek
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
            if (enemy.CompareTag("breakable"))
            {
                enemy.GetComponent<Breakables>().Destroy();
            }
        }
    }
    void death()
    {
        if(health == 0)
        {
            this.enabled = false;
        }
        //animasyon gelecek
        
    }
    private IEnumerator Invincibility()
    {
        Physics2D.IgnoreLayerCollision(0,7,true);
        Physics2D.IgnoreLayerCollision(0,6,true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        Physics2D.IgnoreLayerCollision(0, 7, false);
        Physics2D.IgnoreLayerCollision(0, 6, false);

    }

    public void takeDamage(float damage)
    {
        
        health = health - damage;
        death();
        Debug.Log("Health: " + health);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(playerDirection.x * movementSpeed, playerDirection.y * movementSpeed);

    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
