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
    public Animator anim;
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 100f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public Vector2 idlePoint;



    [SerializeField] private TrailRenderer tr;
    public static Player instance;



    void Start()
    {
     rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        instance = this;
    }
    private void Awake()
    {
        
    }

    void Update()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
        anim.SetFloat("Horizontal", directionX);
        anim.SetFloat("Vertical", directionY);
        playerDirection = new Vector2(directionX, directionY).normalized;
        if (isDashing)
        {
            return;
        }
        if (directionX != 0)
        {
            anim.SetFloat("Speed", 1);
            if (directionX < 0)
            {
                anim.SetFloat("Horizontal", directionX);
                anim.SetFloat("lastVertical", 0);
                anim.SetFloat("lastHorizontal", -1);
                Vector3 center = rb.position;
                Vector3 add = new Vector3(-1,0,0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add,Quaternion.identity);
                spriteRend.flipX = true;
                
            }
            
            if (directionX > 0)
            {
                anim.SetFloat("Horizontal", directionX);
                anim.SetFloat("lastVertical", 0);
                anim.SetFloat("lastHorizontal", 1);
                Vector3 center = rb.position;
                Vector3 add = new Vector3(+1, 0, 0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add, Quaternion.identity);
                spriteRend.flipX = false;
                
            }
        }
        if(directionY != 0)
        {
            anim.SetFloat("Speed", 1);
            if (directionY < 0)
            {
                anim.SetFloat("Vertical", directionY);
                anim.SetFloat("lastHorizontal", 0);
                anim.SetFloat("lastVertical", -1);
                Vector3 center = rb.position;
                Vector3 add = new Vector3(0, -1, 0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add, Quaternion.identity);
                
            }
            if (directionY > 0)
            {
                anim.SetFloat("Vertical", directionY);
                anim.SetFloat("lastHorizontal", 0);
                anim.SetFloat("lastVertical", 1);
                Vector3 center = rb.position;
                Vector3 add = new Vector3(0, 1, 0);
                attackPoint.transform.SetPositionAndRotation(transform.position + add, Quaternion.identity);
                
            }
            
        }
        if(directionX == 0 && directionY == 0)
        {
            anim.SetFloat("Speed", 0);
        }
        if (Time.time >= nextAttackTime) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("isAttacking", true);
                //stopMovement();
                Attack();
                nextAttackTime = Time.time + 1f / attackSpeed;                
                StartCoroutine(Wait(0.1f));
                anim.SetFloat("Horizontal", 0);
                anim.SetFloat("Vertical", 0);
                
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
        if (Input.GetKeyDown(KeyCode.LeftControl) && canDash)
        {
            StartCoroutine(Dash());
        }

    }
   /* private void stopMovement()
    {
        anim.SetBool("canMove", false);
        float temp;
        temp = movementSpeed;
        movementSpeed = 0f;
        StartCoroutine(Waitt(0.8f));
        
    }*/
    private IEnumerator Wait(float second)
    {
        yield return new WaitForSeconds(second);
        anim.SetBool("isAttacking", false);
        //anim.SetBool("canMove", true);
    }
    private IEnumerator Waitt(float second)
    {
        yield return new WaitForSeconds(second);
        movementSpeed = 5f;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        tr.emitting = true;
        float temp;
        temp = movementSpeed;
        movementSpeed = 10f;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        movementSpeed = temp;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }


    private void Attack()
    {
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
            if (enemy.CompareTag("breakable"))
            {
                enemy.GetComponent<Breakables>().Destroy();
            }
            if (enemy.CompareTag("enemy"))
            {
                enemy.gameObject.GetComponent<EnemyController>().DamageEnemy();
            }
            if (enemy.CompareTag("Boss"))
            {
                Debug.Log("hit boss");
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
        if (isDashing)
        {
            return;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
