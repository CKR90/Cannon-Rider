using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishLineParticles : MonoBehaviour
{
    public static FinishLineParticles Instance;

    public Transform Vehicle;
    public List<ParticleSystem> particles;
    private bool finish = false;
    private float timer = 0.2f;

    private int particleIndex = 0;

    private float dist = 0f;

    void Start()
    {
        Instance = this;
    }

    
    void Update()
    {
        if (!finish) return;

        timer += Time.deltaTime;

        if(timer >= 0.2f)
        {
            timer = 0f;
            if(particles.Count > particleIndex)
            {
                particles[particleIndex].Play();
                particleIndex++;
            }
            else
            {
                finish = false;
            }
        }

        Vector3 pos = Vehicle.position + Vector3.forward * dist;
        pos.y = transform.position.y;

        transform.position = pos;
    }

    public void FinishGame()
    {
        finish = true;

        dist = Vector3.Distance(Vehicle.position, transform.position);

    }
}
