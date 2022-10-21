
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Veriables  
    public float speed;
    RigiBody2D rigiBody;   


    // Start is called before the first frame update
    void Start()
    {

        rigiBody = SetComponent<RigiBody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Imput.GetAxis("vertical");

        rigiBody.velocity = new Vector()

    }
}
