using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{



    //STATES -ATTACKING - MOVING - STUNNED(AFTER ATTACKING)

    private bool hasAttacked;
    private bool isStunned;
    [SerializeField] private float attackRadius;


    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player.position = Player.instance.gameObject.transform.position;

        if ((transform.position - player.position).magnitude <= attackRadius && !hasAttacked)
        {
            //Needs to be reset
            hasAttacked = true;

            //Attack block


            //
        }
        //Move Block if also not stunned
        else if (!isStunned)
        {

        }




    }


    private void Attack()
    {

    }
}
