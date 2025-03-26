using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using RealityEditor;
public class FaceToUser : MonoBehaviour
{
    RealityEditorManager realityEditorManager;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

        realityEditorManager=FindAnyObjectByType<RealityEditorManager>();
        player=realityEditorManager.PlayerCamera.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(
            new Vector3(player.transform.position.x,transform.position.y,player.transform.position.z)
            );
    }
}
