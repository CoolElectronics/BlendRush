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
    public enum State {shooting, laser, blast};
    [SerializeField]
    State state;
    [SerializeField]
    GameObject laser;
    [SerializeField]
    GameObject lvl;
    [SerializeField]
    float timeUntilBlast;
    [SerializeField]
    Transform beamPivot;

    void Start()
    {
        switch (state){
            case State.shooting:
                Invoke("Shoot",5f);
                break;
            case State.laser:
                break;
        }
    }

    void Update()
    {
        if (isBeingHit){
            health -= 0.2f;
        }
        if (health < 0){
            health = 100;
            switch (state)
            {
                case State.shooting:
                    state = State.laser;
                    GetComponent<BoxCollider2D>().enabled = false;
                    transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                    laser = Instantiate(laser,Vector3.zero,Quaternion.identity);
                    GetComponent<Animator>().enabled = false;
                    lvl.GetComponent<Animator>().SetBool("StartLasers",true);
                    break;
                case State.laser:
                    GetComponent<BoxCollider2D>().enabled = true;
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    GetComponent<Animator>().enabled = false;
                    transform.position = new Vector3(0,20,0);
                    Destroy(laser);
                    state = State.blast;
                    beamPivot.gameObject.SetActive(true);
                    break;
            }
        }
        image.fillAmount = health / 100;
        switch (state)
        {
            case State.laser:
                health -= timeUntilBlast;
                break;
            case State.blast:
                Vector3 mouse_pos = player.transform.position;
                Vector3 object_pos = transform.position;
                mouse_pos.x = mouse_pos.x - object_pos.x;
                mouse_pos.y = mouse_pos.y - object_pos.y;
                float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                beamPivot.rotation = Quaternion.Euler(0, 0, angle - 90);
                break;
            default:
                break;
        }
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
     if (state == State.shooting){
        Invoke("Shoot",1.1f);
     }
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
     void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "ParriedBullet"){
            health -= 20;
            Destroy(col.gameObject);
        }
    }
}
