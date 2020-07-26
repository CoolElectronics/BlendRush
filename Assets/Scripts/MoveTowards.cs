using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    float force;
    [SerializeField]
    float rspeed;
    [SerializeField]
    bool isVelocity;
    [SerializeField]
    bool rotate;
    Rigidbody2D rb;
    void Start(){
       rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        if (isVelocity){
            rb.velocity = dir * force;
        }else{
            rb.AddForce(dir * force);
        }
        if (rotate){
            Vector2 v = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(v.x,v.y) * -Mathf.Rad2Deg);
        }
    }
    private void OnCollisionEnter2D(Collision2D col) {
        Destroy(gameObject);
    }
}
