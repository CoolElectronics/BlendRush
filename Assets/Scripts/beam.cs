using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beam : MonoBehaviour
{
    [SerializeField]
    Transform beamPivot;
    [SerializeField]
    float knockback;
    Rigidbody2D rb;
    bool canKnock = true;
    move m;
    [SerializeField]
    float screenShake;
    [SerializeField]
    float duration;
    [SerializeField]
    float returnSpeed;
    [SerializeField]
    float reducedTime;
    public float time;
    public GameObject particles;
    public bool isTimeShifting = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m = GetComponent<move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeShifting)
        {
            if (time >= 0.9)
            {
                isTimeShifting = false;
                Time.timeScale = 1;
            }
        }
        if (time < 1)
        {
            time += returnSpeed;
        }
        if (m.timeSinceGrounded > 0)
        {
            canKnock = true;
        }
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 20;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        beamPivot.rotation = Quaternion.Euler(0, 0, angle - 90);
        beamPivot.gameObject.SetActive(Input.GetMouseButton(0));
        if (Input.GetMouseButtonDown(0))
        {
            if (canKnock)
            {
                canKnock = false;
                shake.e.Shake(screenShake, duration);
                time = reducedTime;
                Time.timeScale = 0;
                isTimeShifting = true;
                Instantiate(particles, transform.position, Quaternion.identity);
                rb.velocity = (Vector2)mouse_pos.normalized * -knockback;
            }

        }
    }
}
