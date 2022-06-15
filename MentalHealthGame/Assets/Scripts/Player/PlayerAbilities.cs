using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public Vector3 grappleToPos;

    public float grappleSpeed;
    public float grappleStoppingDistance;

    public bool shouldGrapple;

    private void Update()
    {
        if (shouldGrapple)
        {
            GrappleTo();
        }
    }

    private void GrappleTo()
    {
        Vector3 startPos = transform.position;

        transform.position = Vector3.Lerp(startPos, grappleToPos, grappleSpeed);
        CheckGrappleDistance();
    }

    private void CheckGrappleDistance()
    {
        if (Vector3.Distance(transform.position, grappleToPos) <= grappleStoppingDistance)
        {
            shouldGrapple = false;
        }
    }

    public void StartGrapple(Vector3 grappleToPoint)
    {
        grappleToPos = grappleToPoint;
        shouldGrapple = true;
    }
}
