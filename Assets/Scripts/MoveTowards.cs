using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MoveTowards : MonoBehaviour
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
    int timer = 120;
    public GameObject explosion;
    public float boomspeed;
    public int explosiongap;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        timer--;
        if (timer == 0){
            boom();
            return;
        }
        string time = toTime(timer);
        if (time.Length >= 8){
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
        boom();
    }
    string toTime(int time)
    {
        return TimeSpan.FromSeconds(time).ToString();
    }
    void boom(){
        Destroy(gameObject);
        int offset = (int)Mathf.Round(UnityEngine.Random.Range(0,360));
        for (int r = offset; r < 360 + offset - explosiongap; r+= 3)
        {
            GameObject tempBullet = Instantiate(explosion, transform.position, Quaternion.identity);
            tempBullet.transform.rotation = Quaternion.Euler(0, 0, r - 90);
            tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(r * Mathf.Deg2Rad), Mathf.Sin(r * Mathf.Deg2Rad)) * boomspeed;
        }
    }
}
