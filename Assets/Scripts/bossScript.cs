﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SoundTools;
public class bossScript : MonoBehaviour
{
    [Header("BossSettings")]
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject player;
    [SerializeField]

    float speed;
    [SerializeField]
    GameObject missileObject;
    [SerializeField]
    Image image;
    [SerializeField]
    States state;
    [SerializeField]
    GameObject laser;
    [SerializeField]
    float timeUntilBlast;
    [SerializeField]
    Transform beamPivot;
    [SerializeField]
    Transform lightPivot;
    Color defaultC;
    [SerializeField]
    Color telegraphedColor;
    [SerializeField]
    Color brokenColor;

    SpriteRenderer rendererObj;
    [SerializeField]
    LayerMask lm;
    [SerializeField]
    LayerMask ExcludePlayerlm;
    Vector2 targetPosition;
    [SerializeField]
    float PathSpeed;
    [SerializeField]
    GameObject emitterObj;
    [SerializeField]
    float amplitude;
    [SerializeField]
    LayerMask PathMask;
    [SerializeField]
    float sight;
    [SerializeField]
    AudioClip hurtSound;
    [SerializeField]
    GameObject lightningPrefab;
    [SerializeField]
    List<float> phaseThresholds;
    [SerializeField]
    int emitterSpawnFrequency = 4;
    [Header("Check to see if target is not close to walls")]
    [SerializeField]
    float avoidRadius;
    [SerializeField]
    LayerMask avoidLm;
    [SerializeField]
    // 
    // 
    // 
    // 
    // 
    // Hidden
    // 
    // 
    // 
    // 
    // 

