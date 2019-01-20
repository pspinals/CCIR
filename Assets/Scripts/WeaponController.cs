using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;
    private AudioSource audioSource;
    public float fireRate;
    public float delay;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating("Fire", Random.Range(1,delay), Random.Range(2,fireRate));
	}

    void Fire() {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        audioSource.Play();
    }
	
}
