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
    public enum States { bouncing, bounceShoot, exploding};
    [SerializeField]
    States state;
    [SerializeField]
    GameObject lvl;
    public Color telegraphedColor;
    public Color brokenColor;
    SpriteRenderer rendererObj;
    [SerializeField]
    Image image;
    void Start()
    {
        rendererObj = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        switch (state)
        {
            case States.bouncing:
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
                break;
            case States.bounceShoot:
                break;
            default:
                break;
        }
    }
}
