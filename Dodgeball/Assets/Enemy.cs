using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Controls the behavior of an on-screen enemy.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// How fast the enemy can accelerate
    /// </summary>
    public float EnginePower = 1;

    /// <summary>
    /// How close it tries to get to the player
    /// </summary>
    public float ApproachDistance = 5;

    /// <summary>
    /// How fast the orbs it fires should move
    /// </summary>
    public float OrbVelocity = 10;

    /// <summary>
    /// How heavy the orbs it fires should be
    /// </summary>
    public float OrbMass = .5f;

    /// <summary>
    /// Period the enemy should wait between shots
    /// </summary>
    public float CoolDownTime = 1;

    /// <summary>
    /// Prefab for the orb it fires
    /// </summary>
    public GameObject OrbPrefab;

    /// <summary>
    /// Transform from the player object
    /// Used to find the player's position
    /// </summary>
    private Transform player;

    /// <summary>
    /// Our rigid body component
    /// Used to apply forces so we can move around
    /// </summary>
    private Rigidbody2D rigidBody;

    /// <summary>
    /// Vector from us to the player
    /// </summary>
    private Vector2 OffsetToPlayer => player.position - transform.position;

    /// <summary>
    /// Unit vector in the direction of the player, relative to us
    /// </summary>
    private Vector2 HeadingToPlayer => OffsetToPlayer.normalized;
    
    /// i added this ~ hyunali
    public float lapsedTime = 0;
    public Rigidbody2D enemyOrbRigidBody;

    /// <summary>
    /// Initialize player and rigidBody fields
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Fire if it's time to do so
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        // TODO
        if (Time.time > lapsedTime) // fire an orb every 1 seconds
        {
            Fire();
            lapsedTime = Time.time + CoolDownTime; // update the cooldown
        }
    }

    /// <summary>
    /// Make a new orb, place it next to us, but shifted one unit in the direction of the player
    /// Set its mass to OrbMass and its velocity to OrbVelocity units per second, in the direction of the player
    /// </summary>
    private void Fire()
    {
        // TODO
        // Vector2 enemyPosition = rigidBody.position;
        // Vector2 enemyOrbPosition = enemyPosition + HeadingToPlayer; // create new position to spawn orb at
        //
        // GameObject enemyOrb = Instantiate(OrbPrefab); // create new orb to fire
        // // Rigidbody2D enemyOrbRigidBody = enemyOrb.GetComponent<Rigidbody2D>();
        // enemyOrbRigidBody = enemyOrb.GetComponent<Rigidbody2D>();
        //
        // // enemyOrbRigidBody.position = enemyOrbPosition;
        // enemyOrb.transform.position = enemyOrbPosition;
        //
        // // Vector2 playerPosition = player.position;  // get player position to fire at
        // enemyOrbRigidBody.mass = OrbMass; // setting enemy orb mass
        // enemyOrbRigidBody.velocity = HeadingToPlayer * OrbVelocity; // update velocity of enemy orb

        Vector2 enemyPosition = transform.position; // enemy's position
        // GameObject enemyOrb = Instantiate(OrbPrefab); // creating new orb to fire
        Vector2 spawnPoint = enemyPosition + HeadingToPlayer;
        GameObject enemyOrb = Instantiate(OrbPrefab, spawnPoint, quaternion.identity); // creating new orb to fire
        
        enemyOrb.transform.position = spawnPoint;

        Rigidbody2D enemyOrbRigidBody = enemyOrb.GetComponent<Rigidbody2D>();
        
        // get the orb's rigidbody and apply acceleration
        enemyOrbRigidBody.mass = OrbMass; // setting enemy orb mass
        enemyOrbRigidBody.velocity = HeadingToPlayer * OrbVelocity; // update velocity of enemy orb



    }

    /// <summary>
    /// Accelerate in the direction of the player, unless we're nearby
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void FixedUpdate()
    {
        var offsetToPlayer = OffsetToPlayer;
        var distanceToPlayer = offsetToPlayer.magnitude;
        var controlSign = distanceToPlayer > ApproachDistance ? 1 : -1; 
        rigidBody.AddForce(controlSign * (EnginePower / distanceToPlayer) * offsetToPlayer);
    }

    /// <summary>
    /// If this is called, we're off screen, so give the player a point.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        ScoreKeeper.ScorePoints(1);
    }
}
