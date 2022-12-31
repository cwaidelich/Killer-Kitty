using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    Animator animator;
    CharacterController2D controller2D;
    Rigidbody2D myRigidbody;
    Material material;
    public GameManager gameManager;
    private UIController ui;

    public float movementSpeed = 5f;

    float h_speed;
    bool jump;
    bool isAttacking;
    bool reAttack;

    GameObject[] plattforms;
    Collider2D[] colliders;

    const int MAX_ENERGY_LEVEL = 100;
    private int energyLevel;
    private int currentHealth;

    public int energy_per_jump;
    public int energy_per_attack;

    private bool invisible = false;
    public float invisible_time = 2f;

    [SerializeField] private GameObject attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public LayerMask endLayers;

    public bool isDead;

    bool active = false;

    public AudioClip meow;
    public AudioClip dying;

    public AudioSource audioSource;

    void Start()
    {


        animator = GetComponent<Animator>();
        controller2D = GetComponent<CharacterController2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();
        material = GetComponent<SpriteRenderer>().material;

        invisible = false;
        isAttacking = false;
        isDead = false;

        plattforms = GameObject.FindGameObjectsWithTag("Plattform");
        colliders = GetComponents<Collider2D>();

        currentHealth = 7;
        energyLevel = MAX_ENERGY_LEVEL;

    }

    private void restart()
    {
        currentHealth = 7;
        energyLevel = MAX_ENERGY_LEVEL;
        invisible = false;
        isAttacking = false;
        isDead = false;
        animator.SetBool("dead", false);
    }

    private void Update()
    {
        if (!active)
            return;

        if (isDead)
            return;

        h_speed = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(h_speed));
        animator.SetFloat("verticalSpeed", myRigidbody.velocity.y);
        animator.SetBool("grounded", controller2D.getGrounded());
        if (Input.GetButtonDown("Jump") && energyLevel > energy_per_jump)
        {
            Jump();
        }
        
        if (Input.GetButtonDown("Fire1") && energyLevel > energy_per_attack)
        {
            Attack();
        }

        JumpThroughPlattforms();

        ui.setEnergy((float) energyLevel/MAX_ENERGY_LEVEL);
    }

    void FixedUpdate()
    {
        controller2D.Move(h_speed * movementSpeed * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void setActive(bool val)
    {
        if (val == active)
            return;
        active = val;
        if (active)
            restart();
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("attack");
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit Rat: {enemy}");
            enemy.GetComponent<Enemy>().die();
        }

        energyLevel = energyLevel - energy_per_attack;

        Collider2D[] endField = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, endLayers);
    }

    void Jump()
    {
        jump = true;
        animator.SetBool("jump", true);
        energyLevel = energyLevel - energy_per_jump;
    }

    void JumpThroughPlattforms()
    {
        if (myRigidbody.velocity.y > 0)
        {
            foreach (GameObject plattform in plattforms)
            {
                foreach (Collider2D collider in colliders)
                {
                    Physics2D.IgnoreCollision(plattform.GetComponent<Collider2D>(), collider, true);
                }
            }
        }
        //else the collision will not be ignored
        else
        {
            foreach (GameObject plattform in plattforms)
            {
                foreach (Collider2D collider in colliders)
                {
                    Physics2D.IgnoreCollision(plattform.GetComponent<Collider2D>(), collider, false);
                }
            }
        }
    }



    public void finishJumping()
    {
        animator.SetBool("jump", false);
    }

    public void finishAttacking()
    {
        isAttacking = false;
    }

    void died()
    {
        StopCoroutine(invisibleCounter());
        isDead = true;
        animator.SetBool("dead", true);
        gameManager.PlayerDead();
        currentHealth = 7;
        audioSource.Stop();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Mouse" && !invisible && !isAttacking)
        {
            currentHealth--;
            StartCoroutine(invisibleCounter());
            audioSource.PlayOneShot(meow);
            if (currentHealth<=0)
            {
                died();
            }
            ui.loseHearth(currentHealth, invisible_time);
        }
    }



    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

    

    IEnumerator invisibleCounter()
    {
        invisible = true;
        Flash();

        yield return new WaitForSeconds(invisible_time);
        invisible = false;
        
    }

    #region flashing when hin

    private IEnumerator flashCoroutine;
    private void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = DoFlash();
        StartCoroutine(flashCoroutine);
    }

    private void SetFlashAmount(float flashAmount)
    {
        material.SetFloat("_FlashAmount", flashAmount);
    }

    private IEnumerator DoFlash()
    {
        float lerpTime = 0;

        while (lerpTime < invisible_time)
        {
            lerpTime += Time.deltaTime;
            float perc = lerpTime / invisible_time;

            SetFlashAmount(1f - perc);
            yield return null;
        }
        SetFlashAmount(0);
    }

    #endregion
}
