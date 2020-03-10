using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLaser : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;

    private GameObject spawnedLaser;

    // Start is called before the first frame update
    void Start()
    {
        spawnedLaser = Instantiate(laserPrefab, firePoint) as GameObject;
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            EnableLaser();
        }
        if (Input.GetMouseButton(0)) {
            UpdateLaser();
        }
        if (Input.GetMouseButtonUp(0)) {
            DisableLaser();
        }
    }

    void EnableLaser() {
        spawnedLaser.SetActive(true);
    }

    void UpdateLaser() {
        if (firePoint != null) {
            spawnedLaser.transform.position = firePoint.position;
        }
    }

    void DisableLaser() {
        spawnedLaser.SetActive(false);
    }
}
