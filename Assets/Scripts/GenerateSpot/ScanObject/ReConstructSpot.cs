using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityEditor;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.UI;
using DimBoxes;
using Oculus.Interaction;
using Klak.Ndi.Interop;
using System.Drawing;

public enum PromptType{
    DreamMesh=0,
    Material=1,
    Drawing=2,
    Reconstruction=3,
    Erase=4,
    DrawOnly=5


}



public class ReConstructSpot : MonoBehaviour
{

     public Toggle isTracking;
    public Transform Tracker;

    //Version ControlUI
    public TMP_Text VersionInfoText;

    public Slider SizeAdjustment;
    public Slider TransparentAdjustment;


    public Toggle MaskDrawing;


    
    



    public ToggleGroup PromptModes;
    
    public PromptType promptType;
    
    public int Version;

    public List<string> versionIds;

    public RealityEditorManager manager;

    public TMP_Text promptText;

    public string prompt;


    public GameObject Target;
    public ModelDownloader modelDownloader;

    public Fast3dFunctions fast3DFunctions;


    public string URLID;

    public string serverURL;
    // Start is called before the first frame update



    public ParticleSystem loadingParticles;


   Coroutine FileCheck;

   public string DownloadURL="";
   public string UploadURL="";
   public string commandURL="";

   public string EraseURL="";

   public string Drawingto3DURL="";

   public BoundBox boundBox;
   public bool isselsected=false;
  public Grabbable _grabbable;

  public Toggle debugShow;

  public TMP_Text DebugMsg;

  public VoiceLabel voiceLabel;

  public DrawingSystem drawingSystem;

  public Shader thePresetShader;

  public string debugPrompt="Red Apple";

   public List<GameObject> ObjectsVersion ;
    
   private bool TextureChanging=false;
   public GameObject AimStar;


   public bool isStrokesOnly;

    void Start()
    {

        ObjectsVersion= new List<GameObject>();
        AimStar=GameObject.FindWithTag("AimStar");
        Tracker=GameObject.FindWithTag("Tracker").transform;
    




       
        versionIds=new List<string>();
        drawingSystem=  FindObjectOfType<DrawingSystem>();
        manager = FindObjectOfType<RealityEditorManager>();
        modelDownloader = FindObjectOfType<ModelDownloader>();
        fast3DFunctions= FindObjectOfType<Fast3dFunctions>();
        DownloadURL=manager.ServerURL;
        UploadURL=manager.ServerURL;
        commandURL=manager.ServerURL;
        EraseURL=manager.ServerURL;
        Drawingto3DURL=manager.ServerURL;

        //DownloadURL+=":"+manager.downloadPort+"/";
        DownloadURL+=":"+"8000/";
        UploadURL+=":"+manager.Port+"/upload";
        commandURL+=":"+manager.Port+"/command";
        EraseURL+=":"+manager.Port+"/EraseMask";
        Drawingto3DURL+=":"+manager.Port+"/DrawToModel";



        _grabbable.WhenPointerEventRaised += HandlePointerEventRaised;
        Version=0;

        //StartCoroutine(StrokeAttach());


        //  FileCheck= StartCoroutine(CheckURLPeriodically("http://192.168.0.139:8000/20250318172841370b55bf@1_Texture.zip"));
    
    
    // DrawingToModel();
    }


    public void GetStrokes(){


        if(isselsected){

  
        GameObject [] targetObject = GameObject.FindGameObjectsWithTag("Stroke");
        if (targetObject != null)
        {
            foreach(var s in targetObject){

                 s.transform.SetParent(Target.transform,true);



            }

           
            Debug.Log("Object attached successfully.");
        }



          }



    }


    public void setDrawing(bool set){

        isStrokesOnly=set;



    }




    IEnumerator StrokeAttach()
    {

        // if( promptType!=PromptType.Drawing)  yield return null;

        yield return new WaitForSeconds(0.5f); // Small delay to allow object instantiation

    if(isselsected){
        GameObject targetObject = GameObject.FindWithTag("Stroke");
        if (targetObject != null)
        {

            targetObject.transform.SetParent(Target.transform,true);
            Debug.Log("Object attached successfully.");
        }



          }

    }


   


