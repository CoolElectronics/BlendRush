using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SoundTools;
public class Missile : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    float force;
    [SerializeField]
    float rspeed;
    [SerializeField]
    bool isVelocity;
    [SerializeField]
    bool rotate;
    Rigidbody2D rb;
    [SerializeField]
    SpriteRenderer min;
    [SerializeField]
    SpriteRenderer second;
    [SerializeField]
    SpriteRenderer second2;
    public Sprite[] numbers;
    public float timer = 120;
    public GameObject explosion;
    public float boomspeed;
    public int explosiongap;
    [SerializeField]
    AudioClip tick;
    [SerializeField]
    AudioClip explode;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        timer -= Time.deltaTime * 30;
        if (Mathf.Round(timer) % 20 == 4){
            soundTools.i.SpawnNewSoundInstance(tick, new SoundSettings(0.3f));
        }
        if (timer <= 0)
        {
            boom();
            return;
        }
        string time = toTime((int)Mathf.Round(timer));
        if (time.Length >= 8)
        {
            string num1 = time[4].ToString();
            string num2 = time[6].ToString();
            string num3 = time[7].ToString();
            min.sprite = numbers[Convert.ToInt32(num1)];
            second.sprite = numbers[Convert.ToInt32(num2)];
            second2.sprite = numbers[Convert.ToInt32(num3)];
        }
        Vector3 dir = (target.position - transform.position).normalized;
        if (isVelocity)
        {
            rb.velocity = dir * force;
        }
        else
        {
            rb.AddForce(dir * force);
        }
        if (rotate)
        {
            Vector2 v = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(v.x, v.y) * -Mathf.Rad2Deg);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Boundary") || col.gameObject.CompareTag("Player"))
        {
            boom();
        }
    }
    string toTime(int time)
    {
        return TimeSpan.FromSeconds(time).ToString();
    }
    void boom()
    {
        shake.e.Shake(0.3f, 1.3f);
        soundTools.i.SpawnNewSoundInstance(explode, new SoundSettings(0.8f));
        Destroy(gameObject);
        Vector2 normalVec = (transform.position - target.position).normalized;
        float angleBetweenPlayer = -Mathf.Atan2(normalVec.x, normalVec.y) * Mathf.Rad2Deg - 90 + explosiongap / 2;
        int offset = (int)Mathf.Round(angleBetweenPlayer) + UnityEngine.Random.Range(-45, 45);
        for (int r = offset; r < 360 + offset - explosiongap; r += 5)
        {
            GameObject tempBullet = Instantiate(explosion, transform.position, Quaternion.identity);
            tempBullet.transform.rotation = Quaternion.Euler(0, 0, r - 90);
            Destroy(tempBullet, 20);
            tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(r * Mathf.Deg2Rad), Mathf.Sin(r * Mathf.Deg2Rad)) * boomspeed;
        }
    }
}
