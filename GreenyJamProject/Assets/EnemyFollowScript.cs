using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowScript : MonoBehaviour
{


    public static EnemyFollowScript instance;

    [SerializeField]private Transform playerTransform;
    public Transform publicPlayerTransform;

    [SerializeField]
    private float stopRange;
    Rigidbody2D enemyRb;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float movementForce;


    public float distanceX { get; private set; }
    public float distanceY { get; private set; }
    private float distanceMagnitude;

    // Start is called before the first frame update

    public float speed = 2f;
    public float force = 2f;

    private Vector2 direction;


    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        instance = this;
    }
    void Start()
    {
        distanceX = 0;
        if (playerTransform == null)
        {
            //FindObjectOfType<PlayerMovement>().
        }
    }

    // Update is called once per frame
    void Update()
    {
        publicPlayerTransform = playerTransform;
        //distance = Mathf.Atan2((playerTransform.position.x - transform.position.x), (playerTransform.position.y - transform.position.y));
        distanceX = playerTransform.position.x - transform.position.x;
        distanceY = playerTransform.position.y - transform.position.y;
        //Debug.Log(distance);
        //STOP COLUMN
        distanceMagnitude = (playerTransform.position - transform.position).magnitude;
        if (Mathf.Abs(distanceMagnitude) <= stopRange)
        {
            //Stop();
        }
        //MOVE TOWARDS PLAYER
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }

    }



    private void FixedUpdate()
    {
        var desiredVelocity = direction * speed;
        var deltaVelocity = desiredVelocity - enemyRb.velocity;
        Vector3 moveForce = deltaVelocity * (force * movementForce * Time.fixedDeltaTime);
        enemyRb.AddForce(moveForce, ForceMode2D.Force);
        //Debug.Log("Added force" + moveForce.magnitude);
    }
}