    void ObjectListUpdate(){

        // if there is no new object in the list, return
    if (ObjectsVersion.Count == Target.transform.childCount) return;
    else{
        //deavtivate the previous object
        foreach (GameObject obj in ObjectsVersion)
        {
            obj.SetActive(false);
        }

    }




    if(VersionInfoText!=null) 

    VersionInfoText.text="Version: "+currentVersion+"(Total Version:)"+ObjectsVersion.Count;

    currentVersion=Version;


        // add the object not in the list and 
    foreach (Transform child in Target.transform)
    {
            if(!ObjectsVersion.Contains(child.gameObject))
            ObjectsVersion.Add(child.gameObject);
            if(!TextureChanging)
                child.gameObject.GetComponentInChildren<MeshRenderer>().material.shader=thePresetShader;



    }

         if(loadingParticles!=null)
          loadingParticles.Stop();

        TextureChanging=false;

    /*
    // Shaer change on the new object
    foreach (GameObject obj in ObjectsVersion)
    {
        Material [] materials = obj.GetComponentInChildren<MeshRenderer>().materials;

        foreach (Material material in materials){

             material.shader = thePresetShader;     
        }
    }
*/












    }

    


    void turnoffall(){
        foreach(var obj in ObjectsVersion){

            obj.SetActive(false);


        }
    }
    //
    int currentVersion=0;
    public void PreviousVersion(){
        if(VersionInfoText!=null) 
        VersionInfoText.text="Version: "+currentVersion+"(Total Version:)"+ObjectsVersion.Count;
        turnoffall();
        //activate the previous object
        if (currentVersion > 0)
        {
           
            currentVersion--;
            ObjectsVersion[currentVersion].SetActive(true);
        }
    }

    //
    public void NextVersion(){

        if(VersionInfoText!=null) 

        VersionInfoText.text="Version: "+currentVersion+"(Total Version:)"+ObjectsVersion.Count;

         turnoffall();




        //activate the next object
        if (currentVersion < ObjectsVersion.Count - 1)
        {

            currentVersion++;
            ObjectsVersion[currentVersion].SetActive(true);
        }


    }



  public void ClearAllChildren()
    {
        return;
        GameObject parentObject=Target;
        Preseting=false;
        // Check if the parentObject is assigned
        if (parentObject != null)
        {
            // Check if the parent has children
            if (parentObject.transform.childCount > 0)
            {
                // Loop through all children and destroy them
                foreach (Transform child in parentObject.transform)
                {
                    Destroy(child.gameObject);
                }
                Debug.Log("All children of " + parentObject.name + " have been cleared.");
            }
            else
            {
                Debug.Log("The parent " + parentObject.name + " has no children to clear.");
            }
        }
        else
        {
            Debug.LogWarning("Parent GameObject is not assigned!");
        }
    }



    public void selectedMode(int Mode){

      PromptType t= (PromptType)Mode;


         promptType=t;



    }



    void UIsetup(){
        if(PromptModes==null) return;


         switch(PromptModes.GetFirstActiveToggle().name){
            case "DreamMesh":
               promptType=PromptType.DreamMesh;
            break;


            case "MaterialChanger":
             promptType=PromptType.Material;
            break ;


            case "Drawing":
             promptType=PromptType.Drawing;
            break;

            case "Reconstruct":

             promptType=PromptType.Reconstruction;

             break;



         }


    }

