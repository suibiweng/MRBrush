using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RealityEditor;

public class EraseLayer : MonoBehaviour
{

    RealityEditorManager realityEditorManager;
    public Renderer DepthPhoto;

    public RawImage CameraTexture;

    private Material photoMat;


    public Fast3dFunctions fast3DFunctions; 

    public Transform Eraseobj;

    public string URLID="";


    public GameObject EraseMsk;
    public GameObject AimStar;

    public Transform player;

    public EraseUIcontrol eraseUIcontrol;

    // Start is called before the first frame update
    void Start()
    {
        realityEditorManager=FindObjectOfType<RealityEditorManager>();
        eraseUIcontrol=FindObjectOfType<EraseUIcontrol>();


        player=realityEditorManager.PlayerCamera;

        fast3DFunctions=FindAnyObjectByType<Fast3dFunctions>();
        photoMat=DepthPhoto.material;
        AimStar=GameObject.FindWithTag("AimStar");


        


       


        

        
        
    }

    // Update is called once per frame
    void Update()
    {
           if(!eraseUIcontrol.eraseisOn)return;


        if (OVRInput.GetDown(OVRInput.Button.One)){

            StartErase();


        }

        // if(!Captured)
        //    photoMat.SetTexture("_RGBMAP",CameraTexture.texture);


        //  if(Input.GetKeyDown(KeyCode.Space)){

        //     StartErase();


        //  }
        
    }
    

    public bool Captured =false;

   public void StartErase(){



    

        URLID=IDGenerator.GenerateID(); 
        fast3DFunctions.UploadErase("http://192.168.0.139:5000/EraseMask",URLID+"_eraseRGB.png",new Vector2(1280/2,960/2),URLID);
        // this.BroadcastMessage("getSpatialTexture",URLID);



        Transform t= DepthPhoto.gameObject.transform;


        GameObject Cover = Instantiate(EraseMsk,AimStar.transform.position,t.rotation);

        Cover.transform.LookAt(player);
        SpatialPicture sp=Cover.GetComponent<SpatialPicture>();
        sp.URLID=URLID;

        // sp.getSpatialTexture(URLID);



        Captured=true;



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
