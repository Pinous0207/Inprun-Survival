using UnityEngine;

public class Particle_Handler : MonoBehaviour
{
    public static Particle_Handler instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();    
    }

    public void OnParticle(MeshRenderer mesh)
    {
        transform.position = mesh.transform.position;
        UpdateParticleMesh(mesh);
        particleSystem.Play();
    }

    private void UpdateParticleMesh(MeshRenderer meshRenderer)
    {
        var shape = particleSystem.shape;
        shape.meshRenderer = meshRenderer;
    }
}
