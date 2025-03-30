using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RealityEditor;
using UnityEngine.UI;
public class EraseUIcontrol : MonoBehaviour
{
    public Toggle EraseOn;

    public Button Createbutton;

    public bool eraseisOn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       eraseisOn= EraseOn.isOn;

       if(eraseisOn){

        Createbutton.interactable=false;


       }else{

        Createbutton.interactable=true;



       }
        


        
    }
}
