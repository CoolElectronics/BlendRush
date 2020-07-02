using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class damageSystem : MonoBehaviour
{
    float health = 100;
    public Image image;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = health / 100;
            if (health <= 0){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Laser"){
            health = 0;
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Bullet"){
            Destroy(col.gameObject);
            health -= 5;
        }
    }
}
