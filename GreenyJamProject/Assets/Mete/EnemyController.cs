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
        if(healthVisuals.Length > 3)
        {
            healthVisuals[2].SetActive(true);
        }

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
        if (healthVisuals.Length > 3 && health <= 3)
        {

            healthVisuals[3].SetActive(true);
        }
        health--;
        if (health >= 0)
            healthVisuals[health].SetActive(true);
        if (healthVisuals.Length > 3 && health ==3)
        {
            health--;
        } 
        if (health == 0)
        {
            Debug.Log("dead");
            isDead = true;
            //gameObject.SetActive(false);
            Invoke("DestroyObject", 2f);
            if (EnemyType == 0)
                GetComponent<Animator>().SetTrigger("Death");
        }
        Debug.Log(isDead);
    }

    public void DeathFunction()
    {
        Debug.Log("Making it smaller");
        transform.localScale -= new Vector3(0.02f, 0.02f, 0);
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
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

    #region Spawner
    [Header("Spawner")]
    EnemySpawner spawner;
    private bool hasInitialized = false;
    [SerializeField] private bool hasSpawned;
    [SerializeField] private float spawnTime;
    [SerializeField] private float resetSpawnTime;

    void Spawner()
    {
        if (!hasInitialized)
        {
            spawner = GetComponent<EnemySpawner>();
            currentTime = 2f;
            hasInitialized = true;
        }

        //Movement 
        //Spawning
        //Getting grid vectors

        currentTime -= Time.deltaTime;
        if (currentTime <= 0 && !hasSpawned)
        {
            hasSpawned = true;
            SpawnEnemy();
        }

        if (!choseApoint)
        {
            Debug.Log("choosing point");
            vectors = FindObjectOfType<RoomController>().currRoom.GetComponent<GridController>().availablePoints;
            target = vectors[Random.Range(0, vectors.Count)];
            Debug.Log(target);
            choseApoint = true;
            Invoke(nameof(ResetPoint), randomPointTime);
        }
        if (EnemyType == 2 && (Vector2)transform.position == target)
            GetComponent<Animator>().SetTrigger("Idle");
        else if (EnemyType == 2)
            GetComponent<Animator>().SetTrigger("Run");
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);


    }
    void ResetSpawner()
    {
        hasSpawned = false;
        currentTime = spawnTime;
    }
    void SpawnEnemy()
    {
        spawner.SpawnEnemy();
        Invoke(nameof(ResetSpawner), resetSpawnTime);
    }

    #endregion

    #region Fly

    [Header("Fly Attributes")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private Transform flyAttackPos;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float resetAttackTime;
    [SerializeField] private bool hasFlyAttacked = false;


    [Header("Time")]
    [SerializeField] private float currentTime = 0.2f;
    [SerializeField] private float randomPointTime;
    private bool choseApoint = false;
    List<Vector2> vectors;
    Vector2 target;
    void Fly()
    {

        if (currentTime <= 0 && !hasFlyAttacked)
        {
            hasFlyAttacked = true;
            FlyAttack();
        }
        currentTime -= Time.deltaTime;
        if (!choseApoint)
        {
            vectors = FindObjectOfType<RoomController>().currRoom.GetComponent<GridController>().availablePoints;
            //vectors = FindObjectOfType<GridController>().availablePoints;
            target = vectors[Random.Range(0, vectors.Count)];
            choseApoint = true;
            Invoke(nameof(ResetPoint), randomPointTime);
        }
        
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

    }

    void ResetPoint()
    {
        choseApoint = false;
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