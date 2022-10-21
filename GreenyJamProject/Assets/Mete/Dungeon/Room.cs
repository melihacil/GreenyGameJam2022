using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int Width;
    public int Height;
    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instantiate == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCenter()
    {
        return new Vector3(x * Width, y * Height);
    }
}
