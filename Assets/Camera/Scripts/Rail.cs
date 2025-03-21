﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Remove for build
using UnityEditor;

[ExecuteInEditMode]

public enum PlayMode
{
    Linear,
    Catmull,

}


public class Rail : MonoBehaviour

{

    public Transform[] nodes;

    private void Start()
    {
        nodes = GetComponentsInChildren<Transform>();

    }

    public Vector3 PositionOnRail(int seg, float ratio, PlayMode mode)
    {
        switch (mode)
        {
            default:
            case PlayMode.Linear:
                return LinearPosition(seg, ratio);
            case PlayMode.Catmull:
                return CatmullPosition(seg, ratio);

        }


    }

    //camera movement less smooth than Catmull, but less complex
    public Vector3 LinearPosition(int seg, float ratio)
    {
        //take two positions, lerp between them
        Vector3 p1 = nodes[seg].position;
        Vector3 p2 = nodes[seg + 1].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    //for smoothing camera movement using math function
    public Vector3 CatmullPosition(int seg, float ratio)
    {

        Vector3 p1, p2, p3, p4;

        //first segment
        if (seg == 0)
        {
            p1 = nodes[seg].position;
            p2 = p1;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }

        //last segment
        else if (seg == nodes.Length - 2)
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = p3;
        }

        //any other segment
        else
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }


        float t2 = ratio * ratio;
        float t3 = t2 * ratio;

        //clever math for Catmull curve
        float x = 
            0.5f * ((2.0f * p2.x) + 
            (-p1.x + p3.x) * ratio + 
            (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x) * t2 + 
            (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) * t3);

        float y =
           0.5f * ((2.0f * p2.y)
           + (-p1.y + p3.y)
           * ratio + (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y)
           * t2 + (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y)
           * t3);

        float z =
           0.5f * ((2.0f * p2.z)
           + (-p1.z + p3.z)
           * ratio + (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z)
           * t2 + (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z)
           * t3);

        return new Vector3(x, y, z);

    }

    //rotation of camera
    public Quaternion Orientation (int seg, float ratio)
    {
        Quaternion q1 = nodes[seg].rotation;
        Quaternion q2 = nodes[seg + 1].rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }


    //Remove for build
    
        // function for drawing line to show rail
    private void OnDrawGizmos()
    {
        for (int i = 0; i < nodes.Length - 1; i++)
        {

            Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3.0f);
        }
    }
     
  



}
