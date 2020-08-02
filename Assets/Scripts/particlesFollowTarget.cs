using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class particlesFollowTarget : MonoBehaviour
{
    ParticleSystem m_System;
    ParticleSystem.Particle[] m_Particles;
    public Transform target;
    public Transform startPoint;
    public float speed;
    public float rate;
    public float deflectSpeed;
    public float effectRange;
    public int numParticles = 0;
    public string objToFind;
    public string objToStart;
    public float innerRad = 2.0f;
    public float outerRad = 6.0f;
    public int timex = 0;
    private void Start() {
        transform.position = Vector3.zero;
        target = GameObject.Find(objToFind).transform;
        startPoint = GameObject.Find(objToStart).transform;
    }
    private void LateUpdate()
    {
        if (timex >= 0){
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = m_System.GetParticles(m_Particles);
        if (numParticles < numParticlesAlive){
            for (int i = 0; i < numParticlesAlive; i++){
                m_Particles[i].position = RandomCircle(startPoint.position, Random.Range(innerRad,outerRad), (i * 6) % 360);
            }
            numParticles = numParticlesAlive;
        }

        Vector3 offset = Vector3.zero;
        speed += rate;
        
        for (int i = 0; i < numParticlesAlive; i++)
        {   offset = (target.position - m_Particles[i].position).normalized * speed;
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
        timex = 0;
        }
        timex ++;
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
     float ang = a;
     Vector3 pos;
     pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
     pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
     pos.z = center.z;
     return pos;
 }
}