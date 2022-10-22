using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    public GameObject doorCollider;

    private GameObject player;
    

    private float widthOffset = 1.75f;

    private void Start()
    {
      //  player = GameObject.FindGameObjectsWithTag("Player");
    }
    /*
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            switch (doorType)
            {
                case DoorType.bottom:
                    player.transform.position = new Vector2(transform.position.x, transform.position - widthOffset);
                        break;
                case DoorType.left:
                    player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position);
                    break;
                case DoorType.right:
                    player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position);
                    break;
                case DoorType.top:
                    player.transform.position = new Vector2(transform.position.x, transform.position + widthOffset);
                    break;
            }
        }
   
     
    }

    */
  
}

