using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraseLayer : MonoBehaviour
{
    public Renderer DepthPhoto;

    public RawImage CameraTexture;

    private Material photoMat;

    // Start is called before the first frame update
    void Start()
    {


        photoMat=DepthPhoto.material;


        


       


        

        
        
    }

    // Update is called once per frame
    void Update()
    {
         photoMat.SetTexture("_RGBMAP",CameraTexture.texture);
        
    }
}
