using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class EndofLevel : MonoBehaviour
{
    
    [SerializeField] AlvildaController alvildaController;
    
    public GameObject Mymousepointer;
    public Animator animator;

   
    
    private void OnTriggerEnter(Collider collision)
    {


        if (collision.tag == "alvilda")
        {
            // Stop Alvilda moving
            alvildaController.AlvildaMovementOverride();

            // Move to the EoL position
            //alvildaController.AlvildaEndLevel();

            // Turn off mouse cursor
            Mymousepointer.SetActive(false);

            // Set the bool to start the animation of the turning stones
            animator.SetBool("PlayOnce", true);

            
        }
    }  
}
