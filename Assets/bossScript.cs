using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bossScript : MonoBehaviour
{   
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject player;
    [SerializeField]
    float speed;
    public bool isBeingHit = false;
    public float health = 100;
    [SerializeField]
    Image image;
    void Start()
    {
        Invoke("Shoot",5f);
    }

    void Update()
    {
        if (isBeingHit){
            health -= 0.2f;
        }
        image.fillAmount = health / 100;
    }
    void Shoot()
    {
    Vector3 mouse_pos = player.transform.position;
     Vector3 object_pos = transform.position;
     mouse_pos.x = mouse_pos.x - object_pos.x;
     mouse_pos.y = mouse_pos.y - object_pos.y;
     float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
     GameObject tempBullet = Instantiate(bullet,transform.position,Quaternion.identity);
     tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
     tempBullet.GetComponent<Rigidbody2D>().velocity = (Vector2)mouse_pos.normalized * speed;
     Invoke("Shoot",0.5f);
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Beam"){
            isBeingHit = true;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if (col.gameObject.tag == "Beam"){
            isBeingHit = false;
        }
    }
}