    [Header("HiddenVars")]
    [HideInInspector]
    // Health
    public float health = 100;
    bool rayset = false;
    float angleToTarget = 0;
    bool canStillCheckPlayerVis = true;
    int emitters = 0;
    bool missilesActive = false;
    int emitterHits = 0;
    Vector3 lockedPlayerPos;
    public enum States { shooting, laser, blast, broken };
    void Start()
    {
        rendererObj = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        defaultC = rendererObj.color;
        switch (state)
        {
            case States.shooting:
                LockLightPos();
                Invoke("Shoot", 5f);
                Invoke("TeleShoot", 4.6f);
                //Invoke("BreakDown", Random.Range(15.0f, 60.0f));
                break;
            case States.laser:
                break;
        }
    }
    void Update()
    {
        if (health < 0)
        {
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            bossManager.i.DestroyBoss(0);
            scene.GetRootGameObjects(rootObjects);
            foreach (GameObject d in rootObjects){
                if (d.layer == 5)
                    Destroy(d);
            }
            
        }
        image.fillAmount = health / 100;
        switch (state)
        {
            case States.shooting:
                Vector3 mouse_pos = player.transform.position;
                Vector3 object_pos = transform.position;
                mouse_pos.x = mouse_pos.x - object_pos.x;
                mouse_pos.y = mouse_pos.y - object_pos.y;
                float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
                lightPivot.rotation = Quaternion.Euler(0, 0, angle + 90);
                if (health < phaseThresholds[1] && !missilesActive)
                {
                    missilesActive = true;
                    ShootMissile();
                }
                if (health < phaseThresholds[2] && emitters < 1)
                {
                    MakeNewEmitter();

                    MakeNewEmitter();
                    emitterHits = 0;
                }
                if (targetPosition != Vector2.zero)
                {
                    Pathfind(targetPosition);
                }
                else
                {
                    transform.position = Hover(transform.position, amplitude);
                }
                Debug.DrawRay(targetPosition, new Vector3(0, 1, 0), Color.red, 0.1f);
                Vector2 topRightCorner = new Vector2(1, 1);
                Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
                if ((player.transform.position - transform.position).magnitude < avoidRadius && targetPosition == Vector2.zero)
                {
                    MoveToNewPosition();
                }
                if (!CanSeePlayer())
                {
                    if (canStillCheckPlayerVis)
                    {
                        canStillCheckPlayerVis = false;
                        Invoke("CheckVisibilityAfter", 1.5f);
                    }
                }
                else
                {
                    // targetPosition = Vector2.zero;
                }
                //code for making the object take the inverse position of the player 
                //transform.position = new Vector3(Mathf.Abs((player.transform.position.x + edgeVector.x) - edgeVector.x * 2) - edgeVector.x,Mathf.Abs((player.transform.position.y + edgeVector.y) - edgeVector.y * 2) - edgeVector.y,0);
                break;
            case States.laser:
                health -= timeUntilBlast;
                break;
            case States.blast:

                break;
            default:
                break;
        }
    }
    void LockLightPos()
    {
        lockedPlayerPos = player.transform.position;
        GameObject bolt = Instantiate(lightningPrefab, Vector3.zero, Quaternion.identity);
        bolt.GetComponent<lightningBolt>().position = lockedPlayerPos;
        Invoke("LockLightPos", Random.Range(3.0f, 7.0f));
    }
    void ShootMissile()
    {
        if ((state == States.shooting || state == States.broken) && health > phaseThresholds[2])
        {
            GameObject tempMissile = Instantiate(missileObject, transform.position, Quaternion.identity);
            tempMissile.GetComponent<Missile>().target = player.transform;
            Invoke("ShootMissile", 12f);
        }
    }
    void Shoot()
    {
        if (state == States.shooting)
        {
            rendererObj.color = defaultC;
            Vector3 mouse_pos = player.transform.position;
            Vector3 object_pos = transform.position;
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            GameObject tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;
            tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 80);
            tempBullet.GetComponent<Rigidbody2D>().velocity = -new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + 10), Mathf.Sin(angle * Mathf.Deg2Rad + 10)) * speed;
            tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 100);
            tempBullet.GetComponent<Rigidbody2D>().velocity = -new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad - 10), Mathf.Sin(angle * Mathf.Deg2Rad - 10)) * speed;
            if (health > phaseThresholds[1])
            {
                tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 110);
                tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad - 20), Mathf.Sin(angle * Mathf.Deg2Rad - 20)) * speed;
                tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 70);
                tempBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad + 20), Mathf.Sin(angle * Mathf.Deg2Rad + 20)) * speed;
            }
            Invoke("Shoot", 1.1f);
            Invoke("TeleShoot", 0.7f);
        }
    }
    void TeleShoot()
    {
        rendererObj.color = telegraphedColor;
    }
    void BreakDown()
    {
        if (state == States.shooting)
        {
            state = States.broken;
            rendererObj.color = brokenColor;
            Invoke("FixUp", 8f);
            Invoke("BreakDown", Random.Range(15.0f, 60.0f));
        }
    }
    void FixUp()
    {
        state = States.shooting;
        rendererObj.color = defaultC;
        Invoke("Shoot", 1.1f);
        Invoke("TeleShoot", 0.7f);
    }
    void TargetLock()
    {
        Vector3 mouse_pos = player.transform.position;
        Vector3 object_pos = transform.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        beamPivot.rotation = Quaternion.Euler(0, 0, angle - 90);
        Invoke("TargetLock", 2f);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Beam")
        {
            Damage(2.5f);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // if (col.gameObject.tag == "ParriedBullet")
        // {
        //     Damage(14);
        //     Destroy(col.gameObject);
        // }
    }
    void MakeNewEmitter()
    {
        emitters++;
        Instantiate(emitterObj, transform.position, Quaternion.identity);
    }
    bool CanSeePlayer()
    {
        Vector3 mouse_pos = player.transform.position;
        Vector3 object_pos = transform.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, reverseAtan(angle), Mathf.Infinity, lm);
        Debug.DrawRay(transform.position, reverseAtan(angle) * hit.distance, Color.yellow);
        if (hit.collider != null)
        {
            GameObject go = hit.collider.gameObject;
            if (go.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    List<Vector2> CalculateLOSPosition()
    {
        Vector3 mouse_pos = transform.position;
        Vector3 object_pos = player.transform.position;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        RaycastHit2D hit;
        List<Vector2> possibleLocations = new List<Vector2>(0);
        for (int a = 0; a < 360; a += 2)
        {
            hit = Physics2D.Raycast(player.transform.position, reverseAtan(angle + a), mouse_pos.magnitude, ExcludePlayerlm);
            if (hit.collider == null)
            {
                Debug.DrawRay(player.transform.position, reverseAtan(angle + a) * mouse_pos.magnitude, Color.yellow);
                possibleLocations.Add((Vector2)player.transform.position + reverseAtan(angle + a) * Mathf.Clamp(mouse_pos.magnitude + Random.Range(-20, 20), 70, 140));
            }
        }
        return possibleLocations;
    }
    Vector2 reverseAtan(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
    void Pathfind(Vector2 target)
    {
        if ((target - (Vector2)transform.position).magnitude > 3)
        {
            if (RaycastInDirection(angleToTarget, sight).hit.collider == null)
            {
                rayset = false;
            }
            Vector2 normal = (target - (Vector2)transform.position).normalized;
            if (!rayset)
            {
                angleToTarget = Mathf.Atan2(normal.x, -normal.y) * Mathf.Rad2Deg;
            }
            DirectionalRaycastResult r = RaycastInDirection(angleToTarget - 90, sight);
            List<DirectionalRaycastResult> rays = new List<DirectionalRaycastResult>(0);
            if (r.hit.collider != null)
            {
                for (int a = -180; a < 180; a += 8)
                {
                    DirectionalRaycastResult r2d = RaycastInDirection(angleToTarget - 90 + a, 1000);
                    if (r2d.hit.collider == null)
                    {
                        rays.Add(r2d);
                    }
                }
                if (rays.Count > 0)
                {
                    rayset = true;
                    Invoke("ClearRays", 2f);
                    float greatestDist = 19999;
                    float greatestAngle = 0;
                    for (int i = 0; i < rays.Count; i++)
                    {
                        if ((((Vector2)transform.position + reverseAtan(rays[i].angle) * 20) - target).magnitude < greatestDist)
                        {
                            greatestAngle = rays[i].angle + 90;
                            greatestDist = (((Vector2)transform.position + reverseAtan(rays[i].angle) * 20) - target).magnitude;
                        }
                    }
                    angleToTarget = greatestAngle;
                }
            }
            transform.position += (Vector3)reverseAtan(angleToTarget - 90) * PathSpeed;// * (target - (Vector2)transform.position).magnitude;
        }
        else
        {
            targetPosition = Vector2.zero;
        }
    }
    DirectionalRaycastResult RaycastInDirection(float angle, float dist)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, reverseAtan(angle), dist, PathMask);
        if (hit.collider == null)
        {
            Debug.DrawRay(transform.position, reverseAtan(angle) * dist, Color.yellow);
        }
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, reverseAtan(angle) * hit.distance, Color.yellow);
        }
        return new DirectionalRaycastResult(hit, angle);
    }
    void ClearRays()
    {
        rayset = false;
    }
    void CheckVisibilityAfter()
    {
        canStillCheckPlayerVis = true;
        if (!CanSeePlayer())
        {
            MoveToNewPosition();
        }
    }
    void MoveToNewPosition()
    {
        List<Vector2> possibleLocations = CalculateLOSPosition();
        if (possibleLocations.Count > 0)
        {
            int safety = 0;
            while (true)
            {
                safety++;
                if (safety > 200)
                    break;
                targetPosition = possibleLocations[(int)Mathf.Round(Random.Range(0, possibleLocations.Count - 1))];
                if (!Physics2D.OverlapCircle(targetPosition, avoidRadius, avoidLm))
                {
                    break;
                }
            }
        }
    }
    Vector3 Hover(Vector3 current, float amplitude)
    {
        Vector3 newPos = current;
        newPos += new Vector3(Mathf.Sin(Time.time), Mathf.Cos(Time.time)) * amplitude;
        return newPos;
    }
    void Damage(float amount)
    {
        health -= amount;
        shake.e.Shake(1f, 0.3f);
        soundTools.i.SpawnNewSoundInstance(hurtSound, new SoundSettings());
        if (health < phaseThresholds[2] && emitters > 0 && emitters < 4)
        {
            emitterHits++;
            if (emitterHits >= emitterSpawnFrequency)
            {
                MakeNewEmitter();
                emitterHits = 0;
            }
        }
    }
}