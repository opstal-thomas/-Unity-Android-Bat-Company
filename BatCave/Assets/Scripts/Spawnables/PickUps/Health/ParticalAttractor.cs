using UnityEngine;
using System.Collections;

//by Joseph Jacir, adapted in part from Unity docs for ParticleSystem.GetParticles().
//Place this on an object to which you want to attract particles emitted from a specified ParticleSystem.
//Use sparingly: this may be slow, as it addresses each particle's position individually.

public class ParticalAttractor : MonoBehaviour
{

    public ParticleSystem pSys;
    public float attraction = 2.0f;
    public bool worldSpaceParticles = false;

    private ParticleSystem.Particle[] m_Particles;

    void LateUpdate()
    {
        if (pSys == null)
        {
            return;
        }

        InitializeIfNeeded();

        if (m_Particles.Length < 1)
        {
            return;
        }

        int numParticlesAlive = pSys.GetParticles(m_Particles);


        Vector3 target = transform.position;
        if (!worldSpaceParticles)
        {
            target = target - pSys.transform.position;
        }

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            m_Particles[i].position = Vector3.MoveTowards(m_Particles[i].position, target, Time.deltaTime * attraction);
        }

        // Apply the particle changes to the particle system
        pSys.SetParticles(m_Particles, numParticlesAlive);

    }

    void InitializeIfNeeded()
    {

        if (m_Particles == null || m_Particles.Length < pSys.maxParticles)
        {
            m_Particles = new ParticleSystem.Particle[pSys.maxParticles];
        }
    }

}
