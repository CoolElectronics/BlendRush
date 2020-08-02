using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Combat : MonoBehaviour
{
    public float parrySpeed;
    public float parryThreshold;
    public float meleeRange;
    public Transform boss;
    public Animator swordAnim;
    public float mana;
    public Image manaCounter;
    public GameObject trailPrefab;
    public Transform beamPivot;
    public bool isSuperAttacking = false;
    public GameObject struckParticles;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject b in bullets){
            if ((transform.position - b.transform.position).sqrMagnitude < parryThreshold * parryThreshold && InputManager.e.mouseClicked && !Input.GetMouseButton(0) && (transform.position - boss.position).sqrMagnitude > (parryThreshold * parryThreshold)){
                if (mana > 5){
                    Vector3 mouse_pos = Input.mousePosition;
                    mouse_pos.z = 20;
                    Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
                    mouse_pos.x = mouse_pos.x - object_pos.x;
                    mouse_pos.y = mouse_pos.y - object_pos.y;
                    b.transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(mouse_pos.x,mouse_pos.y) + 90);
                    b.GetComponent<Rigidbody2D>().velocity = (Vector2)mouse_pos.normalized * parrySpeed;
                    b.layer = 0;
                    b.tag = "ParriedBullet";
                    GameObject trail = Instantiate(trailPrefab,new Vector3(100,100,-200),Quaternion.identity);
                    trail.transform.SetParent(b.transform);
                    trail.transform.localPosition = Vector3.zero;
                    mana -= 5;
                    manaCounter.fillAmount = mana / 100;
                }
            }
        }
        if (InputManager.e.mouseHeldDown && mana >= 90){
            Invoke("StopSuperAttack",1f);
            SuperAttack();
        }  
        if (InputManager.e.mouseClicked && swordAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
            if (CanStrike(transform.position,boss.position,meleeRange)){
                shake.e.Shake(0.6f,0.1f);
                mana += 10;
                manaCounter.fillAmount = mana / 100;
                boss.gameObject.GetComponent<bossScript>().health -= 3;
                Instantiate(struckParticles,Vector3.zero,Quaternion.identity);
            }
            swordAnim.SetTrigger("Slash");
        }
        if (isSuperAttacking){
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 20;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        beamPivot.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
    bool CanStrike(Vector3 a, Vector3 b, float d){
        return (a - b).sqrMagnitude < d * d && Dot2D(a,b) == GetDir();
    }
    int Dot2D(Vector3 a, Vector3 b){
        // Vector3.Dot didn't work. Maybe it's a perspective thing.
        if (a.x < b.x){
            return 1;
        }else{
            return -1;
        }
    }
    int GetDir(){
        return GetComponent<move>().dir;
    }
    void SuperAttack(){
        mana = 0;
        shake.e.Shake(1,1);
        beamPivot.gameObject.SetActive(true);
        Time.timeScale = 0.2f;
        isSuperAttacking = true;
        manaCounter.fillAmount = mana / 100;
    }
    void StopSuperAttack(){
        beamPivot.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isSuperAttacking = false;
    }
}
