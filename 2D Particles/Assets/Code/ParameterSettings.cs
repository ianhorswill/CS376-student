using UnityEngine;

/// <summary>
/// Convenient component to let you edit physics parameters from the editor.
/// </summary>
public class ParameterSettings : MonoBehaviour
{
    [Header("Gravity")]
    public float G = 10;
    [Header("Spring parameters")]
    public float K = 1;
    public float Damping = 0.1f;
    public bool SpringsAreConstraints;
    public bool ShowConstraints = true;
    [Header("Collision handling")]
    public bool InelasticParticleCollisions;
    public bool InelasticScreenCollisions;
    [Header("Integration")]
    public IntegratorType Integrator = IntegratorType.Verlet;

    [Header("Other")] public bool OneD;

    // Use this for initialization
    internal void Start ()
    {
        PhysicsParameters.G = G;

        PhysicsParameters.K = K;
        PhysicsParameters.Damping = Damping;
        PhysicsParameters.SpringsAreConstraints = SpringsAreConstraints;
        PhysicsParameters.ShowConstraints = ShowConstraints;

        PhysicsParameters.InelasticParticleCollisions = InelasticParticleCollisions;
        PhysicsParameters.InelasticScreenCollisions = InelasticScreenCollisions;

        PhysicsParameters.Integrator = Integrator;

        PhysicsParameters.OneD = OneD;
    }
}
