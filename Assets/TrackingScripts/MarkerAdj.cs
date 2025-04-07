using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAdj : MonoBehaviour
{
    public float scaleOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(transform.position.x*scaleOffset
                                      ,transform.position.y*scaleOffset,
                                      transform.position.z*scaleOffset);
        
    }
}
