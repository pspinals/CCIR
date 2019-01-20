using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary {
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    private Rigidbody rb;
    private AudioSource audioSource;
    public float speed;
    public float tilt;
    private int level;
    private int weapon;
    public Boundary boundary;
    public GameObject[] shots;
    public GameObject[] shots2;
   
    public Transform shotSpawn;
    
    public float fireRate;
    private float nextFire;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        level = GameObject.Find("Game Controller").GetComponent<GameController>().powerupLevel;
        weapon = DestroyByContact.nextWeapon;
        if((Input.GetButton("Fire1") || Input.GetButton("Jump")) && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            if(weapon == 0) {
                if(level <= 10) Instantiate(shots[level], shotSpawn.position, shotSpawn.rotation);
                else Instantiate(shots[shots.Length - 1], shotSpawn.position, shotSpawn.rotation);
            }
            else if (weapon == 1) {
                if(level <= 10) Instantiate(shots2[level], shotSpawn.position, shotSpawn.rotation);
                else Instantiate(shots2[shots2.Length - 1], shotSpawn.position, shotSpawn.rotation);
            }
            audioSource.Play();
        }

    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        rb = GetComponent<Rigidbody>();
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;
        rb.position = new Vector3(Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax), 0.0f, Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax));
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }


}
