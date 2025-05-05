using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    //for setting up the camera trigger script
    [SerializeField] Camera_Trigger camera_trigger;
    private int cameraTriggerNumber;  
    
    
    
    [SerializeField] Rail rail1;
    [SerializeField] Rail rail2;
    private Rail rail;

    

    public PlayMode mode;

    
    public float speed = 0f;

    public bool isReversed;
    public bool isLooping;
    public bool pingPong;

    private int currentSeg;
    private float transition;
    private bool isCompleted;

    // I wrote this
    public void Start()
    {

        
        rail = rail1;


        isLooping = true;
    }






    private void Update()
    {
        if (!rail)
            return;

        if (!isCompleted)
        {
            Play(!isReversed);

        }

        DetectTrigger();
     
    
    
    }


   
    // Method to see if Alvilda is stepping on the trigger 
    private void DetectTrigger()
    {
        
        camera_trigger.GetAlvildaTrigger();

        if (camera_trigger.GetAlvildaTrigger())
        {
            cameraTriggerNumber = camera_trigger.GetCameraTriggerNumber();

            if (cameraTriggerNumber == 1)
            {
                //changes to the second camera rail, as that's all we have, ready to add additional trigger/rails if necessary
                rail = rail2;

            }
            
        }


    }




    private void Play(bool forward = true)
    {
        // for normalising speed when considering different lengths of the rail (camera on a 10 Meter isn't faster than a camera on a 1 Meter rail)
        float m = (rail.nodes[currentSeg + 1].position - rail.nodes[currentSeg].position).magnitude;
        float s = (Time.deltaTime * 1 / m) * speed;


        transition += (forward) ? s : -s ; 

        if (transition > 1)
        {
            transition = 0;
            currentSeg++;

            // If the last node
            if (currentSeg == rail.nodes.Length -1)
            {
                if (isLooping)
                {
                    if (pingPong)
                    {
                        transition = 1;
                        currentSeg = rail.nodes.Length - 2;
                        isReversed = !isReversed;
                    }
                    else
                    {
                        currentSeg = 0;

                    }

                }
                else
                {

                    isCompleted = true;
                    return;
                }
              }
           }



        //halfway
        else if (transition < 0)
        {
            transition = 1;
            currentSeg--;

            // If the last node
            if (currentSeg == - 1)
            {
                if (isLooping)
                {
                    if (pingPong)
                    {
                        transition = 0;
                        currentSeg = 0;
                        isReversed = !isReversed;
                    }
                    else
                    {
                        currentSeg = rail.nodes.Length - 2;

                    }

                }
                else
                {

                    isCompleted = true;
                    return;
                }


            }

        }

        transform.position = rail.PositionOnRail(currentSeg, transition, mode);
        transform.rotation = rail.Orientation(currentSeg, transition);
    }

}
