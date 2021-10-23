using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<JumpMan>().RemoveHealth(1);
            BreakPlatform();
        }

    }

    void BreakPlatform()
    {
        Destroy(gameObject);
    }
}
