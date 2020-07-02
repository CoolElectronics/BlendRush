﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamManager : MonoBehaviour
{
    public Transform beamObject;
    public LayerMask lm;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector3(0,1,0)), Mathf.Infinity, lm);
        Debug.DrawRay(transform.position, transform.TransformDirection(new Vector3(0,1,0)) * hit.distance, Color.yellow);
        if (hit.collider != null)
        {
            float dist = hit.distance;
            beamObject.localScale = new Vector3(1,dist / 2,1);
            beamObject.localPosition = new Vector3(0,dist / 2,0);
            Debug.Log("Did Hit");
        }
    }
}