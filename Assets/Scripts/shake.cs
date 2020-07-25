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
    public Color col1;
    public Color col2;
    void Start()
    {
        e = this;
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Camera>().backgroundColor = Color.Lerp(col1,col2,Mathf.Sin(Time.time));
        if (isShaking)
        {
            shakeOffset = new Vector3(Random.Range(-shakeMag,shakeMag),Random.Range(-shakeMag,shakeMag),0);
            transform.position = offset + shakeOffset;
        }else{
            transform.position = offset;
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
