using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bouncyBall : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    float speed;
    public float health = 100;
    Vector3 direction;
    [SerializeField]
    Transform hpPrefab;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    float bulletSpeed;
    [SerializeField]
    float turnSpeed;
    Camera cameraMain;
    float radius = 0;
    float phi = 0;
    int bulletGap = 0;
    bool shooting = false;
    [SerializeField]
    float bulletLongevity = 0;
    [SerializeField]
    float bulletUpgradeTime;

    void Start()
    {
        phi = (1 + Mathf.Sqrt(5)) / 2;
        cameraMain = Camera.main;
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList.Length > 0)
        {
            player = playerList[0];
        }
        else
        {
            throw new System.Exception("Umm, so this is a game, which wouldn't be much fun without a player...");
        }
        hpPrefab = Instantiate(hpPrefab);
        hpPrefab.parent = GameObject.Find("Canvas").transform;
        direction = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        PlayerLock();
    }
    private void FixedUpdate() {
        Shoot();
    }
    void PlayerLock()
    {
        direction = (player.transform.position - transform.position).normalized;
        Invoke("PlayerLock", 2f);
    }
    void Shoot()
    {
        bulletLongevity += bulletUpgradeTime;
         if (bulletGap > 20){
                shooting = !shooting;
                bulletGap = 0;
            }
            bulletGap ++;
        if (shooting)
        {
            radius += phi * turnSpeed;
            float angle = radius;
            GameObject tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * bulletSpeed;
            Destroy(tempBullet, Random.Range(0.0f, 1.0f + bulletLongevity));
        }
    }
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        //transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(direction.x,-direction.y) * Mathf.Rad2Deg);
        Vector3 imagepos = transform.position;
        imagepos.z = 10;
        imagepos = cameraMain.WorldToScreenPoint(imagepos);
        imagepos.z = 0;
        hpPrefab.position = imagepos;
        hpPrefab.GetChild(1).GetComponent<Image>().fillAmount = health / 20;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            direction = (transform.position - player.transform.position).normalized;
            player.GetComponent<damageSystem>().Damage(1);
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
                    Destroy(col.gameObject);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Beam")
        {
            Damage(5f);
        }
    }
    public void Damage(float amount)
    {
        health -= amount;
        shake.e.Shake(1f, 0.3f);
        if (health <= 0)
        {
            Kill();
        }
        // soundTools.i.SpawnNewSoundInstance(hurtSound, new SoundSettings());
    }
    public void Kill()
    {
        Destroy(hpPrefab.gameObject);
        Destroy(gameObject, 1f);
    }
}
