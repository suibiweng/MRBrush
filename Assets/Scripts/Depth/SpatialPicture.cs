using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RealityEditor;

public class SpatialPicture : MonoBehaviour
{
    ReConstructSpot spot;

    RealityEditorManager manager;
    public MeshRenderer meshRenderer;
    
    public Material material;


    public Coroutine fileCheck;

    string donwloadurl;
    bool lookat=false;


    public Transform HeadCamera;

    public GameObject HideSpot;
    // Start is called before the first frame update
    void Start()
    {
         manager = FindObjectOfType<RealityEditorManager>();

        donwloadurl=spot.DownloadURL;

        HeadCamera=manager.PlayerCamera;

        

       meshRenderer=GetComponent<MeshRenderer>();
       
       material=meshRenderer.materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(!lookat){
            meshRenderer.gameObject.transform.LookAt(HeadCamera.position);
        }
        
    }


    public void getSpatialTexture(){
         lookat=true;

       StartCoroutine(DownloadTextures(donwloadurl+spot.URLID+"@"+spot.Version+"_EraseRGB.png",
        donwloadurl+spot.URLID+"@"+spot.Version+"_EraseDepth.png"));


    }
    





   public IEnumerator DownloadTextures(string textureRGB, string textureDepth)
{
    bool rgbSuccess = false;
    bool depthSuccess = false;

    // Continue looping until both textures are successfully downloaded
    while (!rgbSuccess || !depthSuccess)
    {
        // Wait before trying again (prevents spamming the server)
        yield return new WaitForSeconds(5f);

        if (!rgbSuccess)
        {
            UnityWebRequest www1 = UnityWebRequestTexture.GetTexture(textureRGB);
            yield return www1.SendWebRequest();

            if (www1.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download texture 1: " + www1.error);
            }
            else
            {
                Texture2D texture1 = DownloadHandlerTexture.GetContent(www1);
                material.SetTexture("_RGBMAP", texture1);
                rgbSuccess = true;
            }
        }

        if (!depthSuccess)
        {
            UnityWebRequest www2 = UnityWebRequestTexture.GetTexture(textureDepth);
            yield return www2.SendWebRequest();

            if (www2.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to download texture 2: " + www2.error);
            }
            else
            {
                Texture2D texture2 = DownloadHandlerTexture.GetContent(www2);
                material.SetTexture("_DepthMap", texture2);
                depthSuccess = true;
            }
        }

       
    }

    // When both downloads are successful, the coroutine naturally ends.
    }
}
