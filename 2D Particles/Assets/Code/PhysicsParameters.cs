/// <summary>
/// Global parameters for physics simulation
/// Can be adjusted in the editor using the ParameterSettings component.
/// </summary>
public static class PhysicsParameters
{
    /// <summary>
    /// Strength of gravity
    /// </summary>
    public static float G = 10;
    /// <summary>
    /// Strength of springs (all the same)
    /// </summary>
    public static float K = 1;
    /// <summary>
    /// Damping force on springs (all the same)
    /// </summary>
    public static float Damping = 0.1f;
    /// <summary>
    /// Treat springs as infinite stiffness when integrating with Verlet
    /// </summary>
    public static bool SpringsAreConstraints;
    /// <summary>
    /// Particle collisions absorb energy
    /// </summary>
    public static bool InelasticParticleCollisions;
    /// <summary>
    /// Screen collisions absorb energy
    /// </summary>
    public static bool InelasticScreenCollisions;
    /// <summary>
    /// Whether to use Verlet or Euler (hint: Eurler is nortoriously bad)
    /// </summary>
    public static IntegratorType Integrator = IntegratorType.Verlet;
    /// <summary>
    /// If true, Y coordinates are fixed.
    /// </summary>
    public static bool OneD;

    public static bool ShowConstraints = true;
}

public enum IntegratorType
{
    Euler,
    Verlet
}