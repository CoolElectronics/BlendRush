using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shake : MonoBehaviour
{
    Vector3 shakeOffset;
    Vector3 offset;
    float shakeMag;
    bool isShaking = false;
    public static shake e;
    void Start()
    {
        e = this;
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            shakeOffset = new Vector3(Random.Range(-shakeMag,shakeMag),Random.Range(-shakeMag,shakeMag),0);
            transform.position = offset + shakeOffset;
        }
    }
    public void Shake(float magnitude, float duration){
        isShaking = true;
        shakeMag = magnitude;
        StartCoroutine(StopShake(duration));
    }
    IEnumerator StopShake(float time){
        yield return new WaitForSecondsRealtime(time);
        shakeOffset = Vector3.zero;
        isShaking = false;
        shakeMag = 0;
    }
}
