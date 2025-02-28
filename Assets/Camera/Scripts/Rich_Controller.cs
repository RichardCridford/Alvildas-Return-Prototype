using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rich_Controller : MonoBehaviour {

    //The camera has a separate moving script to the train cart
    protected CameraMover mover;

    // for raycast for traincarts
    InputController input;
    [SerializeField] private int objectNumber;

    //camera bools
    private bool cameraLeft;
    private bool cameraRight;

    //Bool for grab button
    private bool grab;


    // Use this for initialization
    void Start()
    {
        mover = FindObjectOfType<CameraMover>();
        input = FindObjectOfType<InputController>();

    }

    void CheckInput()
    {
        cameraLeft = Input.GetButton("LeftButton");
        cameraRight = Input.GetButton("RightButton");

        grab = Input.GetButton("Grab");

    }



    void ControlCamera()
    {
        if (cameraLeft)
        {

            Debug.Log("LB Pressed");
            mover.speed = -60;

        }


        if (cameraRight)
        {
            Debug.Log("RB Pressed");
            mover.speed = 60;


        }

        

        //Newly added
        if (cameraLeft && cameraRight)
        {

            mover.speed = 0;


        }

        else if (!cameraLeft && !cameraRight)
        {

            mover.speed = 0;

        }

        
    }

    
    

    // At somepoint need to create a raycast, so that each object can be identfied and then dragged 
    void ControlGrab()
    {
        if (grab)
        {

            // Loop for taking any objects with the traincart tag and running the increase speed method on the Mover script 
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("traincart"))
            {
                go.GetComponent<Mover>().Increasespeed();

            }




        }

        if (!grab)
        {

            // Loop for taking any objects with the traincart tag and running the decrease speed method on the Mover script 
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("traincart"))
            {
                go.GetComponent<Mover>().Decreasespeed();

            }


        }



    }

    //Start of method for moving traincarts by dragging
    void ManageGrab ()
    {
        objectNumber = input.GetNumberFromRaycast();


        if (objectNumber == 3)
        {
            Debug.Log("Train cart ready ");
        
        }

    }


    

    // Update is called once per frame
    void Update ()
    {
        CheckInput();
        ControlCamera();
        ControlGrab();

        
        ManageGrab();

    }
}
