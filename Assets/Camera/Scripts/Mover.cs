using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rail rail;
    public PlayMode mode;

    
    public float speed = 2.5f;

    public bool isReversed;
    public bool isLooping;
    public bool pingPong;

    private int currentSeg;
    private float transition;
    private bool isCompleted;

    // I wrote this
    public void Start()

    {
       

        isLooping = true;
    }

    /////////////
    public void Increasespeed()
    {

        speed = 10f;
    }

    public void Decreasespeed()
    {

        speed = 0f;

    }

    /////////////


    private void Update()
    {
        if (!rail)
            return;

        if (!isCompleted)
        {
            Play(!isReversed);

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
