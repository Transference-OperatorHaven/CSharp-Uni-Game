using System;
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

    //reference to attached PlayerStats custom script;
    private PlayerStats pS;

    //The direction the player is moving in
    [SerializeField] private Vector2 playerDirection;
    #endregion


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
        pS = GetComponent<PlayerStats>();
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
        //if (!pS.isDodging)
        //{
            rb.velocity = playerDirection.normalized * (pS.playerSpeed * pS.playerSpeedMax) * Time.fixedDeltaTime;
        //}
    }

    /// <summary>
    /// When the update loop is called, it runs every frame, ca run more or less frequeSntly depending on performance. Used to catch changes in variables or input.
    /// </summary>
    private void Update()
    {

        // read input from WASD keys
        playerDirection.x = Input.GetAxisRaw("Horizontal");
        playerDirection.y = Input.GetAxisRaw("Vertical");

        // check if there is some movement direction, if there is something, then set animator flags and make speed = 1
        if (playerDirection.magnitude != 0)
        {
            animator.SetFloat("Horizontal", playerDirection.x);
            animator.SetFloat("Vertical", playerDirection.y);
            animator.SetFloat("Speed", (playerDirection.magnitude > 1) ? 1 : playerDirection.magnitude);

            //And set the speed to 1, so they move!
            pS.playerSpeed = 1f;


            if (Input.GetAxisRaw("Dodge") == 1)
            {
                if(pS.dodgeCooldown <= Time.time)
                {
                    Debug.Log("dodged!");
                    Dodge();
                }
            }

            if(Input.GetKey(KeyCode.U))
            {
                pS.dodgeSpecialUnlocked = true;
            }
            if (Input.GetKey(KeyCode.O))
            {
                pS.dodgeSpecialUnlocked = false;
            }
        }
        else
        {
            //Was the input just cancelled (released)? If so, set
            //speed to 0
            pS.playerSpeed = 0f;

            //Update the animator too, and return
            animator.Play("idleTree");
            animator.SetFloat("Speed", 0);
        }

        // Was the fire button pressed (mapped to Left mouse button or gamepad trigger)
        if (Input.GetButtonDown("Fire1"))
        {
            //Shoot (well debug for now)
            Debug.Log($"Shoot! {Time.time}", gameObject);
        }


    }

    private IEnumerator Dodge()
    {
        pS.isDodging = true;
        animator.Play("rollTree");
        pS.dodgeCooldown = Time.time + pS.dodgeCooldownPeriod;

        yield return new WaitForSeconds(pS.dodgeDuration);

        pS.isDodging = false;
    }


    
}
