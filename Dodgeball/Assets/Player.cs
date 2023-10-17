using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Control the player on screen
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Prefab for the orbs we will shoot
    /// </summary>
    public GameObject OrbPrefab;

    /// <summary>
    /// How fast our engines can accelerate us
    /// </summary>
    public float EnginePower = 1;
    
    /// <summary>
    /// How fast we turn in place
    /// </summary>
    public float RotateSpeed = 1;

    /// <summary>
    /// How fast we should shoot our orbs
    /// </summary>
    public float OrbVelocity = 10;

    
    /// added this myself (Najma)
    // initializing the RigidBody2D's
    private Rigidbody2D playerRigidBody;
    private Rigidbody2D orbRigidbody;
    private Transform playerTransform;
    
    void Start()
    {
        // fetching the RigidBody from the GameObject
        playerRigidBody = GetComponent<Rigidbody2D>(); // fetch RigidBody2D for the player
        orbRigidbody = GetComponent<Rigidbody2D>(); // // fetch RigidBody2D for the orb
        playerTransform = this.transform; // the players transform
    }
    /// <summary>
    /// Handle moving and firing.
    /// Called by Unity every 1/50th of a second, regardless of the graphics card's frame rate
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void FixedUpdate()
    {
        Manoeuvre();
        MaybeFire();
    }

    /// <summary>
    /// Fire if the player is pushing the button for the Fire axis
    /// Unlike the Enemies, the player has no cooldown, so they shoot a whole blob of orbs
    /// </summary>
    void MaybeFire()
    {
        // TODO

        if (Input.GetButton("Fire")) // fire whenever the trigger (R2) is pulled
        {
            FireOrb();
            // FireOrb();   //     lol
            // FireOrb();
            // FireOrb();
            // FireOrb();
            // FireOrb();
            // FireOrb();
            // FireOrb();   //     yikes ...
            // FireOrb();
            // FireOrb();
            // FireOrb();   //      >:)
        }
        
    }

    /// <summary>
    /// Fire one orb.  The orb should be placed one unit "in front" of the player.
    /// transform.right will give us a vector in the direction the player is facing.
    /// It should move in the same direction (transform.right), but at speed OrbVelocity.
    /// </summary>
    private void FireOrb()
    {
        // TODO
        Vector2 transformRight = playerTransform.right; // direction the player is facing
        Vector2 playerPosition = playerTransform.position; // the player's position
        Vector2 orbPosition = playerPosition + transformRight; // offsetting new orb position
        
        GameObject playerOrb = Instantiate(OrbPrefab, orbPosition, quaternion.identity); // creating an orb
        Vector2 newVelocity = transformRight * OrbVelocity; // calculating velocity for the orb
        orbRigidbody.velocity = newVelocity; // setting the orb's new velocity
    }

    /// <summary>
    /// Accelerate and rotate as directed by the player
    /// Apply a force in the direction (Horizontal, Vertical) with magnitude EnginePower
    /// Note that this is in *world* coordinates, so the direction of our thrust doesn't change as we rotate
    /// Set our angularVelocity to the Rotate axis time RotateSpeed
    /// </summary>
    void Manoeuvre()
    {
        // TODO
        // names for controller mapping (accurate to DualSense Controller)
        // string xAxis = "Horizontal"; // horizontal direction of joystick (X axis)
        // string yAxis = "Vertical"; // vertical direction of joystick (Y axis -- inverted)
        // string rotateAxis = "Rotate"; // rotation input from joystick (Axis 3)
        // Vector2 moveTowards; // create a vector for new direction
        
        // controller input
        // float rotation = Input.GetAxisRaw("Rotate");
        // var xDir = Input.GetAxis("Horizontal"); // Horizontal * EnginePower
        // var yDir = Input.GetAxis("Vertical"); // Vertical * EnginePower
        
        // calculating direction/rotation
        // moveTowards = new Vector2(xDir, yDir); // creating a vector of the new direction
        // rotation *= RotateSpeed; // scaling rotation by given rotation speed

        // applying the new forces
        // playerRigidBody.AddForce(moveTowards * EnginePower); // moving RigidBody2D in new direction
        // playerRigidBody.AddForce(new Vector2(xDir,yDir) * EnginePower); // moving RigidBody2D in new direction
        playerRigidBody.AddForce(new Vector2(Input.GetAxis("Horizontal"),
                                             Input.GetAxis("Vertical")) * EnginePower); // moving RigidBody2D in new direction
        playerRigidBody.angularVelocity = (Input.GetAxisRaw("Rotate") * RotateSpeed); // applying rotation to RigidBody2D
    }

    /// <summary>
    /// If this is called, we got knocked off screen.  Deduct a point!
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        ScoreKeeper.ScorePoints(-1);
    }
}
