using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Rigidbody2D rb;

    private Transform player;
    private bool isShot = false;

    [SerializeField] private float projectileForce;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (!isShot)
        {
            Debug.Log(player.position + " Attacking");
            isShot = true;

            rb.AddForce((player.position - transform.position) * projectileForce, ForceMode2D.Impulse);
            Invoke(nameof(DestroyFireball), 5f);
        }
    }
    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Debug.Log("Hit player");
            //collision.gameObject.GetComponent<PlayerStats>().Damage(5f);
            collision.gameObject.GetComponent<Player>().takeDamage(2f);
        }
        Destroy(gameObject);
    }
}
