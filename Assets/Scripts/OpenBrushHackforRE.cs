using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiltBrush;
using RealityEditor;
public class OpenBrushHackforRE : MonoBehaviour
{   
    public RealityEditorManager REmanager;
    public SketchControlsScript sketchControlsScript;

    public PointerManager pointerManager;
    // Start is called before the first frame update
    void Start()
    {
            PanelManager.m_Instance.ToggleSketchbookPanels();
            App.Instance.ExitIntroSketch();
            PromoManager.m_Instance.RequestAdvancedPanelsPromo();
        
    }
    bool triggeronce=false;

    // Update is called once per frame
    void Update()
    {
        if(REmanager.GetSelected()!=null){
            GameObject spot=REmanager.GetSelected();
            ReConstructSpot re=spot.GetComponent<ReConstructSpot>();

            if(re.promptType!=PromptType.Drawing){

                sketchControlsScript.RequestPanelsVisibility(false);

                  if(triggeronce)
                {
                pointerManager.SetPointersRenderingEnabled(false);

                triggeronce=false;}

            }else{

                sketchControlsScript.RequestPanelsVisibility(true);

                if(!triggeronce)
                {

                      pointerManager.SetPointersRenderingEnabled(true);
                
            triggeronce=true;


                }
                  

                
               
            }





        
        
        
        
        
        
        }
        
        
    }
}
