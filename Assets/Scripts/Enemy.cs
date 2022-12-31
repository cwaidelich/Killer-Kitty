using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    const int MAX_HEALTH = 100;
    
    private Player cat;

    public int health;
    public float speed;
    bool rawRight;
    bool playerDead = false;

    public AudioSource audioSource;
    public AudioClip miceSound;

    [SerializeField] Animator animator;
    [SerializeField] List<Collider2D> colliders;
    [SerializeField] CharacterController2D characterController;
    UIController ui;

    // Start is called before the first frame update
    void Start()
    {
        cat = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        health = MAX_HEALTH;
        animator.SetInteger("AnimHealth", MAX_HEALTH);
        rawRight = true;
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerDead)
            return;
        int raw;
        if(health > 0)
        {
            raw = rawRight ? 1 : -1;
            characterController.Move(raw * speed * Time.fixedDeltaTime, false, false);
        }
        
    }

    private void Update()
    {
        if (cat.isDead)
            PlayerIsDead();
    }

    public void die()
    {
        health = 0;
        Debug.Log("attacked Mouse");
        animator.SetInteger("AnimHealth", 0);
    }

    public void stayDead()
    {
        Debug.Log("stayDead Mouse");
        animator.enabled = false;
        transform.gameObject.tag = "DeadMouse";
        GetComponent<Rigidbody2D>().simulated = false;
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rawRight = !rawRight;
        }
        if (collision.gameObject.tag == "Wand")
        {
            rawRight = !rawRight;
        }
        if (collision.gameObject.tag == "Mouse")
        {
            foreach (Collider2D collider in colliders)
            {
                Physics2D.IgnoreCollision(collider, collision.collider, true);

            }
        }
    }

    public void PlayerIsDead()
    {
        Debug.Log("Player is dead-not moving");
        playerDead = true;
    }

    public void onNightEnd()
    {
        if (health > 0)
            ui.livingMouse++;
        else
            ui.deadMouse++;
    }

}
