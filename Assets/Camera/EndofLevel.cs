using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndofLevel : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 10f;
    [SerializeField] AlvildaController alvildaController;
    
    public GameObject Mymousepointer;

     private void OnTriggerEnter(Collider collision)
    {


        if (collision.tag == "alvilda")
        {

            //UnityEngine.Debug.Log("Alvilda entered box collider");
            alvildaController.AlvildaMovementOverride();

            StartCoroutine(WaitAndLoad());


        }
    }

    
    
        


    IEnumerator WaitAndLoad()
    {
        // Stop Alvilda moving
        alvildaController.AlvildaMovementOverride();

        // Turn off mouse cursor
        Mymousepointer.SetActive(false);


        // Wait
        yield return new WaitForSeconds(delayInSeconds);
        
        // Load the next available scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
