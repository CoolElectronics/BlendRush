using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneTrail : MonoBehaviour
{
    public GameObject clonePlayer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(Instantiate(clonePlayer,transform.position,Quaternion.identity),1f);
        
    }
}
