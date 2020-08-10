using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundTools;
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
    public GameObject beamObject;
    public GameObject gunObject;
    float timeSincePress;
    public float pulseDuration;
    bool laserOn = false;
    public int ammo;
    public int maxAmmo = 5;
    public GameObject ammoCrate;
    GameObject[] ammoPositions;
    [SerializeField]
    AudioClip pickupSound;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m = GetComponent<move>();
        ammo = maxAmmo;
        ammoPositions = GameObject.FindGameObjectsWithTag("ammoCrate");
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
        gunObject.GetComponent<SpriteRenderer>().flipY = angle - 90 < 0;
        if (angle - 90 < 0)
        {
            gunObject.transform.localPosition = new Vector3(0.28f, 2.5f, 0);
        }
        else
        {
            gunObject.transform.localPosition = new Vector3(-0.28f, 2.5f, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0)
            {
                laserOn = true;
                timeSincePress = pulseDuration;
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
                ammo--;
                if (ammo == 1)
                {
                    spawnAmmoCrate();
                }
            }
        }
        beamObject.SetActive(laserOn);
        timeSincePress -= 60f * Time.deltaTime;
        if (timeSincePress <= 0)
        {
            laserOn = false;
        }
    }
    void spawnAmmoCrate()
    {
        Vector3 ammoPos = ammoPositions[Random.Range(0,ammoPositions.Length - 1)].transform.position;
        Instantiate(ammoCrate,ammoPos,Quaternion.identity);
    }
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "ammoCrate"){
            Destroy(col.gameObject);
            soundTools.i.SpawnNewSoundInstance(pickupSound, new SoundSettings());
            ammo = maxAmmo;
        }
    }
}
