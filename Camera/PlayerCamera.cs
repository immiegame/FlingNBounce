using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject leftWall;
    public GameObject rightWall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftWall.transform.position = new Vector3(leftWall.transform.position.x, transform.position.y, 0);
        rightWall.transform.position = new Vector3(rightWall.transform.position.x, transform.position.y, 0);
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(0, player.transform.position.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 20 * Time.deltaTime);
        transform.position = smoothedPosition;
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }
}
