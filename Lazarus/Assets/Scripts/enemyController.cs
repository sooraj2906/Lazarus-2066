using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class enemyController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float playerRange = 5.0f;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private GameObject Player;
    [SerializeField] private float rotationSpeed = 10f;
    private int maxHealth = 10;
    private int currentHealth;
    public UIManager uiManager;
    public PlayerController playerController;
    private bool isFollow = false;
    private Vector3 startPos;
    public GameObject col;
    bool firstTime = false;
    AudioManager audioManager;

    float attackDelay =1, nextAttack = 0;
    bool isAttacking;

    void Start()
    {
        anim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
        playerController = FindObjectOfType<PlayerController>();
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        Agent = this.GetComponent<NavMeshAgent>();
        isFollow = false;
        startPos = this.transform.position;
        currentHealth = maxHealth;
        audioManager = GetComponent<AudioManager>();
        audioManager.Play("Idle", 10);
    }
    
    void Update()
    {
        //Wait for player in idle
        //Chase player when within range
        if(Vector3.Distance(this.transform.position, Player.transform.position) <= playerRange)
        {

            //Attack player if within attack range
            if (Vector3.Distance(this.transform.position, Player.transform.position) <= 1.25f || isAttacking)
            {
                //Agent.angularSpeed = 0;
                Agent.isStopped = true;
                anim.SetBool("walk", false);
                if (Time.time > nextAttack)
                {
                    StartCoroutine(Attack());
                    anim.SetTrigger("attack");
                    audioManager.Play("Attack", 10);
                    //playerController.currentHealth -= 10;
                    //uiManager.UpdateHealth(playerController.currentHealth);
                    nextAttack = Time.time + attackDelay;
                }
            }

            else
            {
                Agent.isStopped = false;
                isFollow = true;
                if(firstTime == false)
                {
                    audioManager.Play("Walk", 10);
                    audioManager.Stop("Idle");
                    firstTime = true;
                }
                anim.SetBool("walk", true);
                Agent.SetDestination(Player.transform.position);// - new Vector3(0.4f, 0, 0.4f));y
            }
        }
        //Go back to start position after player moves out of range
        else
        {
            Agent.isStopped = false;
            //Agent.angularSpeed = 120;
            Agent.SetDestination(startPos);
            isFollow = false;
            firstTime = false;
            if (Vector3.Distance(this.transform.position, startPos) <= 0.5f && anim.GetBool("walk") == true)
            {
                anim.SetBool("walk", false);
                audioManager.Stop("Walk");
                audioManager.Play("Idle", 10);
                firstTime = false;
            }
        }

        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "PlayerHand")
        {
            currentHealth -= 2;
            audioManager.Play("Dmg", 10);
            Debug.Log(currentHealth);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.6333f );
        isAttacking = false;
    }
}
