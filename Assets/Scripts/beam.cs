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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m = GetComponent<move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time < 0){
            Time.timeScale = 0;
        }else{
            Time.timeScale = time;
        }

     if (Time.timeScale < 1){
         time += returnSpeed;
     }
     if (m.fGroundedRemember > 0){
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
     if (Input.GetMouseButtonDown(0)){
         if (canKnock){
             canKnock = false;
             shake.e.Shake(screenShake,duration);
             time = reducedTime;
             Destroy(Instantiate(particles,transform.position,Quaternion.identity),1f);
             rb.velocity += (Vector2)mouse_pos.normalized * -knockback;
         }
         
     }
    }
}
