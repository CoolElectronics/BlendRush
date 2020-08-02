using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class damageSystem : MonoBehaviour
{
    public float health = 100;
    public Image image;
    public GameObject hitParticles;
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
        if (col.gameObject.tag == "Explosion"){
            Destroy(col.gameObject);
            health -= 10;
            shake.e.Shake(0.1f,0.1f);
            Instantiate(hitParticles,transform.position,Quaternion.identity);
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Laser"){
            health = 0;
        }
        if (col.gameObject.tag == "Bullet"){
            Destroy(col.gameObject);
            health -= 10;
            shake.e.Shake(0.1f,0.1f);
            Instantiate(hitParticles,transform.position,Quaternion.identity);
        }
    }
}
