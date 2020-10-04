using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    Transform boss;
    [SerializeField]
    float magnitude;
    [SerializeField]
    float speed;
    [SerializeField]
    float magnx;
    [SerializeField]
    float speedx;
    GameObject target;
    [SerializeField]
    Transform beamPivot;
    [SerializeField]
    GameObject targetPrefab;
    float onOffTimer = 0;
    [SerializeField]
    Sprite normal;
    [SerializeField]
    Sprite disabled;
    [SerializeField]
    float timerMax;
    bool onoff = false;
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").transform;
        target = Instantiate(targetPrefab);
        onOffTimer = timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("LaserEmitter");
        float index = GetIndexInList(list);
        onOffTimer--;
        if (onOffTimer < 0)
        {
            onOffTimer = timerMax + index * 60;
            GameObject beam = beamPivot.GetChild(0).gameObject;
            if (onoff)
            {
                beam.GetComponent<SpriteRenderer>().sprite = normal;
                beam.GetComponent<BoxCollider2D>().enabled = true;
                onoff = false;
            }
            else
            {
                beam.GetComponent<SpriteRenderer>().sprite = disabled;
                beam.GetComponent<BoxCollider2D>().enabled = false;
                onoff = true;
            }
        }
        float offsetDeg = (360 / list.Length) * index;
        transform.position = boss.position + new Vector3(Mathf.Sin(Time.time * speed + offsetDeg * Mathf.Deg2Rad), Mathf.Cos(Time.time * speed + offsetDeg * Mathf.Deg2Rad)) * magnitude;
        Vector3 mouse_pos = target.transform.position;
        Vector3 object_pos = transform.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        beamPivot.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    int GetIndexInList(GameObject[] list)
    {
        for (int g = 0; g < list.Length; g++)
        {
            if (list[g] == gameObject)
            {
                return g;
            }
        }
        throw new System.Exception("You did something wrong");
    }
}
