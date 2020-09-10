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
    Camera cameraMain;
    void Start()
    {
        cameraMain = Camera.main;
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        if (playerList.Length > 0){
            player = playerList[0];
        }else{
            throw new System.Exception("Umm, so this is a game, which wouldn't be much fun without a player...");
        }
        hpPrefab = Instantiate(hpPrefab);
        hpPrefab.parent = GameObject.Find("Canvas").transform;
        direction = new Vector3(Random.Range(-1f, 1f), 1f, 0f).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        PlayerLock();
    }
    void PlayerLock(){
        direction = (player.transform.position - transform.position).normalized;
        Invoke("PlayerLock",2f);
    }
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
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
        if (health <= 0){
            Kill();
        }
        // soundTools.i.SpawnNewSoundInstance(hurtSound, new SoundSettings());
    }
    public void Kill(){
        Destroy(hpPrefab.gameObject);
        Destroy(gameObject,1f);
    }
}
