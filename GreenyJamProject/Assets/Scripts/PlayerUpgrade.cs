using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    float fireRate, firePower, speed, hitPoint;
    [SerializeField] GameObject player;
    PlayerMovement pMovement;
    
    void Start()
    {
        pMovement = player.GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Speed(false, 0.1f);
    }

    void Speed(bool signal, float value)
    {
        if (signal) 
        {
            pMovement.movementSpeed += value;
        }
        else pMovement.movementSpeed -= value;
    }

    void HitPoint(bool signal, float value)
    {
        if (signal) 
        {
            hitPoint += value;
        }
        else hitPoint -= value;
    }

    
}
