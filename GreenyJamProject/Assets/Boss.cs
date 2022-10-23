using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private Transform bossShadow;

    private Vector3 offset;
    [SerializeField] private float bossMovementSpeed;
    [SerializeField] private float randomMax;
    [SerializeField] private float randomMin;
    [SerializeField] private GameObject blast;
    [SerializeField] private LayerMask playerMask;
    //Attacking or following
    private bool isAttacking;
    private bool hasChosenRandomTime;
    public float AttackTime;
    public float yerBekleme;
    public float havaBekleme;
    public float yerBeklemeMax;
    public float attackRadius;

    private bool isVulnerable = false;



    [SerializeField] private float bossHealth;
    [SerializeField] private Slider healthSlider;




    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - bossShadow.position;
        //playerMask = LayerMask.NameToLayer("PlayerLayer");

        healthSlider.maxValue = bossHealth;
        healthSlider.value = bossHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Following player
        if (!isAttacking)
        {
            yerBekleme = yerBeklemeMax;
            //GetComponent<Animator>().SetTrigger("Waiting");
            if (!hasChosenRandomTime)
            {
                AttackTime = Random.Range(randomMin, randomMax);
                hasChosenRandomTime = true;
            }
            if (AttackTime <= 0)
            {
                isAttacking = true;
                GetComponent<Animator>().SetBool("isAttack", true);
                GetComponent<Animator>().SetBool("Hava", false);                
                GetComponent<Animator>().SetBool("Yer", false);
                GetComponent<Animator>().SetBool("isReset", false);
            }

            bossShadow.position = Vector3.MoveTowards(bossShadow.position, new Vector3(playerTransform.position.x, playerTransform.position.y -1, 0), bossMovementSpeed * Time.deltaTime);
            transform.position = bossShadow.position + offset;
            AttackTime -= Time.deltaTime;
        }
        else
        {
            
            yerBekleme -= Time.deltaTime;
            if(yerBekleme <= 0)
            {
                GetComponent<Animator>().SetBool("isReset", true);
                GetComponent<Animator>().SetBool("Yer", false);
                
            }
            //animation
            Debug.Log("Attacking");
            //GetComponent<Animator>().SetTrigger("Attack");
            blast.transform.position = bossShadow.position;
            blast.GetComponent<Animator>().SetTrigger("Blast");
            //Invoke(nameof(AttackReset), 2f);
        }
    }


    public void DamageBoss()
    {
        bossHealth -= 20;
        healthSlider.value = bossHealth;
    }

    public void ResetFalse()
    {
        GetComponent<Animator>().SetBool("isReset", false);
        AttackReset();
    }

    //Attack animasyonunun bittiði blok
    public void YerTrue()
    {
        Collider2D collision = Physics2D.OverlapCircle(bossShadow.position,attackRadius, playerMask);
        if (collision != null)
        {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.GetComponent<Player>().takeDamage(3f);

        }
        GetComponent<Animator>().SetBool("isAttack", false);
        GetComponent<Animator>().SetBool("Yer", true);
    }

    void AttackReset()
    {
        hasChosenRandomTime = false;
        isAttacking = false;
        //GetComponent<Animator>().SetTrigger("Reset");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(bossShadow.position, attackRadius);
    }
}
