using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HookshotSystem : MonoBehaviour
{
    private bool canShoot;
    [SerializeField] private HookshotState currentHookshotState;
    [SerializeField] private GameObject hookshotOwner;
    [SerializeField] private float hookshotRange = 10.0f;
    [SerializeField] private float hookshotSpeed = 10.0f;
    [SerializeField] private GameObject hookshotBody;
    [SerializeField] private GameObject hookshotTip;
    [SerializeField] private GameObject laserPoint;

    [SerializeField]private LineRenderer lineRenderer;
    [SerializeField] private GameObject hookshotTarget;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.forward * 100, Color.red);
        lineRenderer.SetPosition(0, transform.forward * 100);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void FixedUpdate()
    {
        ShootRay();
    }

    public void ShootRay()
    {
        Ray hookshotRay = new Ray(transform.position, transform.forward);
        RaycastHit hookshotHit;

        Debug.Log("Raycasting");

        //Object is in range of hookshot
        if (Physics.Raycast(hookshotRay, out hookshotHit, hookshotRange))
        {
            Debug.Log("Hookshot is targetting: " + hookshotHit.collider.name);
            hookshotTarget = hookshotHit.collider.gameObject;

            //Check if object hit is hookshotable
            if (hookshotHit.collider.tag == "Hookshotable")
            {
                laserPoint.transform.position = hookshotHit.point;
            }
            else
            {
                laserPoint.transform.position = Vector3.zero;
            }
        }
        else //Object is not in range
        {
            laserPoint.transform.position = Vector3.zero;
            hookshotTarget = null;
        }

    }

    public void Shoot()
    {
        //Check if hookshot is can shoot
        if (canShoot)
        {
            canShoot = false;

            //Hookshot animation and stuff routine

            canShoot = true;
        }
    }
}

public enum HookshotState
{
    Aiming,
    Shooting,
    Pulling,
    Returning
}
