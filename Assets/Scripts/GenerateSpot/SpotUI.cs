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

    // Update is called once per frame
    void Update()
    {

        if(spot!=null){

            modeImage.sprite=Modesprites[(int)spot.promptType];
            mode_text.text=modeTexts[(int)spot.promptType];


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
