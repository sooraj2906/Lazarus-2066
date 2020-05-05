using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Camera cam;
    public GameObject player, playerHead;
    public bool isRotate = true;
    public float rotateSpeed = 5.0f;
    private Vector3 offset;
    private bool firstHit = false;
    RaycastHit oldHit;
    void Start()
    {
        offset = transform.position - player.transform.position;
        firstHit = false;
    }

    void Update()
    {
        float characterDistance = Vector3.Distance(transform.position, playerHead.transform.position);
        Debug.DrawRay(this.transform.position, (playerHead.transform.position - this.transform.position));
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, (playerHead.transform.position - this.transform.position), out hit, characterDistance))
        {
            if (oldHit.transform && hit.transform.name != "Player" && firstHit == true|| hit.transform.name == "Player" && firstHit == true)
            {
                //oldHit.collider.GetComponent<Renderer>().enabled = true;
                Renderer rend = oldHit.collider.GetComponent<Renderer>();
                Material[] myMaterials;
                myMaterials = rend.materials;
                for (int i = 0; i < myMaterials.Length; i++)
                {
                    if (myMaterials[i].name != "Glass (Instance)")
                    {
                        myMaterials[i].SetFloat("_Mode", 0);
                        myMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        myMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                        myMaterials[i].SetInt("_ZWrite", 1);
                        myMaterials[i].DisableKeyword("_ALPHATEST_ON");
                        myMaterials[i].DisableKeyword("_ALPHABLEND_ON");
                        myMaterials[i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        myMaterials[i].renderQueue = -1;
                    }
                    Color col = myMaterials[i].color;
                    if (myMaterials[i].name == "Glass (Instance)")
                        col.a = 0.35f;
                    else
                        col.a = 1;
                    myMaterials[i].SetColor("_Color", col);
                    //Debug.Log("Enabled");
                }
            }
            
            if (hit.transform.name != "Player")
            {
                firstHit = true;
                //hit.collider.GetComponent<Renderer>().enabled = false;
                Renderer rend = hit.collider.GetComponent<Renderer>();
                Material[] myMaterials;
                myMaterials = rend.materials;
                for (int i = 0; i < myMaterials.Length; i++)
                {
                    if (myMaterials[i].name != "Glass (Instance)")
                    {
                        myMaterials[i].SetFloat("_Mode", 3);
                        myMaterials[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                        myMaterials[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        myMaterials[i].SetInt("_ZWrite", 0);
                        myMaterials[i].DisableKeyword("_ALPHATEST_ON");
                        myMaterials[i].DisableKeyword("_ALPHABLEND_ON");
                        myMaterials[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        myMaterials[i].renderQueue = 3000;
                    }
                    Color col = myMaterials[i].color;
                    col.a = 0.1f;
                    myMaterials[i].SetColor("_Color", col);
                    oldHit = hit;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (isRotate == true)
        {
            Quaternion camTurnAngleHor = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up);
            offset = camTurnAngleHor *  offset;
        }
        transform.position = player.transform.position + offset;
        transform.LookAt(player.transform);
    }
}
