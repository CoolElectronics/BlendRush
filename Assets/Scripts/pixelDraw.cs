using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixelDraw : MonoBehaviour
{
    public float cubesToRow;
    public float spacing;
    public float numCubes;
    public GameObject cube;
    public Vector3 offset;
    void Start()
    {
        float indey = 0;
        float index = 0;
        for (int i = 0; i < numCubes; i++)
        {
            if (index > cubesToRow){
                indey ++;
                index = 0;
            }
            Instantiate(cube, new Vector3(index * spacing,indey * spacing,0) + offset,Quaternion.identity);
            index ++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
