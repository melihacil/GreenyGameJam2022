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

    private float movementForce;


    public float distanceX { get; private set; }
    public float distanceY { get; private set; }
    private float distanceMagnitude;

    // Start is called before the first frame update

    public float speed = 2f;
    public float force = 2f;
    private bool stop = false;
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
        distanceX = playerTransform.position.x - transform.position.x;
        distanceY = playerTransform.position.y - transform.position.y;
        //STOP COLUMN
        distanceMagnitude = (playerTransform.position - transform.position).magnitude;
        if (Mathf.Abs(distanceMagnitude) <= stopRange)
        {
            Stop();
        }
        //MOVE TOWARDS PLAYER
        else if (!stop)
        {

            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }

    }

    public void Stop()
    {
        stop = true;
        transform.position = transform.position;
    }

    public void ResetStop()
    {
        stop = false;
    }
    private void FixedUpdate()
    {

        //Debug.Log("Added force" + moveForce.magnitude);
    }
}
