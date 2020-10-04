using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    Transform player;
    [SerializeField]
    Vector3 velocity;
    [SerializeField]
    float speed;
    Vector3 savedPos;
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = Vector3.SmoothDamp(transform.position, player.position,ref velocity, speed);
        target.z = -10;
        transform.position = target;
    }
    void OnPreRender(){
        savedPos = transform.position;
        transform.position = new Vector3(Mathf.Round(transform.position.x * 2) / 2, Mathf.Round(transform.position.y * 2) / 2, -10);
    }
    private void OnPostRender() {
        transform.position = savedPos;
    }
}
