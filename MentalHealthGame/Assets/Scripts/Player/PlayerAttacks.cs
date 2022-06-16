using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public Transform shootOrigin;
    public LayerMask shootableLayer;

    public float pistolRange;
    public float pistolDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("Pistol");

        Ray ray = new Ray(shootOrigin.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pistolRange, shootableLayer))
        {
            HandleHit(hit);
        }
    }

    private void HandleHit(RaycastHit hit)
    {
        if (hit.collider.gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(pistolDamage);
        }
    }
}
