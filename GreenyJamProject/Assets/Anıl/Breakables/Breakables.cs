using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    private Animator anim;
    public BoxCollider2D bc;
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }
    public void Destroy()
    {
        anim.SetBool("smash", true);
        Destroy(bc);
    }
}
