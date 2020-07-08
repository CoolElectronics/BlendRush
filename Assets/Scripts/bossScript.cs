﻿using System.Collections;
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
    public bool isBeingHyperHit = false;
    public float health = 100;
    [SerializeField]
    Image image;
    public enum States {shooting, laser, blast, broken};
    [SerializeField]
    States state;
    [SerializeField]
    GameObject laser;
    [SerializeField]
    GameObject lvl;
    [SerializeField]
    float timeUntilBlast;
    [SerializeField]
    Transform beamPivot;
    Color defaultC;
    public Color telegraphedColor;
    public Color brokenColor;

    Renderer rendererObj;
    void Start()
    {
        rendererObj = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        defaultC = rendererObj.material.color;
        switch (state){
            case States.shooting:
                Invoke("Shoot",5f);
                Invoke("TeleShoot",4.6f);
                Invoke("BreakDown",Random.Range(15.0f,60.0f));
                break;
            case States.laser:
                break;
        }
    }

    void Update()
    {
        if (isBeingHit){
            health -= 0.2f;
        }
        if (isBeingHyperHit){
            health -= 2f;
        }
        if (health < 0){
            health = 100;
            switch (state)
            {
                case States.shooting:
                    state = States.laser;
                    GetComponent<BoxCollider2D>().enabled = false;
                    transform.GetChild(0).GetComponent<Renderer>().enabled = false;
                    laser = Instantiate(laser,Vector3.zero,Quaternion.identity);
                    GetComponent<Animator>().enabled = false;
                    lvl.GetComponent<Animator>().SetBool("StartLasers",true);
                    break;
                case States.laser:
                    GetComponent<BoxCollider2D>().enabled = true;
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    GetComponent<Animator>().enabled = false;
                    transform.position = new Vector3(0,20,0);
                    Destroy(laser);
                    state = States.blast;
                    beamPivot.gameObject.SetActive(true);
                    break;
            }
        }
        image.fillAmount = health / 100;
        switch (state)
        {
            case States.laser:
                health -= timeUntilBlast;
                break;
            case States.blast:
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
     rendererObj.material.color = defaultC;
     Vector3 mouse_pos = player.transform.position;
     Vector3 object_pos = transform.position;
     mouse_pos.x = mouse_pos.x - object_pos.x;
     mouse_pos.y = mouse_pos.y - object_pos.y;
     float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
     GameObject tempBullet = Instantiate(bullet,transform.position,Quaternion.identity);
     tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
     tempBullet.GetComponent<Rigidbody2D>().velocity = (Vector2)mouse_pos.normalized * speed;
     if (state == States.shooting){
        Invoke("Shoot",1.1f);
        Invoke("TeleShoot",0.7f);
     }
    }
    void TeleShoot(){
        rendererObj.material.color = telegraphedColor;
    }
    void BreakDown(){
        if (state == States.shooting){
        state = States.broken;
        rendererObj.material.color = brokenColor;
        GetComponent<Animator>().enabled = false;
        Invoke("FixUp",8f);
        Invoke("BreakDown",Random.Range(15.0f,60.0f));
        }
    }
    void FixUp(){
        state = States.shooting;
        rendererObj.material.color = defaultC;
        GetComponent<Animator>().enabled = true;
        Invoke("Shoot",1.1f);
        Invoke("TeleShoot",0.7f);
    }
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Beam"){
            isBeingHit = true;
        }
        if (col.gameObject.tag == "HyperBeam"){
            isBeingHyperHit = true;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if (col.gameObject.tag == "Beam"){
            isBeingHit = false;
        }
        if (col.gameObject.tag == "HyperBeam"){
            isBeingHyperHit = false;
        }
    }

     void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "ParriedBullet"){
            health -= 14;
            Destroy(col.gameObject);
        }
    }
}
