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
        
        StartCoroutine(Wait(0.1f));
        
        Destroy(bc);
    }
    private IEnumerator Wait (float seconds)
    {
        yield return new WaitForSeconds(seconds);
        anim.SetBool("smash", true);


    }
}
