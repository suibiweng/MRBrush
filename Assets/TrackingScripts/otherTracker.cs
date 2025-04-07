using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otherTracker : MonoBehaviour
{
    public Transform ARUCOTracker;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position=ARUCOTracker.position;
        transform.rotation=ARUCOTracker.rotation;
        
    }
}
