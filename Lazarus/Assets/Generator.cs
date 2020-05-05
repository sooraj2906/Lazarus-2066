using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject player, exitdoor;
    public PlayerController playerController;
    public Inventory inventory;
    public float radius = 1.5f;
    public GameObject fix1, fix2, fix3;
    public int fix = 0;
    public Item ScrapMetal;
    public GameObject warning;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = FindObjectOfType<Inventory>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, this.transform.position) <= 1.5f)
        {
            if (Input.GetKey(KeyCode.F) && fix < 3)
            {
                if(playerController.ScrapMetal == 0)
                {
                    warning.SetActive(true);
                    Debug.Log("warning");
                }
                if (playerController.ScrapMetal > 0)
                {
                    if (fix == 0)
                    {
                        fix1.SetActive(true);
                        inventory.Remove(ScrapMetal);
                        playerController.ScrapMetal--;
                    }
                    else if (fix == 1)
                    {
                        fix2.SetActive(true);
                        inventory.Remove(ScrapMetal);
                        playerController.ScrapMetal--;
                    }
                    else if (fix == 2)
                    {
                        fix3.SetActive(true);
                        inventory.Remove(ScrapMetal);
                        playerController.ScrapMetal--;
                    }
                    fix++;
                }
            }
        }
        else if(Vector3.Distance(player.transform.position, this.transform.position) > 1.5f)
        {
            warning.SetActive(false);
        }

        if(fix == 3)
        {
            exitdoor.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }
}
