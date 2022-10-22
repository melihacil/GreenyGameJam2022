using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanMovement : MonoBehaviour
{
    //For d�ng�s�nde arraydaki lokasyonlardan oyuncuya en uzak olan� se�ecek ve oraya do�ru h�zl� bir �ekilde gidecek ve spawn ba�layacak
    //lokasyona vard���nda spawn s�reci ba�l�yor ve 1.5 saniye sonra spawn yap�p 5 saniye boyunca idle ve damage yiyebilir �ekilde bekliyor.
    //spawn s�recinde hasar yerse tekrar ka�maya ba�l�yor

    
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
