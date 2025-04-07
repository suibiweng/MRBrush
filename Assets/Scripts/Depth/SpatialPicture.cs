using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RealityEditor;


public class SpatialPicture : MonoBehaviour
{
    public string URLID;
    // ReConstructSpot spot;

    RealityEditorManager manager;
    public MeshRenderer meshRenderer;
    
    public Material material;


    public Coroutine fileCheck;

    string donwloadurl;
    bool lookat=false;


    public Transform HeadCamera;

    public GameObject HideSpot;
    public GameObject Cover;

    public float currentDepth;

    
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<RealityEditorManager>();

        donwloadurl="http://192.168.0.139:8000/";

     //   HeadCamera=manager.PlayerCamera;


        // getSpatialTexture("20250326201534000c2a5f");

        

    //    meshRenderer=GetComponent<MeshRenderer>();

       
       
       material=meshRenderer.materials[0];


       currentDepth= material.GetFloat("_Power");

        if(!manager.forMovie)
        StartCoroutine(CheckAndUpdateTexturesPeriodically(donwloadurl+URLID+"_EraseMask.png",donwloadurl+URLID+"_Remove_Depth.png"));
    }

    // Update is called once per frame
    void Update()
    {
        // if(!lookat){
        //     meshRenderer.gameObject.transform.LookAt(HeadCamera.position);
        // }
        
    }



    public void Scaleup(float s){


    Cover.transform.localScale+=new Vector3(s,s,s);




    }

    public void movingMaskZ(float x,float y, float z){


        Cover.transform.localPosition+=new Vector3(x,y,z);




    }

    public void setDepth(float z){

        currentDepth+=z;

        material.SetFloat("_Power",currentDepth);



    }


    public void getSpatialTexture(string urlid){

    //    StartCoroutine(DownloadTextures(donwloadurl+URLID+"_EraseMask.png",donwloadurl+URLID+"_Remove_Depth.png"));

        URLID=urlid;


       StartCoroutine(CheckAndUpdateTexturesPeriodically(donwloadurl+URLID+"_EraseMask.png",donwloadurl+URLID+"_Remove_Depth.png"));

    }
    





  

        private bool rgbSuccess = false;
    private bool depthSuccess = false;
       public float checkInterval = 5f;
int rgbTries = 0;
int depthTries = 0;
 IEnumerator CheckAndUpdateTexturesPeriodically(string rgb, string depth)
    {
        yield return new WaitForSeconds(2f); // optional delay before first check

        while (!rgbSuccess || !depthSuccess)
        {
            if (!rgbSuccess)
            {
                rgbTries++;
                Debug.Log($"🔄 RGB try #{rgbTries}");
                yield return StartCoroutine(DownloadAndApplyTexture(rgb, "_RGBMAP", true));
            }

            if (!depthSuccess)
            {
                depthTries++;
                Debug.Log($"🔄 Depth try #{depthTries}");
                yield return StartCoroutine(DownloadAndApplyTexture(depth, "_DepthMap", false));
            }

            if (!rgbSuccess || !depthSuccess)
                yield return new WaitForSeconds(checkInterval);
        }

        Debug.Log("✅ Both RGB and Depth textures successfully downloaded and applied.");
    }

    IEnumerator DownloadAndApplyTexture(string url, string textureProperty, bool isRGB)
    {
        Debug.Log($"➡️ Attempting to download from {url}");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);

            if (texture != null)
            {
                if (material == null)
                {
                    Debug.LogError("❗ Material not assigned!");
                    yield break;
                }

                material.SetTexture(textureProperty, texture);

                if (isRGB)
                {
                    rgbSuccess = true;
                    Debug.Log("✅ RGB texture set.");
                }
                else
                {
                    depthSuccess = true;
                    Debug.Log("✅ Depth texture set.");
                }
            }
            else
            {
                Debug.LogError($"❌ Texture from {url} was null. Will retry.");
            }
        }
        else
        {
            Debug.LogError($"❌ Could not fetch {url}. Error: {www.error} | Result: {www.result} | Code: {www.responseCode}");
        }

        www.Dispose();
    }
}
