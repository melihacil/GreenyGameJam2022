using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    float fireRate, firePower, speed, hitPoint;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Speed(bool signal, float value)
    {
        if (signal) 
        {
            speed += value;
        }
        else speed -= value;
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
