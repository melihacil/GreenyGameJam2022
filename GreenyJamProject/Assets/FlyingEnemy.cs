using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{


    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform randomPos_1, randomPos_2;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float resetAttackTime;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float moveSpeed;
    private float currentTime = 0.2f;
    private bool hasChosenRandomPos = false;
    private bool hasAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float randomX;
    float randomY;
    // Update is called once per frame
    void Update()
    {

        if (currentTime <= 0 && !hasAttacked)
        {
            hasAttacked = true;
            Attack();
            //Stop movement
            randomX = transform.position.x;
            randomY = transform.position.y;
        }
        //Movement
        if (!hasChosenRandomPos)
        {
            randomX = Random.Range(randomPos_1.position.x, randomPos_2.position.x);
            randomY = Random.Range(randomPos_1.position.y, randomPos_2.position.y);
            hasChosenRandomPos = true;
        }
        currentTime -= Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(randomX, randomY, 0f), moveSpeed * Time.deltaTime);
    }


    void Attack()
    {
        Instantiate(projectile, attackPos.position, Quaternion.identity);
        Invoke(nameof(ResetAttack), resetAttackTime);
    }


    void ResetAttack()
    {
        hasAttacked = false;
        currentTime = attackSpeed;
        hasChosenRandomPos=false;
    }
}
