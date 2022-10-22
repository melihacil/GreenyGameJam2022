using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,

    Follow,

    Die,

    Torba,

    TorbaAttack
};
//STOP EKLENECEK



public class EnemyController : MonoBehaviour
{


    GameObject player; 

    public EnemyState currState = EnemyState.Wander;

    public float speed;

    public float range;

    public float attackRange;

    private bool chooseDir = false;

    private bool dead = false;

    private bool interrupted = false;

    private bool hasAttacked = false;

    [SerializeField] private float torbaAttackRadius;
    [SerializeField] private float torbaAttackResetTime;
    [SerializeField] private float attackTime;
    [SerializeField] private GameObject attackFlashTorba;
    private float maxAtkktime;

    private int EnemyType = 0;
    /*
     * 0 Torba
     * 1 Kuþ
     * 2 Bidon
     */
    private Vector3 randomDir;

    private LayerMask playerLayerMask;


    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        playerLayerMask = LayerMask.NameToLayer("PlayerLayer");
        Debug.Log(playerLayerMask.value);
        maxAtkktime = attackTime;
    }

    void Update()
    {

        switch (currState)
        {
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):             
                break;
            case (EnemyState.Torba):
                EnemyType = 0;
                currState = EnemyState.Wander;
                break;
        }

        if(IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState=EnemyState.Wander;
        }
  
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }


    private bool IsPlayerInAttackRange(float attackRange)
    {
        return Vector3.Distance(transform.position, player.transform.position) >= attackRange;
    }

    //Rotation
    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        //Quaternion nextRotation = Quaternion.Euler(randomDir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;

    }

    void Wander()
    {

        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }


    }

    void Follow()
    {
        //Debug.Log((transform.position - player.transform.position).magnitude);
        if ((transform.position - player.transform.position).magnitude <= attackRange)
        {
            if (interrupted)
                attackTime = maxAtkktime;
            if (!hasAttacked && attackTime < 0)
            {
                hasAttacked = true;
                TorbaAttack();
            }
            else
                attackTime -= Time.deltaTime;
            return;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

    }




 

    private IEnumerator Attack()
    {
        //Debug.Log(playerLayerMask);
        yield return new WaitForSeconds(2f);
        TorbaAttack(); 
   
    }

    //Torba dusmaninin saldiriyor
    private void TorbaAttack()
    {
        Debug.Log("TorbaATTACKfonksiyonu");
        attackTime = maxAtkktime;
        Instantiate(attackFlashTorba, player.transform.position, Quaternion.identity);
        player.GetComponent<Player>().takeDamage(1f);
        Invoke(nameof(ResetAttack), torbaAttackResetTime);

    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, torbaAttackRadius);
    }


}
