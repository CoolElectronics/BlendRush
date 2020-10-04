using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
public class lightningBolt : MonoBehaviour {
    public Vector3 position;
    private void Start() {
        StartCoroutine(Strike());
    }
    private void Update() {
        
    }
    IEnumerator Strike(){
        yield return new WaitForSeconds(2f);
        transform.position = position;
        transform.rotation = Quaternion.Euler(0,0,Random.Range(0.0f,360.0f));
    }
}