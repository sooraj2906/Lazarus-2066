using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelController : MonoBehaviour
{
    [SerializeField] private GameObject level5;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Vigil_Textured_NoAnimation")
        {
            Destroy(level5.gameObject);
        }
    }
}
