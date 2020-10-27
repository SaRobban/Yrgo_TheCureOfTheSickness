﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask rayMask;
    public bool laser = true;
    public bool homingMissile = false;

    public float speed = 10;

    [Header ("FX")]
    public GameObject onHitFX;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (laser)
            LaserBeam();

    }

    private void LaserBeam()
    {
        Debug.DrawRay(transform.position, transform.forward * speed * Time.fixedDeltaTime, Color.red,1);

        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, transform.forward, out rayHit, speed * Time.fixedDeltaTime, rayMask)){
            if (onHitFX != null)
            {
                Instantiate(onHitFX, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
    }
}
