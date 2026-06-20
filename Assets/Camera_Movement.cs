using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    //The postion of the player and the offset of the camera
    public Transform player;
    public Vector3 offset;
    // Update is called once per frame
    void LateUpdate()
    {
        //The camera moves to the player's current position
        transform.position = player.position + offset;
    }
}