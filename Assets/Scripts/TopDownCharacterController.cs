using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    #region Framework Stuff
    //Reference to attached animator
    public Animator animator;

    //Reference to attached rigidbody 2D
    private Rigidbody2D rb;

    //reference to attached collider;
    private BoxCollider2D bC;

    //reference to attached sprite renderer;
    private SpriteRenderer sR;

    //reference to attached PlayerStats custom script;
    public PlayerStats ps;

    private Weapon weapon;

    //The direction the player is moving in
    [SerializeField] private Vector2 playerDirection;

    [SerializeField] private Transform attackPos;
    
    #endregion

    [SerializeField] public Vector2 savedDirection;
    [SerializeField] Texture2D crosshair;
    [SerializeField] GameObject bulletPrefab;
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
        ps = GetComponent<PlayerStats>();
        weapon = GetComponentInChildren<Weapon>();
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
        if (!ps.isDodging && !ps.isAiming)
        {
            rb.velocity = playerDirection.normalized * (ps.speed * ps.speedMax) * Time.fixedDeltaTime;
        }
    }

    /// <summary>
    /// When the update loop is called, it runs every frame, ca run more or less frequeSntly depending on performance. Used to catch changes in variables or input.
    /// </summary>
    private void Update()
    {


        // read input from WASD keys whilst not dodging
        if (!ps.isDodging && !ps.isAiming)
        {
            playerDirection.x = Input.GetAxisRaw("Horizontal");
            playerDirection.y = Input.GetAxisRaw("Vertical");
        }

        if (!ps.isDodging && !ps.isAiming)
        {
            if (Input.GetAxisRaw("Dodge") == 1)
            {
                if (ps.dodgeCooldown <= Time.time)
                {

                    StartCoroutine(Dodge());
                }
            }
        }

        if ((Input.GetAxisRaw("reload") == 1) && !ps.isShooting && !weapon.reloading && (ps.gunMagCurrent != ps.gunMagBase))
        {
            weapon.StartReload();

        }
        // check if there is some movement direction, if there is something, then set animator flags and make speed = 1
        if (playerDirection.magnitude != 0)
        {
            animator.SetFloat("Horizontal", playerDirection.x);
            animator.SetFloat("Vertical", playerDirection.y);
            animator.SetFloat("Speed", (playerDirection.magnitude > 1) ? 1 : playerDirection.magnitude);

            //And set the speed to 1, so they move!
            ps.speed = 1f;
            

            if(Input.GetKey(KeyCode.U))
            {
                ps.dodgeSpecialUnlocked = true;
            }
            if (Input.GetKey(KeyCode.O))
            {
                ps.dodgeSpecialUnlocked = false;
            }

            savedDirection = playerDirection;
        }
        else
        {
            //Was the input just cancelled (released)? If so, set
            //speed to 0
            ps.speed = 0f;

            //Update the animator too, and return if not dodging
            if (!ps.isDodging && !ps.isAiming && !ps.isAttacking)
            {
                animator.Play("idleTree");
                animator.SetFloat("Speed", 0);
            }
        }

        // Was the fire button pressed (mapped to Left mouse button or gamepad trigger)
        if (Input.GetAxisRaw("Ready Aim") == 1)
        {
            ps.isAiming = true;
            InitiateAiming();
        }
        else if(Input.GetAxisRaw("Ready Aim") == 0)
        {

            ps.isAiming = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        }


        attackPos.localPosition = savedDirection;


    }


    private IEnumerator Dodge()
    {
        float t = 0;
        ps.isDodging = true;
        gameObject.layer = 7;
        ps.dodgeCooldown = Time.time + ps.dodgeCooldownLengthCurrent;
        while (t < ps.dodgeDuration)
        {

            animator.Play("rollTree");
            t += Time.deltaTime;
            rb.velocity = savedDirection.normalized * ps.dodgeVelocityCurrent;
            animator.SetFloat("Speed", 1);
            yield return null;
        }
        rb.velocity = savedDirection * 0;
        ps.isDodging = false;
        gameObject.layer = 6;
    }

    private void InitiateAiming()
    {
        Vector2 playerToMouseVector;
        animator.Play("idleTree");
        rb.velocity = new Vector2 (0, 0);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerToMouseVector = (transform.position - mousePosition) * -1;   
        playerDirection = playerToMouseVector.normalized;
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.Auto);
        
        if((ps.isAiming && Input.GetAxisRaw("Shoot") == 1) && !ps.isShooting)
        {
            ps.isShooting = true;
            weapon.Fire(bulletPrefab, transform.position, ps);

        }
        
    }

    /*void Fire(GameObject bulletPrefab, Vector2 position, PlayerStats ps)
    {
        Debug.Log("Firing!");
        GameObject bulletToSpawn = Instantiate(bulletPrefab, position, quaternion.identity);
        bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(savedDirection.normalized * ps.gunSpeedCurrent, ForceMode2D.Impulse);
        bulletToSpawn.GetComponent<Bullet>().bulletDamage = ps.gunDamageCurrent;
        bulletToSpawn.GetComponent<Bullet>().bulletSpeed = ps.gunSpeedCurrent;
        bulletToSpawn.GetComponent<Bullet>().owner = gameObject;
        bulletToSpawn.GetComponent<Bullet>().bulletLifetime = ps.gunLifetimeCurrent;

    }

    IEnumerator FiringCooldown()
    {
        float t = 0;
        while (t < ps.gunFireRateCurrent)
        {
            t += Time.deltaTime;
            yield return null;
        }

        ps.isShooting = false;
    }*/
}
