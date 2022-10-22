using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentalHazards : MonoBehaviour
{

    public Collider2D cd;
    
    void Start()
    {
    }

   
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Player>().takeDamage(0.5f);
        


    }

    
}
