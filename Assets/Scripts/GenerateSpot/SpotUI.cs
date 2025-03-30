using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RealityEditor;


public class SpotUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Toggle HideToggle;


    public Image modeImage;

    public Sprite [] Modesprites;
    public string [] modeTexts;

    public TMP_Text mode_text;
    public TMP_Text Status;

    ReConstructSpot spot;







    public CanvasGroup ToolCanvas;
    void Start()
    {
        spot=GetComponent<ReConstructSpot>();
        
    }

    void setMode(){

        switch (spot.promptType){
            case PromptType.DreamMesh:

            modeImage.sprite=Modesprites[0];
            mode_text.text=modeTexts[0];




            break;


            case PromptType.Material:

              modeImage.sprite=Modesprites[1];
            mode_text.text=modeTexts[1];

            break;



            case PromptType.Reconstruction:

                               modeImage.sprite=Modesprites[3];
            mode_text.text=modeTexts[3];

            break;




            case PromptType.Drawing:

                    modeImage.sprite=Modesprites[2];
            mode_text.text=modeTexts[2];

            break;















        }






  



    }

    // Update is called once per frame
    void Update()
    {

        if(spot!=null){




        }


        if (HideToggle.isOn){
            ToolCanvas.alpha = 0;
            ToolCanvas.interactable = false;


        }else{

               ToolCanvas.alpha = 1;
            ToolCanvas.interactable = true;
        }
        
    }
}
