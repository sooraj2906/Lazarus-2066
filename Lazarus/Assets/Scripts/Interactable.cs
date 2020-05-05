using UnityEngine;



public class Interactable : MonoBehaviour
{

    public float radius = 3f;
    public Transform interactionTransform;
    public PlayerController playerController;

    bool isFocus = false;
    Transform player;

    bool hasInteracted = false;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public virtual void Interact()
    {
       Debug.Log("Interacting with " + transform.name);
    }

    void Update()
    {

        if (isFocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                // Interact with the object
                Interact();
                hasInteracted = true;
            }
        }
    }

    // Called when the object starts being focused
    public void OnFocused(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    // Called when the object is no longer focused
    public void OnDefocused()
    {
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    // Draw our radius in the editor
    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}