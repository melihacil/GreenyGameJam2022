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
    public LayerMask enemyLayers;

    void Start()
    {
     rb = GetComponent<Rigidbody2D>();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

    }

    void Attack()
    {
        //animasyon gelecek
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
        }
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
