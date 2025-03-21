using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpotUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle HideToggle;
    public CanvasGroup ToolCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (HideToggle.isOn){
            ToolCanvas.alpha = 0;
            ToolCanvas.interactable = false;


        }else{

               ToolCanvas.alpha = 1;
            ToolCanvas.interactable = true;
        }
        
    }
}
