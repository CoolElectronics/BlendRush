﻿using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class particlesFollowTarget : MonoBehaviour
{
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    public Transform target;
    public float speed;
    public float rate;
    public float deflectSpeed;
    public float effectRange;
    public int numParticles = 0;
    private void Start() {
        transform.position = Vector3.zero;
        target = GameObject.Find("Player").transform;
    }
    private void LateUpdate()
    {
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = m_System.GetParticles(m_Particles);
        if (numParticles < numParticlesAlive){
            Debug.Log("NewParticle");
            for (int i = 0; i < numParticlesAlive; i++){
                m_Particles[i].position = RandomCircle(target.position, Random.Range(2.0f,6.0f), (i * 6) % 360);
            }
            numParticles = numParticlesAlive;
        }

        Vector3 offset = Vector3.zero;
        speed += rate;
        
        for (int i = 0; i < numParticlesAlive; i++)
        {   offset = (target.position - m_Particles[i].position).normalized * speed;
            Debug.DrawLine(m_Particles[i].position,m_Particles[i].position + offset,Color.green);
           //m_Particles[i].velocity += offset;
            m_Particles[i].velocity = offset;
            for (int x = 0; x < numParticlesAlive; x++){
                if ((m_Particles[x].position - m_Particles[i].position).sqrMagnitude > effectRange){
                    offset = -(m_Particles[x].position - m_Particles[i].position).normalized * deflectSpeed;
                    m_Particles[i].velocity += offset;
                }
            }
        }

        // Apply the particle changes to the Particle System
        m_System.SetParticles(m_Particles, numParticlesAlive);
        if (numParticlesAlive <= 5){
            Destroy(gameObject);
        }
    }

    void InitializeIfNeeded()
    {
        if (m_System == null)
            m_System = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
    }
     Vector3 RandomCircle(Vector3 center, float radius,int a)
 {
     Debug.Log(a);
     float ang = a;
     Vector3 pos;
     pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
     pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
     pos.z = center.z;
     return pos;
 }
}