    public void DrawTo3D(){

        //fast3DFunctions.UploadDrawing("http://192.168.0.139:5000/DrawToModel",URLID+"@"+Version+"_Darwing3D.png",prompt,ObjectScreenPosition(),URLID+"@"+Version);
        fast3DFunctions.UploadDrawing(Drawingto3DURL,URLID+"@"+Version+"_Darwing3D.png",prompt,ObjectScreenPosition(),URLID+"@"+Version);

        if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + URLID+"@"+Version + "_Drawing.zip"));

        loadingParticles.Play();








    }



    







    public void SendThePrompt(){
        if(!isselsected) return;


        if(loadingParticles!=null)
            loadingParticles.Play();

            GetComponent<SpotUI>().HideToggle.isOn=true;

        switch (promptType){
            case PromptType.DreamMesh:
                CreateDreamMesh();
            break;

            case PromptType.Drawing:
                if(!isStrokesOnly)
                DrawingToModel();
                else{

                    DrawTo3D();


                }


            break;



            case PromptType.Material:
            ChangeMaterial();
            break;

            case PromptType.Reconstruction:

            ReconstructionTheModel();
            break;

            case PromptType.Erase:
                CreareEraseMask();

            break;






        }






    }

    public void TransformUpdate(){
         if(SizeAdjustment==null)return;
          Target.transform.localScale=new Vector3(1f+SizeAdjustment.value,1f+SizeAdjustment.value,1f+SizeAdjustment.value);
    }

    

    void tracking(){

        if(isTracking==null) return;

        if(isTracking.isOn){

            

            transform.position = Tracker.position/10;




        }else{



            transform.SetParent(null, true); 


            
        }



    }




    

    // Update is called once per frame
    void Update()
    {


        tracking();



        TransformUpdate();

    
        if(debugShow!=null)
        debugShow.isOn = isselsected;





       
        ObjectListUpdate();

        // PresetTheDownloadedModel();




        if (OVRInput.GetDown(OVRInput.Button.Two)){
            if( GetComponent<SpotUI>().HideToggle.isOn==false)
                    SendThePrompt();

        }








        // if (Input.GetKeyDown(KeyCode.P)){
           
        //           DrawingToModel();

        // }


        


        

    



       // prompt= manager.VoiceToPrompt;

        
        
        prompt=promptText.text;
        DebugMsg.text=prompt;

       // UIsetup();




    }

    bool Preseting=false;


    void PresetTheDownloadedModel(){

        if (Preseting)return;


        Material [] materials = Target.GetComponentInChildren<MeshRenderer>().materials;

        foreach (Material material in materials){

             material.shader = thePresetShader;


        }



        Preseting=true;

    }








    public void setVoiceInput(){

        prompt= voiceLabel.Label.text;

        
        





    }


    private void HandlePointerEventRaised(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                OnSelect();
                
                break;
            case PointerEventType.Unselect:

            // PreviewWindow.gameObject.SetActive(false);
             Release();

                break;
        }
    }


        public void OnSelect()
    {

        manager.updateSelected( URLID);
        isselsected = true;
       

    }

    public void   Release(){

         isselsected = false;


    }


    

Vector2 ObjectScreenPosition()
{
    // Convert this GameObject's position to 2D screen coordinates
    Vector3 screenPosition = fast3DFunctions.MaskCamera.WorldToScreenPoint(AimStar.transform.position);

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




bool Capturing=false;


public void CreateDreamMesh(){

    fast3DFunctions.DreamMesh(commandURL,URLID+"@"+Version,prompt);
         if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + URLID+"@"+Version + "_ShapE.zip"));

}


public void CreareEraseMask(){
     Vector2 TargetPos=ObjectScreenPosition();


    fast3DFunctions.UploadErase(EraseURL,URLID+"@"+Version+"_eraseRGB.png",TargetPos,URLID);

    //BroadcastMessage("getSpatialTexture"); 

        // if(FileCheck==null)
        // FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + URLID+"@"+Version + "_ShapE.zip"));
}


public void ReconstructionTheModel(){


    if(!manager.forMovie)
        StartGeneration();

    else{


        if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + "20250406175338f3a773eeMovie.zip"));



    }



}

public void ChangeMaterial(){


     if(!manager.forMovie){

           fast3DFunctions.ChangeMaterial(commandURL,URLID+"@"+Version,prompt);
         if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + URLID+"@"+Version + "_Texture.zip"));
     }

     else{


        // fast3DFunctions.ChangeMaterial(commandURL,"20250406175338f3a773eeMovie",prompt);
         if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + "20250406175338f3a773eeMovie_Texture.zip"));



     }

     



}


