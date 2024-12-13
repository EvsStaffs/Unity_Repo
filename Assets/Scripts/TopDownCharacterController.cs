using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class to control the top down character.
/// Implements the player controls for moving and shooting.
/// Updates the player animator so the character animates based on input.
/// </summary>
public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Variables

    //The inputs that we need to retrieve from the input system.
    private InputAction m_moveAction;
    private InputAction m_attackAction;

    //The components that we need to edit to make the player move smoothly.
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    
    //The direction that the player is moving in.
    private Vector2 m_playerDirection;
    //private Vector2 fireDefault = Vector2.down;
    private Vector2 lastDirection;
    private float _canFire; 
   

    [Header("Movement parameters")]
    //The speed at which the player moves
    [SerializeField] private float m_playerSpeed = 200f;
    //The maximum speed the player can move
    [SerializeField] private float m_playerMaxSpeed = 1000f;

    [Header("Projectile parameters")]

    [SerializeField] GameObject m_projectilePrefab;
    //Reference to our projectile object
    [SerializeField] Transform m_firePoint;
    //Where we will be spawning our projectile from
    [SerializeField] float m_projectileSpeed;
    //How fast our projectile will travel
    [SerializeField] float m_fireRate;
    //How fast our projectile will spawn


    #endregion

    /// <summary>
    /// When the script first initialises this gets called.
    /// Use this for grabbing components and setting up input bindings.
    /// </summary>
    private void Awake()
    {
        //bind movement inputs to variables
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_attackAction = InputSystem.actions.FindAction("Attack");
        
        //get components from Character game object so that we can use them later.
        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called after Awake(), and is used to initialize variables e.g. set values on the player
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc performance.
    /// This ensures that physics are calculated properly.
    /// </summary>
    private void FixedUpdate()
    {
        //clamp the speed to the maximum speed for if the speed has been changed in code.
        float speed = m_playerSpeed > m_playerMaxSpeed ? m_playerMaxSpeed : m_playerSpeed;
        
        //apply the movement to the character using the clamped speed value.
        m_rigidbody.linearVelocity = m_playerDirection * (speed * Time.fixedDeltaTime);
    }
    
    /// <summary>
    /// When the update loop is called, it runs every frame.
    /// Therefore, this will run more or less frequently depending on performance.
    /// Used to catch changes in variables or input.
    /// </summary>
    void Update()
    {
        // store any movement inputs into m_playerDirection - this will be used in FixedUpdate to move the player.
        m_playerDirection = m_moveAction.ReadValue<Vector2>();
        
        // ~~ handle animator ~~
        // Update the animator speed to ensure that we revert to idle if the player doesn't move.
        m_animator.SetFloat("Speed", m_playerDirection.magnitude);
        
        // If there is movement, set the directional values to ensure the character is facing the way they are moving.
        if (m_playerDirection.magnitude > 0)
        {
            m_animator.SetFloat("Horizontal", m_playerDirection.x);
            m_animator.SetFloat("Vertical", m_playerDirection.y);

            lastDirection = m_playerDirection;

        }

        //Debug.Log(m_playerDirection);

        // check if an attack has been triggered.

        if (m_attackAction.IsPressed() && Time.time > _canFire)
        {
            // just log that an attack has been registered for now
            // we will look at how to do this in future sessions.
            //Debug.Log("Attack!");

            // We can fire everytime enough time has passed as a relative
            // percentage of a second - m_fireRate of 0.5 is every half second / 0.1 is every tenth
            _canFire = Time.time + m_fireRate;
            Fire();
        }

        //if (m_attackAction.IsPressed())
        //{
        //    // just log that an attack has been registered for now
        //    // we will look at how to do this in future sessions.
        //    //Debug.Log("Attack!");

        //    Fire();
        //}

        //Debug.Log(m_playerDirection.normalized.ToString());
    }

    void Fire()
    {
        Vector2 fireDir = lastDirection;

        GameObject projectileToSpawn = Instantiate(m_projectilePrefab, m_firePoint.position, Quaternion.identity);

        if (m_playerDirection == Vector2.zero)
        {
            if (fireDir == Vector2.zero)
            {
                fireDir = Vector2.down;
            }

        }

        if (projectileToSpawn.GetComponent<Rigidbody2D>() != null)
        {

            projectileToSpawn.GetComponent<Rigidbody2D>().AddForce(fireDir.normalized * m_projectileSpeed, ForceMode2D.Impulse);

        }
        //Debug.Log(m_playerDirection);
    }


    //void Fire()
    //{
    //    GameObject projectileToSpawn = Instantiate(m_projectilePrefab, m_firePoint.position, Quaternion.identity);

    //    if (projectileToSpawn.GetComponent<Rigidbody2D>() != null)
    //    {
    //        projectileToSpawn.GetComponent<Rigidbody2D>().AddForce(Vector2.up * m_projectileSpeed, ForceMode2D.Impulse);
    //    }    

    //}
}
