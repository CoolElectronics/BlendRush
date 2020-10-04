using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class overworldTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        Invoke("Action",3f); 
    }
    public void Action(){
        SceneManager.LoadSceneAsync("Boss1");
    }
}