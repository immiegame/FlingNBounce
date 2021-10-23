using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whoosh : MonoBehaviour
{
    public float PresentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PresentTime += Time.deltaTime;
        if (PresentTime > 0.2)
        {
            Destroy(gameObject);
        }
    }
}
