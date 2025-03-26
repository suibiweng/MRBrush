using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RealityEditor;

public class EraseLayer : MonoBehaviour
{
    public Renderer DepthPhoto;

    public RawImage CameraTexture;

    private Material photoMat;


    public Fast3dFunctions fast3DFunctions; 

    public Transform Eraseobj;

    public string URLID;

    // Start is called before the first frame update
    void Start()
    {

        fast3DFunctions=FindAnyObjectByType<Fast3dFunctions>();


        photoMat=DepthPhoto.material;


        


       


        

        
        
    }

    // Update is called once per frame
    void Update()
    {
         photoMat.SetTexture("_RGBMAP",CameraTexture.texture);
        
    }


   public void StartErase(){

        URLID=IDGenerator.GenerateID(); 

        

        fast3DFunctions.UploadErase("",URLID+"@"+"_eraseRGB.png", ObjectScreenPosition(Eraseobj),URLID);





    }


    Vector2 ObjectScreenPosition(Transform pos)
{
    // Convert this GameObject's position to 2D screen coordinates
    Vector3 screenPosition = fast3DFunctions.MaskCamera.WorldToScreenPoint(pos.position);

    // Check if the object is in front of the camera
    if (screenPosition.z > 0)
    {
        Vector2 screenPosition2D = new Vector2(screenPosition.x, screenPosition.y);
        Debug.Log("Screen Position (2D): " + screenPosition2D);
        return screenPosition2D;
    }
    else
    {
        Debug.Log("Object is behind the camera.");
        return Vector2.zero; // Or any value you choose to represent an invalid position
    }
}
}
