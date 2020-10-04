using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class meleeBoss : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float speed;
    public float health = 100;
    SpriteRenderer rendererObj;
    [SerializeField]
    Image image;
    Camera cameraMain;
    [SerializeField]
    GameObject bullet;
    void Start()
    {

        cameraMain = Camera.main;
        // rendererObj = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        Vector3 imagepos = transform.position;
        imagepos.z = 10;
        imagepos = cameraMain.WorldToScreenPoint(imagepos);
        imagepos.z = 0;
        image.gameObject.transform.parent.position = imagepos;
        image.fillAmount = health / 100;
    }
    void FixedUpdate()
    {
    }
    void Shoot()
    {
        Invoke("Shoot", 10f);
    }
    GameObject ShootBullet()
    {
        Vector3 mouse_pos = player.transform.position;
        Vector3 object_pos = transform.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        GameObject tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
        return tempBullet;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Beam")
        {
            Damage(2.5f);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            
        }
    }
    void Damage(float amount)
    {
        health -= amount;
        shake.e.Shake(1f, 0.3f);
        // soundTools.i.SpawnNewSoundInstance(hurtSound, new SoundSettings());
    }
}
