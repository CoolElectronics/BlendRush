using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class futurePlotter
{
    void Update(){
        Physics.autoSimulation = false;
    }
    Vector3 predictRigidBodyPosInTime(Rigidbody sourceRigidbody, float timeInSec)
    {
        //Get current Position
        Vector3 defaultPos = sourceRigidbody.position;

        Debug.Log("Predicting Future Pos from::: x " + defaultPos.x + " y:"
            + defaultPos.y + " z:" + defaultPos.z);

        //Simulate where it will be in x seconds
        while (timeInSec >= Time.fixedDeltaTime)
        {
            timeInSec -= Time.fixedDeltaTime;
            Physics.Simulate(Time.fixedDeltaTime);
        }

        //Get future position
        Vector3 futurePos = sourceRigidbody.position;

        Debug.Log("DONE Predicting Future Pos::: x " + futurePos.x + " y:"
            + futurePos.y + " z:" + futurePos.z);

        //Re-enable Physics AutoSimulation and Reset position
        Physics.autoSimulation = true;
        sourceRigidbody.velocity = Vector3.zero;
        sourceRigidbody.useGravity = false;
        sourceRigidbody.position = defaultPos;

        return futurePos;
    }
}