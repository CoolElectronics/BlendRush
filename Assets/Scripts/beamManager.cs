using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beamManager : MonoBehaviour
{
    public Transform beamObject;
    public LayerMask lm;
    public float xOffset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.TransformDirection(new Vector3(0,xOffset,0)), transform.TransformDirection(new Vector3(0,1,0)), Mathf.Infinity, lm);
        Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0,xOffset,0)), transform.TransformDirection(new Vector3(0,1,0)) * hit.distance, Color.yellow);
        if (hit.collider != null)
        {
            float dist = hit.distance;
            beamObject.localScale = new Vector3(beamObject.localScale.x,dist / 4,beamObject.localScale.z);
            beamObject.localPosition = new Vector3(0,dist / 8 + xOffset / 4,0);
        }
    }
}
