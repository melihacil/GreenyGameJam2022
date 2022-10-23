using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,

    Follow,

    Die,

    Torba,

    TorbaAttack,

    Spawner,

    Fly
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

    public bool isInRoom = false;

    public int EnemyType = 0;
    /*
     * 0 Torba
     * 1 Kuþ
     * 2 Bidon
     */
    private Vector3 randomDir;

    private LayerMask playerLayerMask;


    //Health Stuff


    [SerializeField] private int health;
    private int visualCount;
    private bool isDead = false;
    [SerializeField] private GameObject[] healthVisuals;



    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        playerLayerMask = LayerMask.NameToLayer("PlayerLayer");
        Debug.Log(playerLayerMask.value);
        maxAtkktime = attackTime;

        health = healthVisuals.Length - 1;
        visualCount = healthVisuals.Length;
        foreach (var healthVisual in healthVisuals)
        {
            healthVisual.SetActive(false);
        }
        if (healthVisuals.Length > 0)
        healthVisuals[healthVisuals.Length - 1].SetActive(true);


    }

    void Update()
    {
        if (!isInRoom)
            return;
        if (isDead)
        {
            DeathFunction();
            return;
        }
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
            case (EnemyState.Spawner):
                Spawner();
                break;
            case (EnemyState.Fly):
                Fly();

                break;
        }
        if (EnemyType == 2 )
        {
            currState = EnemyState.Spawner;
            return;
        }
        else if (EnemyType == 1)
        {
            currState = EnemyState.Fly;
            return;
        }
        if (IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
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

    public void DamageEnemy()
    {
        if (isDead || healthVisuals.Length <= 0)
            return;
        visualCount--;
        healthVisuals[health].SetActive(false);
        health--;
        if (health >= 0)
            healthVisuals[health].SetActive(true);
        if (health == 0)
        {
            isDead = true;
            //gameObject.SetActive(false);
            Invoke("DestroyObject", 2f);
            GetComponent<Animator>().SetTrigger("Death");
        }
        Debug.Log(isDead);
    }

    public void DeathFunction()
    {
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        transform.localScale -= new Vector3(0.02f, 0.02f, 0);
    }


    private void DestroyObject()
    {
        Destroy(gameObject);
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


    EnemySpawner spawner;
    void Spawner()
    {
        spawner = GetComponent<EnemySpawner>();




    }


    #region Fly

    [Header("Fly Attributes")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private Transform flyAttackPos;
    [SerializeField] private float currentTime = 0.2f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float resetAttackTime;
    [SerializeField] private bool hasFlyAttacked = false;
    void Fly()
    {
        Debug.Log("Fly--");
        if (currentTime <= 0 && !hasFlyAttacked)
        {
            hasFlyAttacked = true;
            FlyAttack();
        }
        currentTime -= Time.deltaTime;



    }
    void FlyAttack()
    {
        Debug.Log("Attacking");
        Instantiate(projectile, flyAttackPos.position, Quaternion.identity);
        Invoke(nameof(ResetFlyAttack), resetAttackTime);
    }


    void ResetFlyAttack()
    {
        hasFlyAttacked = false;
        currentTime = attackSpeed;
    }



    #endregion
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
        //Instantiate(attackFlashTorba, player.transform.position, Quaternion.identity);
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