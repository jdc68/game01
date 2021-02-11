using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public class Character
    {
        public string name;
        public string idle;
        public string run;
        public string jump;
        public string hit;
        public string ab;

        public Character(string _name = "", string _idle = "", string _run = "", string _jump = "", string _hit = "", string _ab = "")
        {
            name = _name;
            idle = _idle;
            run = _run;
            jump = _jump;
            hit = _hit;
            ab = _ab;
        }
    }

    public float speed;
    public float jumpSpeed;
    public float fallMultilier = 2.5f;
    public float lowFallMultiplier = 2f;
    public float enemyDetectionRadius = 30f;
    public LayerMask enemyMask;
    public float zoomSpeed;
    public float zoomTarget;
    public float zoomDefault;
    public float zoomMagnifier = 0;
    public bool continuousJumping;
    public bool customSpawn = false;

    private float move;
    public bool grounded;
    [HideInInspector] public bool jumping;
    private bool zoomActive;
    [HideInInspector]
    public bool isAttacking = false;
    [HideInInspector]
    public bool thrust = false;

    [SerializeField]
    public Transform groundCheck;
    public Transform groundCheck_;

    public ParticleSystem dustPS;

    private Collider2D[] enemies;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D box;
    private CapsuleCollider2D capsule;

    public characterSwitch characterSwitch;
    private GameObject activeCharacter;
    [HideInInspector]
    public Character localActiveCharacter;

    const string SEREJA_IDLE = "sereja_idle";
    const string SEREJA_RUN = "sereja_run";
    const string SEREJA_JUMP = "sereja_jump";

    const string ROMA_IDLE = "roma_idle";
    const string ROMA_RUN = "roma_run";
    const string ROMA_JUMP = "roma_jump1";
    const string ROMA_HIT = "roma_hit";
    const string ROMA_AB = "roma_dash";

    const string TANYA_IDLE = "tanya_idle";
    const string TANYA_RUN = "tanya_run";
    const string TANYA_JUMP = "tanya_jump";
    const string TANYA_HIT = "tanya_hit";
    const string TANYA_AB = "tanya_hook";

    private Character Sereja = new Character("Sereja", SEREJA_IDLE, SEREJA_RUN, SEREJA_JUMP);
    private Character Roma = new Character("Roma", ROMA_IDLE, ROMA_RUN, ROMA_JUMP, ROMA_HIT, ROMA_AB);
    private Character Tanya = new Character("Tanya", TANYA_IDLE, TANYA_RUN, TANYA_JUMP, TANYA_HIT, TANYA_AB);

    private List<Character> characters = new List<Character>();
    private string currentState;
    private int charactersCount;
    [HideInInspector]
    public bool ability;
    [HideInInspector]
    public bool canDoubleJump = true;
    [HideInInspector]
    public bool doubleJumping;
    int steps = 0;
    public float jumpMultiplier;
    

    private void Start()
    {
        // Add all characters to the list
        characters.Add(Sereja);
        characters.Add(Roma);
        characters.Add(Tanya);

        charactersCount = characters.Count;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        capsule = GetComponent<CapsuleCollider2D>();

        Physics2D.IgnoreLayerCollision(8, 15);

        if (!customSpawn)
        {
            transform.parent.position = FindObjectOfType<PlayerRespawn>().transform.position;
        }
    }

    private void Update()
    {
        //int mask1 = 1 << LayerMask.NameToLayer("Ground");
        //int mask2 = 1 << LayerMask.NameToLayer("enemy");
        //int mask = mask1 | mask2;

        int mask = 1 << LayerMask.NameToLayer("Ground");

        activeCharacter = characterSwitch.activeCharacter;

        // Check if character list is complete
        if (characters.Count < charactersCount)
        {
            characters.Add(Sereja);
            characters.Add(Roma);
            characters.Add(Tanya);
        }

        for (int i = 0; i < characters.Count; i++)
        {
            if (activeCharacter.name == characters[i].name)
            {
                localActiveCharacter = characters[i];
            }
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultilier - 1) * Time.deltaTime;
        }

        if (Physics2D.Linecast(transform.position, groundCheck.position, mask) || Physics2D.Linecast(transform.position, groundCheck_.position, mask))
        {
            grounded = true;
            if (!ability)
            {
                capsule.enabled = false;
                box.enabled = true;
            }
            jumping = false;
            doubleJumping = false;
        } else
        {
            grounded = false;
        }

        move = Input.GetAxisRaw("Horizontal");
        
        if (!ability)
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);
        }

        if (continuousJumping)
        {
            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetButton("Jump") && grounded && !ability)
            {
                dustPS.Play();
                canDoubleJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                capsule.enabled = true;
                box.enabled = false;
                jumping = true;
            }
        } else
        {
            if (!Input.GetKey(KeyCode.LeftShift) && Input.GetButtonUp("Jump") && grounded && !ability)
            {
                canDoubleJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * jumpMultiplier);
                capsule.enabled = true;
                box.enabled = false;
                jumping = true;
            }
        }

        enemies = Physics2D.OverlapCircleAll(rb.position, enemyDetectionRadius, enemyMask);
        if (enemies.Length > 0)
        {
            zoomActive = true;
        }
        else if (enemies.Length == 0)
        {
            zoomActive = false;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(currentState)) {
            animator.Play(currentState);
        }
    }

    private void LateUpdate()
    {
        if (grounded && !jumping && !isAttacking && !ability)
        {
            if (move != 0)
            {
                changeAnimationState(localActiveCharacter.run);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                changeAnimationState(localActiveCharacter.idle);   
            }
        }

        if (!grounded && !isAttacking && !thrust && !doubleJumping && !ability)
        {
            try
            {
                changeAnimationState(localActiveCharacter.jump);
            } catch { }
        }
        if (doubleJumping && !isAttacking)
        {
            changeAnimationState("roma_jump");
        }
        if (move != 0 && grounded && !ability)
        {
            if (localActiveCharacter.name == "Tanya")
            {
                if (!FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[23].isPlaying && !FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[24].isPlaying)
                {
                    steps++;
                    if (steps % 2 == 0)
                    {
                        FindObjectOfType<AudioManager>().Play("Step3");
                    }
                    else
                    {
                        FindObjectOfType<AudioManager>().Play("Step4");
                    }
                }
            } else
            {
                if (!FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[5].isPlaying && !FindObjectOfType<AudioManager>().GetComponents<AudioSource>()[6].isPlaying)
                {
                    steps++;
                    if (steps % 2 == 0)
                    {
                        FindObjectOfType<AudioManager>().Play("Step1");
                    } else
                    {
                        FindObjectOfType<AudioManager>().Play("Step2");
                    }
                }
            }
        }
    }

    public void changeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
}

