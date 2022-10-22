using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanMovement : MonoBehaviour
{
    //For döngüsünde arraydaki lokasyonlardan oyuncuya en uzak olaný seçecek ve oraya doðru hýzlý bir þekilde gidecek ve spawn baþlayacak
    //lokasyona vardýðýnda spawn süreci baþlýyor ve 1.5 saniye sonra spawn yapýp 5 saniye boyunca idle ve damage yiyebilir þekilde bekliyor.
    //spawn sürecinde hasar yerse tekrar kaçmaya baþlýyor

    
    [SerializeField] private Transform[] canNests;
    [SerializeField] private float moveSpeed;
    private Transform canNextNest;
    private bool movingToNest = false;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = FindObjectOfType<Player>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movingToNest)
        {
            playerTransform.position = Player.instance.gameObject.transform.position;
            float distance = 0f;
            //Choosing next nest that has the max distance from the player
            foreach (Transform t in canNests)
            {
                float a = (t.position - playerTransform.position).magnitude;
                if (a > distance)
                {
                    distance = a;
                    canNextNest = t;
                }
            }

            Debug.Log(canNextNest.gameObject.name);
            MoveToNextNest();
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
        }
    }

    void MoveToNextNest()
    {
        if (transform.position == canNextNest.position) 
            movingToNest = true;

        gameObject.layer = LayerMask.NameToLayer("TrashcanMoving");
        transform.position = Vector3.MoveTowards(transform.position, canNextNest.position, moveSpeed * Time.deltaTime);
        //addforce ya da movetowards yada transform move pos 

    }

    //HASAR ALDIKTAN SONRA RESETLENECEK
    private void ResetMovingNest()
    {
        movingToNest = false;
    }
}
