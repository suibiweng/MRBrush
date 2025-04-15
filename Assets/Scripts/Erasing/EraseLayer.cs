using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RealityEditor;
using Anaglyph.DisplayCapture.Barcodes;
using Meta.WitAi;

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

    public GameObject Eraseindicator;

    public Transform player;

    public EraseUIcontrol eraseUIcontrol;

    Vector3 originalScale,adjscale;

    public SpatialPicture currentMsk;

    public float depthShader;

    // Start is called before the first frame update
    void Start()
    {
        realityEditorManager=FindObjectOfType<RealityEditorManager>();
        eraseUIcontrol=FindObjectOfType<EraseUIcontrol>();


        player=realityEditorManager.PlayerCamera;

        fast3DFunctions=FindAnyObjectByType<Fast3dFunctions>();
        photoMat=DepthPhoto.material;
        AimStar=GameObject.FindWithTag("AimStar");


        originalScale= Eraseindicator.transform.localScale;
        adjscale=new Vector3(0,0,0);


        


       


        

        
        
    }

    // Update is called once per frame
    void Update()
    {
           if(!eraseUIcontrol.eraseisOn){
                Eraseindicator.SetActive(false);
                return;
               


            
           }else{
            Eraseindicator.SetActive(true);

            Eraseindicator.transform.position=AimStar.transform.position;

             Eraseindicator.transform.LookAt(player);
           }
            
           


        if (OVRInput.GetDown(OVRInput.Button.One)){

            StartErase();


        }



        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick); // Or SecondaryThumbstick
        float x = input.x;
        float y = input.y;
        float scale=0.03f;
        // You can tweak this deadzone to your needs
        float threshold = 0.7f;
        if (y > threshold)
        {
            Debug.Log("Push Forward");

            adjscale.y+=scale;
            


        }
        else if (y < -threshold)
        {
            Debug.Log("Push Backward");

             adjscale.y-=scale;
        }
        else if (x < -threshold)
        {
            Debug.Log("Push Left");
              adjscale.x-=scale;
        }
        else if (x > threshold)
        {
            Debug.Log("Push Right");
               adjscale.x+=scale;
             
        }


        Eraseindicator.transform.localScale=originalScale+adjscale;



        if(currentMsk!=null){

        Vector2 input2 = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick); 

             float lx = input2.x;
             float ly = input2.y;







        if (ly > threshold)
        {
            Debug.Log("Push Forward");

            currentMsk.Scaleup(1f);
            

          
            


        }
        else if (ly < -threshold)
        {
            Debug.Log("Push Backward");

            currentMsk.Scaleup(-1f);

             //currentMsk.movingMaskZ(0,0,-0.1f);

            //  adjscale.y-=scale;
        }
        else if (lx < -threshold)
        {
            Debug.Log("Push Left");


           currentMsk.movingMaskZ(-0.01f,0,0);


            //currentMsk.setDepth(-0.001f);
            

            //  depthShader-=0.001f;
        }
        else if (lx > threshold)
        {

            currentMsk.movingMaskZ(0.01f,0,0);

            Debug.Log("Push Right");
          // currentMsk.setDepth(0.001f);
          
        }

       if (OVRInput.Get(OVRInput.Button.Three)) // X button
        {

currentMsk.movingMaskZ(0,-0.05f,0);

        //currentMsk.setDepth(-0.005f);
        Debug.Log("X button pressed");
        }
        if (OVRInput.Get(OVRInput.Button.Four)) // Y button
        {

currentMsk.movingMaskZ(0,0.05f,0);
       // currentMsk.setDepth(0.005f);
        Debug.Log("Y button pressed");
        }


    float leftTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
    float leftGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);

    float rightTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
    float rightGrip = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);








if (leftTrigger > 0.1f)
    {
        currentMsk.setDepth(-0.002f);
        Debug.Log("Left index trigger pressed with intensity: " + leftTrigger);
    }
    if (leftGrip > 0.1f)
    {
        Debug.Log("Left grip pressed with intensity: " + leftGrip);
        currentMsk.setDepth(0.002f);
    }





















        }

        // if(!Captured)
        //    photoMat.SetTexture("_RGBMAP",CameraTexture.texture);


        //  if(Input.GetKeyDown(KeyCode.Space)){

        //     StartErase();


        //  }
        
    }
    

    public bool Captured =false;

   public void StartErase(){







        if(currentMsk!=null){
            GameObject b= currentMsk.gameObject;
            currentMsk=null;
            Destroy(b);

        }

    
    

        URLID=IDGenerator.GenerateID(); 








       if(!realityEditorManager.forMovie){

                fast3DFunctions.UploadErase("http://192.168.0.139:5000/EraseMask",URLID+"_eraseRGB.png",new Vector2(1280/2,960/2),URLID);
        // this.BroadcastMessage("getSpatialTexture",URLID);

        Transform t= DepthPhoto.gameObject.transform;
        GameObject Cover = Instantiate(EraseMsk,AimStar.transform.position,t.rotation);

        Cover.transform.LookAt(player);
        SpatialPicture sp=Cover.GetComponent<SpatialPicture>();
        sp.URLID=URLID;
        sp.HideSpot.transform.localScale=Eraseindicator.transform.localScale;
        currentMsk=sp;
        sp.getSpatialTexture(URLID);
        Captured=true;
        Eraseindicator.transform.localScale=originalScale;







       }else{


        StartCoroutine(DelayForMovive());
       }



    }


    IEnumerator DelayForMovive(){

        yield return new WaitForSeconds(5f);

        Transform t= DepthPhoto.gameObject.transform;
        GameObject Cover = Instantiate(EraseMsk,AimStar.transform.position,t.rotation);

        Cover.transform.LookAt(player);
        SpatialPicture sp=Cover.GetComponent<SpatialPicture>();
        sp.URLID=URLID;
        sp.HideSpot.transform.localScale=Eraseindicator.transform.localScale;
        currentMsk=sp;
        // sp.getSpatialTexture(URLID);
        Captured=true;
        Eraseindicator.transform.localScale=originalScale;




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
