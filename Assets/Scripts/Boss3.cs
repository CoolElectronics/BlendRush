using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss3 : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float speed;
    public float health = 100;
    public enum States {Lasers, Follow, Teleport, exploding};
    [SerializeField]
    States state = States.Lasers;
    [SerializeField]
    GameObject lvl;
    public Color telegraphedColor;
    public Color brokenColor;
    SpriteRenderer rendererObj;
    [SerializeField]
    Image image;
    Camera cameraMain;
    [SerializeField]
    GameObject PivotPivot;
    [SerializeField]
    GameObject LaserPivot;
    [SerializeField]
    GameObject TeleportPointCollection;
    [SerializeField]
    float tpCooldownMin;
    [SerializeField]
    float tpCooldownMax;
    [SerializeField]
    float teleportCooldown = 0;
    void Start()
    {

        cameraMain = Camera.main;
        // rendererObj = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        // switch (state)
        // {
        //     case States.bouncing:
        //         break;
        //     case States.bounceShoot:
        //         break;
        // }
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        teleportCooldown -= Time.deltaTime;
        if (teleportCooldown < 0){
            transform.position = TeleportPointCollection.transform.GetChild(Random.Range(0, TeleportPointCollection.transform.childCount)).position;
            teleportCooldown = Random.Range(tpCooldownMin,tpCooldownMax);
        }
        if (health < 0)
        {
            health = 100;
            switch (state)
            {
                case States.Lasers:
                    // Once he dies
                    break;
                case States.Follow:
                    break;
            }
        }
        // Vector3 imagepos = transform.position;
        // imagepos.z = 10;
        // imagepos = cameraMain.WorldToScreenPoint(imagepos);
        // imagepos.z = 0;
        // image.gameObject.transform.parent.position = imagepos;
        image.fillAmount = health / 100;
        switch (state)
        {
                case States.Lasers:
                    PivotPivot.transform.rotation = Quaternion.Euler(0,0,PivotPivot.transform.rotation.eulerAngles.z + speed);
                    LaserPivot.transform.rotation = Quaternion.Euler(0,0,getAngle() - 90);
                    break;
                case States.Follow:
                    break;
        }
    }
    void FixedUpdate()
    {
        // framesRemaining++;
        // if (framesRemaining == framesBetweenBullets)
        // {
        //     framesRemaining = 0;
        //     if (bulletsRemaining > 0)
        //     {
        //         Destroy(ShootBullet(),6f);
        //         bulletsRemaining--;
        //     }
        // }
    }
    // void Shoot()
    // {
    //     if (state == States.bouncing){
    //         bulletsRemaining = numBullets;
    //         Invoke("Shoot", 10f);
    //     }
    // }
    // GameObject ShootBullet()
    // {
    //     Vector3 mouse_pos = player.transform.position;
    //     Vector3 object_pos = transform.position;
    //     mouse_pos.x = mouse_pos.x - object_pos.x;
    //     mouse_pos.y = mouse_pos.y - object_pos.y;
    //     float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
    //     GameObject tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
    //     tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    //     tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
    //     return tempBullet;
    // }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Beam")
        {
            Damage(8f);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            // hurt player
        }
    }
    void Damage(float amount)
    {
        health -= amount;
        shake.e.Shake(1f, 0.3f);
    }
    float getAngle(){
        Vector3 mouse_pos = player.transform.position;
        Vector3 object_pos = transform.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        return angle;
    }
}
