using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField]private float speed = 2.0f;
    [SerializeField] private float sprintSpeed = 4.0f;
    public int maxStamina = 100;
    private float regen = 0.5f;
    public float rotationSpeed = 150;
    private float gravity = 8;
    private float rot = 0f;
    private bool isPaused = false;
    private bool isRunning = false;
    private Vector3 moveDir = new Vector3(0, -10, 0);
    private Animator anim;
    private CharacterController charController;
    private Rigidbody rb;
    public Interactable interactable;
    public UIManager uiManager;
    private Camera cam;
    public SphereCollider handColldider;
    public int MedKit = 0, PainKiller = 0, Bandage = 0, ScrapMetal = 0, MedicalSupply = 0;
    AudioManager audioManager;
    bool firstTime = false;

    public int currentHealth;
    public int currentStamina;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        uiManager = FindObjectOfType<UIManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        InvokeRepeating("Regenerate", 0.0f, 0.25f / regen);
        cam = Camera.main;
        audioManager = GetComponent<AudioManager>();
    }
    
    void Update()
    {
        if (isPaused == false)
        {

            charController.Move(Vector3.forward * 0.001f);
            //Punch
            if (Input.GetMouseButtonDown(0) && currentStamina >=10)
            {
                handColldider.enabled = true;
                audioManager.Play("Attack", 10);
                currentStamina -= 10;
                uiManager.UpdateStamina(currentStamina);
                anim.SetTrigger("punch");
            }
            
            //Movement
            if (Input.GetKey(KeyCode.W))
            {   
                if (firstTime == false)
                {
                    audioManager.Play("Run", 10);
                    firstTime = true;
                }
                isRunning = true;
                //Debug.Log("W down");
                anim.SetInteger("Condition", 1);
                moveDir = new Vector3(0, -10, 1);
                moveDir *= speed;
                moveDir = transform.TransformDirection(moveDir);
            }
            else
            {
                isRunning = false;
                firstTime = false;
                audioManager.Stop("Run");
                //Debug.Log("W up");
                anim.SetInteger("Condition", 0);
                moveDir = new Vector3(0, -10, 0);
            }

            if (Input.GetMouseButtonDown(1))
            {
                // We create a ray
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // If the ray hits
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    if (interactable != null)
                    {
                        SetFocus(interactable);
                    }
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (isRunning == false)
                {
                    anim.Play("jump");
                    //anim.SetTrigger("jump");
                }
                else if (isRunning == true)
                {
                    anim.Play("runJump");
                    //anim.SetTrigger("runjump");
                }
            }

            //if (Input.GetKey(KeyCode.Tab))
            //{
            //    anim.Play("FrontFlip");
            //}

            rot = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rot, 0);
            charController.Move(moveDir * Time.deltaTime);
        }
        else if(isPaused == true)
        {
            if(Input.GetKey(KeyCode.P))
            {
                
            }
        }
        
        if(currentHealth<= 0)
        {
            SceneManager.LoadScene(0);
        }

    }

    // Set our focus to a new focus
    void SetFocus(Interactable newFocus)
    {
        // If our focus has changed
        if (newFocus != interactable)
        {
            // Defocus the old one
            if (interactable != null)
                interactable.OnDefocused();

            interactable = newFocus;   // Set our new focus
            //motor.FollowTarget(newFocus);   // Follow the new focus
        }

        newFocus.OnFocused(transform);
    }

    // Remove our current focus
    void RemoveFocus()
    {
        if (interactable != null)
            interactable.OnDefocused();

        interactable = null;
        //motor.StopFollowingTarget();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.name == "HandCollider")
        {
            Debug.Log(other.gameObject.name);
            currentHealth -= 5;
            uiManager.UpdateHealth(currentHealth);
            audioManager.Play("Dmg", 5);
        }
        if(other.transform.name == "ExitTrigger")
        {
            Application.Quit();
        }
    }

    public void DisableCollider()
    {
        Debug.Log("Disable collider");
        handColldider.enabled = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
    }

    void Regenerate()
    {
        if (currentStamina < maxStamina)
            currentStamina += 1;
        uiManager.UpdateStamina(currentStamina);
    }
}
