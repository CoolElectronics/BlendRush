using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        Vector2 RandomPos = Vector3.zero;
        if (Random.value >= 0.5f)
        {
            RandomPos = new Vector2(Random.Range(-edgeVector.x, edgeVector.x), -edgeVector.y);
        }
        else
        {
            if (Random.value >= 0.5f)
            {
                RandomPos = new Vector2(edgeVector.x, Random.Range(-edgeVector.y, edgeVector.y / 2));
            }
            else
            {
                RandomPos = new Vector2(-edgeVector.x, Random.Range(-edgeVector.y, edgeVector.y / 2));
            }
        }
        transform.position = (Vector3)RandomPos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
