using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyDestructables : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "breakable" && InputManager.e.mTimer < 0.1){
            Destroy(col.gameObject);
        }
    }
}
