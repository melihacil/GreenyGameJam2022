using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{



    //STATES -ATTACKING - MOVING - STUNNED(AFTER ATTACKING)

    private bool hasAttacked = false;
    private bool isStunned = false;
    [SerializeField] private float attackRadius;
    [SerializeField] private float bossMoveSpeed;
    [SerializeField] private Transform playerTransform;

    //private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<Player>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //player.position = Player.instance.gameObject.transform.position;
        //player.position = FindObjectOfType<Player>().transform.position;
        if ((transform.position - playerTransform.position).magnitude <= attackRadius && !hasAttacked)
        {
            //Needs to be reset
            hasAttacked = true;

            //Attack block


            //
        }
        //Move Block if also not stunned
        else if (!isStunned)
        {

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, bossMoveSpeed * Time.deltaTime);

        }


    }


    private void Attack()
    {

    }
}
