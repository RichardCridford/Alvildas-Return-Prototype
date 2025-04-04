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

        grab = Input.GetButton("Styx");

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

    
  
    

    // Update is called once per frame
    void Update ()
    {
        CheckInput();
        ControlCamera();


    }
}
