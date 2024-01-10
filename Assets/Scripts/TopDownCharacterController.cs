using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Stuff
    //Reference to attached animator
    private Animator animator;

    //Reference to attached rigidbody 2D
    private Rigidbody2D rb;

    //reference to attached collider;
    private BoxCollider2D bC;

    //reference to attached sprite renderer;
    private SpriteRenderer sR;

    //The direction the player is moving in
    [SerializeField] private Vector2 playerDirection;

    //The speed at which they're moving
    [SerializeField] private float playerSpeed = 1f;

    [Header("Movement parameters")]
    //The maximum speed the player can move
    [SerializeField] private float playerMaxSpeed = 100f;
    #endregion

    private bool isDodge, hasDodgeUpgrade;
    [SerializeField]float dodgeDuration, dodgeLength, dodgeCooldown, dodgeCooldownLength;


    /// <summary>
    /// When the script first initialises this gets called, use this for grabbing componenets
    /// </summary>
    private void Awake()
    {
        //Get the attached components so we can use them later
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bC = GetComponent<BoxCollider2D>();
        sR = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Called after Awake(), and is used to initialize variables e.g. set values on the player
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// When a fixed update loop is called, it runs at a constant rate, regardless of pc perfornamce so physics can be calculated properly
    /// </summary>
    private void FixedUpdate()
    {
        //Set the velocity to the direction they're moving in, multiplied
        //by the speed they're moving
        if (!isDodge)
        {
            rb.velocity = playerDirection * (playerSpeed * playerMaxSpeed) * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// When the update loop is called, it runs every frame, ca run more or less frequeSntly depending on performance. Used to catch changes in variables or input.
    /// </summary>
    private void Update()
    {
        // read input from WASD keys
        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");

        // check if there is some movement direction, if there is something, then set animator flags and make speed = 1
        if (playerDirection.magnitude != 0)
        {
            animator.SetFloat("Horizontal", playerDirection.x);
            animator.SetFloat("Vertical", playerDirection.y);
            animator.SetFloat("Speed", playerDirection.magnitude);

            //And set the speed to 1, so they move!
            playerSpeed = 1f;


            if (Input.GetAxisRaw("Dodge") == 1)
            {
                if(dodgeCooldown < Time.deltaTime)
                {
                    Dodge();
                }
            }

            if(Input.GetKey(KeyCode.U))
            {
                hasDodgeUpgrade = true;
            }
            if (Input.GetKey(KeyCode.O))
            {
                hasDodgeUpgrade = true;
            }
        }
        else
        {
            //Was the input just cancelled (released)? If so, set
            //speed to 0
            playerSpeed = 0f;

            //Update the animator too, and return
            animator.SetFloat("Speed", 0);
        }

        // Was the fire button pressed (mapped to Left mouse button or gamepad trigger)
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoot (well debug for now)
            Debug.Log($"Shoot! {Time.time}", gameObject);
        }


    }

    void Dodge()
    {
        if (!isDodge)
        {
            StartCoroutine(BecomeDodge());
        }
    }

    private IEnumerator BecomeDodge()
    {
        Debug.Log("Dodging!");
        isDodge = true;
        if (hasDodgeUpgrade)
        {
            gameObject.layer = 7;
            sR.color = Color.white;
        }

        rb.velocity = playerDirection * dodgeLength * Time.deltaTime;

        yield return new WaitForSeconds(dodgeDuration);

        Debug.Log("Stopped Dodging!");
        isDodge = false;
        if (hasDodgeUpgrade)
        {
            gameObject.layer = 6;
            sR.color = Color.white;
        }
        dodgeCooldown = Time.deltaTime + dodgeCooldownLength;
    }
}
