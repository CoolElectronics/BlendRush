using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bounceBoss : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float speed;
    public float health = 100;
    public enum States { bouncing, bounceShoot, exploding };
    [SerializeField]
    States state;
    [SerializeField]
    GameObject lvl;
    public Color telegraphedColor;
    public Color brokenColor;
    SpriteRenderer rendererObj;
    [SerializeField]
    Image image;
    Vector3 direction;
    Camera cameraMain;
    [SerializeField]
    GameObject bullet;
    void Start()
    {
        
        cameraMain = Camera.main;
        // rendererObj = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        switch (state)
        {
            case States.bouncing:
                direction = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
                GetComponent<Rigidbody2D>().velocity = direction * speed;
                break;
            case States.bounceShoot:
                break;
        }
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Shoot();
    }

    void Update()
    {
        if (health < 0)
        {
            health = 100;
            switch (state)
            {
                case States.bouncing:
                    state = States.bounceShoot;
                    break;
                case States.bounceShoot:
                    state = States.exploding;
                    break;
            }
        }
        Vector3 imagepos = transform.position;
        imagepos.z = 10;
        imagepos = cameraMain.WorldToScreenPoint(imagepos);
        imagepos.z = 0;
        image.gameObject.transform.parent.position = imagepos;
        image.fillAmount = health / 100;
        switch (state)
        {
            case States.bouncing:
                GetComponent<Rigidbody2D>().velocity = direction * speed;
                break;
            case States.bounceShoot:
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
        GameObject tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
        Invoke("Shoot",5f);
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
            direction = (transform.position - player.transform.position).normalized;
        }
        else
        {
            ContactPoint2D[] points = new ContactPoint2D[10];
            GetComponent<CircleCollider2D>().GetContacts(points);
            if (points.Length > 0)
            {
                direction = Vector3.Reflect(direction, points[0].normal);
                if (col.gameObject.tag == "breakable")
                {
                    //Destroy(col.gameObject);
                }
            }
        }
    }
    void Damage(float amount)
    {
        health -= amount;
        shake.e.Shake(1f, 0.3f);
        // soundTools.i.SpawnNewSoundInstance(hurtSound, new SoundSettings());

    }
}
