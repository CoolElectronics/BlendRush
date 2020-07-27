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
    void Start()
    {
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
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            direction = (transform.position - player.transform.position).normalized;
        }
        else
        {
            ContactPoint2D[] points = new ContactPoint2D[10];
            GetComponent<BoxCollider2D>().GetContacts(points);
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
}
