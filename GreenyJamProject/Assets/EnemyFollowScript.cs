using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{

    [SerializeField]
    private Transform playerTransform;


    Rigidbody2D enemyRb;

    // Start is called before the first frame update


    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
       
        if (playerTransform == null)
        {
            //FindObjectOfType<PlayerMovement>().
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
