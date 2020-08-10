using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SoundTools;
public class damageSystem : MonoBehaviour
{
    public float health = 100;
    public Image image;
    public GameObject hitParticles;
    public float maxInvulnerableTime;
    float invulnerableTime;
    public GameObject shield;

    [SerializeField]
    public AudioClip hurtSound;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shield.SetActive(invulnerableTime > 0);
        invulnerableTime -= Time.deltaTime;
        image.fillAmount = health / 10;
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Explosion")
        {
            Destroy(col.gameObject);
            Damage(1);
        }
        if (col.gameObject.tag == "Laser")
        {
            Damage(1);
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bullet")
        {
            Destroy(col.gameObject);
            Damage(1);
        }
    }
    void Damage(int amount)
    {
        if (invulnerableTime < 0)
        {
            health -= amount;
            invulnerableTime = maxInvulnerableTime;
            shake.e.Shake(1f, 0.3f);
            soundTools.i.SpawnNewSoundInstance(hurtSound, new SoundSettings());
        }
    }
}
