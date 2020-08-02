using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloneTrail : MonoBehaviour
{
    public GameObject clonePlayer;
    int x = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        x++;
        if (x > 1)
        {
            x = 0;
            Destroy(Instantiate(clonePlayer, transform.position, Quaternion.identity), 0.4f);
        }
    }
}
