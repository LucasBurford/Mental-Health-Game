using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraInteraction : MonoBehaviour
{
    public Image crossHair;
    public Color defaultColour;
    public Color enemyColour;
    public LayerMask interactLayer;

    public float rayLength;
    public bool enemyInSights;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CastRay();

        if (enemyInSights)
        {
            crossHair.color = enemyColour;
        }
        else
        {
            crossHair.color = defaultColour;
        }
    }

    private void CastRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, interactLayer))
        {
            enemyInSights = (hit.collider.gameObject.CompareTag("Enemy"));
        }
        else
        {
            enemyInSights = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * rayLength);
    }
}
