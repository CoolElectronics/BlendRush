using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class laserManager : MonoBehaviour
{
    public float dist;
    public Transform emitter1;
    public Transform emitter2;
    public Transform beam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        beam.localScale = new Vector3(0.8f,dist,1);
        emitter1.localPosition = new Vector3(dist,0,0);
        emitter2.localPosition = new Vector3(-dist,0,0);
    }
}
