using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Trigger : MonoBehaviour 
{
    //This script can be used to make triggers for the camera, may need to destroy the triggers at a later date
    
    
    [SerializeField ] bool alvildaTrigger = false;

    // This number is an identifier of the Camera Trigger - ready for multiple triggers
    [SerializeField] int cameraTriggerNumber;


    // Gameobject you can switch on from the camera trigger
    [SerializeField] GameObject itemTrigger;


    void Start()
    {
        if (itemTrigger != null)
        {

            itemTrigger.SetActive(false);
        }
    
    }
    



    
    private void OnTriggerEnter(Collider collision)
    {

        
        if (collision.tag == "alvilda")
        {
            alvildaTrigger = true;
            
            
        }

        if (itemTrigger != null)
        {

            itemTrigger.SetActive(true);
        }
    }

    public bool GetAlvildaTrigger() { return alvildaTrigger; }

    public int GetCameraTriggerNumber() { return cameraTriggerNumber; }

}
