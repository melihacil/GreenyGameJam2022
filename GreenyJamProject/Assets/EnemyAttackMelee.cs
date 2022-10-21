using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackMelee : MonoBehaviour
{

    [SerializeField] private Transform attackPos;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private float attackRadius;
    [SerializeField] private float attackResetTime;
    private bool hasAttacked = false;
    //Attackpos needs to rotate around transform position
    //It will rotate with atan2 possibly
    //need to get player transform



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float distanceX = EnemyFollowScript.instance.distanceX;
        float distanceY = EnemyFollowScript.instance.distanceY;
        //Debug.Log("Distance X = " + EnemyFollowScript.instance.distanceX + "Distance Y = " + EnemyFollowScript.instance.distanceY);
        //Working No 8 axis 
        //Only 4 directions
        if (distanceX > 0  && (distanceY < 1.5f && distanceY > -1.5f)) 
            attackPos.position = new Vector2(transform.position.x + 1, transform.position.y);
        else if (distanceX < 0 && (distanceY < 1.5f && distanceY > -1.5f))
            attackPos.position = new Vector2(transform.position.x - 1, transform.position.y);
        else if ((distanceX < 1.5F && distanceX > -1.5f) && distanceY > 0)
            attackPos.position = new Vector2(transform.position.x, transform.position.y + 1);
        else if ((distanceX < 1.5F && distanceX > -1.5f) && distanceY < 0)
            attackPos.position = new Vector2(transform.position.x, transform.position.y - 1);


        if (!hasAttacked && new Vector2(distanceX,distanceY).magnitude <= attackRadius)
        {
            hasAttacked = true;
            Attack();
        }

    }

    public void Attack()
    {
        Collider2D collision = Physics2D.OverlapCircle(attackPos.position, attackRadius, playerLayerMask);
        if (collision != null)
        {
            Debug.Log(collision.gameObject.name);
            //collision.gameObject.GetComponentInParent<PlayerStats>().DamagePlayer(20);
        }
        Invoke(nameof(ResetAttack), attackResetTime);
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }



    private void OnDrawGizmosSelected()
    {
        if (attackPos == null)
            return;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