public void DrawingToModel(){
   
   if(manager.forMovie)
    modifywithPrompt();

    else{


        //fast3DFunctions.UploadDrawing("http://192.168.0.139:5000/DrawToModel",URLID+"@"+Version+"_Darwing3D.png",prompt,ObjectScreenPosition(),URLID+"@"+Version);
        fast3DFunctions.ObjwithDrawing(Drawingto3DURL,URLID+"@"+Version+"_Darwing3D.png",prompt,ObjectScreenPosition(),URLID+"@"+Version);

        if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + URLID+"@"+Version + "_Drawing.zip"));

        loadingParticles.Play();



    }


}







    public void StartGeneration(){

        if(!isselsected) return;
        //ClearAllChildren();

        if(!Capturing)
            StartCoroutine(CaptureRouting());

        // fast3DFunctions.Capture(UploadURL,URLID+".png",ObjectScreenPosition(),URLID);
        // fast3DFunctions.UploadMask(UploadURL,URLID+"_Mask.png","MaskTest",ObjectScreenPosition(),URLID);         
      //FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL + URLID + "_reconstruct.zip"));
    }

    public void modifywithPrompt(){



        //     if(!isselsected) return;
        //  ClearAllChildren();
            if(!Capturing)
                StartCoroutine(MaskWithPrompt());









        }





        IEnumerator MaskWithPrompt(){
     //prompt=promptText.text;

        Capturing=true;
        prompt=voiceLabel.Label.text;


       

        Vector2 TargetPos=ObjectScreenPosition();
        // fast3DFunctions.ToggleCullingMask();
        yield return new WaitForSeconds(0.3f);
       fast3DFunctions.ModifyCapture(UploadURL,URLID+"@"+Version+"_Modify.png",TargetPos,URLID+"@"+Version);
        
        yield return new WaitForSeconds(0.3f);
        fast3DFunctions.UploadMask(UploadURL,URLID+"@"+Version+"_Mask.png",prompt,TargetPos,URLID+"@"+Version); 

          
         //yield return new WaitForSeconds(0.3f);
        //fast3DFunctions.CaptureDepth(UploadURL,URLID+"_Depth.png",TargetPos,URLID);
        // yield return new WaitForSeconds(0.3f);
        // fast3DFunctions.ToggleCullingMask();
        if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL+"/" + URLID+"@"+Version + "_reconstruct.zip"));
        // drawingSystem.ClearAndDestroyStackObjects();

        Capturing=false;


        }






    IEnumerator CaptureRouting(){
        Capturing=true;


        Vector2 TargetPos =ObjectScreenPosition();
       // fast3DFunctions.ToggleCullingMask();
        yield return new WaitForSeconds(0.3f);

        if(MaskDrawing!=null)fast3DFunctions.Capture(UploadURL,URLID+"@"+Version+".png",TargetPos,URLID+"@"+Version,MaskDrawing.isOn);

        else
        fast3DFunctions.Capture(UploadURL,URLID+"@"+Version+".png",TargetPos,URLID+"@"+Version);
        //fast3DFunctions.UploadDepthMap(UploadURL,URLID+"@"+Version+"_Depth.png",TargetPos,URLID+"@"+Version);
        


       // fast3DFunctions.sendCommand(commandURL,"IpcamCapture",URLID);
          //yield return new WaitForSeconds(0.3f);
          //fast3DFunctions.UploadMask(UploadURL,URLID+"_Mask.png","MaskTest",TargetPos,URLID);   
         //yield return new WaitForSeconds(0.3f);
        //fast3DFunctions.CaptureDepth(UploadURL,URLID+"_Depth.png",TargetPos,URLID);
        yield return new WaitForSeconds(0.3f);
      // fast3DFunctions.ToggleCullingMask();


        if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL +"/"+ URLID+"@"+Version + "_reconstruct.zip"));


        Capturing=false;




    }




    public void Delete(){

        manager.RemoveReConSpot(URLID);
        var hacking= FindFirstObjectByType<OpenBrushHackforRE>();

        if(hacking!=null){
            hacking.toDestroySpot(gameObject);


        }


    }



    public void ChangeTheMaterial(){



        if(FileCheck==null)
            FileCheck= StartCoroutine(CheckURLPeriodically(DownloadURL +"/"+ URLID+"@"+Version + "_Modify.zip"));


    }




    








    public void downloadModel(string url, GameObject warp)
    {
        modelDownloader.AddTask(
            new ModelIformation()
            {
                ModelURL = url,
                gameobjectWarp = warp
            }
        );

        // loadingIcon.SetActive(false);
        // loadingParticles.Stop();
        // SmoothCubeRenderer.enabled = false;

        modelDownloader.startDownload();
    }




      IEnumerator CheckURLPeriodically(string urltocheck)
    {
        yield return new WaitForSeconds(10f);
        while (true)
        {
            yield return CheckURL(urltocheck);
            yield return new WaitForSeconds(checkInterval);
        }
    }

     public float checkInterval = 5f; // Check the URL every 5 seconds
    public event Action<bool> OnURLResponse = delegate { };

     IEnumerator CheckURL(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequestAsyncOperation requestAsyncOperation = www.SendWebRequest();

        while (!requestAsyncOperation.isDone)
        {
            yield return null;
        }

        if (www.result == UnityWebRequest.Result.Success)
        {
            if(url.Contains("_Texture")){
                    TextureChanging=true;
            }
            Debug.Log("URL is responding!");






            StopCoroutine(FileCheck);
            FileCheck=null;

            downloadModel(url, Target);
            Version++;







            OnURLResponse(true);
        }
        else
        {
            //  Debug.LogError("Error checking URL: " + www.error);
            OnURLResponse(false);
        }

        www.Dispose();
    }



}
