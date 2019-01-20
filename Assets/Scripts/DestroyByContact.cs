using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public GameObject gameControllerobject;
    private GameController gameController;
    public int scoreValue;
    public float baseHealth;
    public float enemyHealth;
    private float damage;
    public static int nextWeapon;
    public static int enemyCount;

    void Start() {
        gameControllerobject = GameObject.FindWithTag("GameController");
        if(gameControllerobject != null) 
            gameController = gameControllerobject.GetComponent<GameController>();
        if(gameControllerobject == null) Debug.Log("Cannot find 'GameController' script");
        enemyHealth = baseHealth + gameControllerobject.GetComponent<GameController>().wave + 1;
        nextWeapon = 1;
    }

	void OnTriggerEnter(Collider other) {
        if(other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Asteroid" || other.tag == "Boss") return;
        if(gameObject.tag == "BoltPlayer") return;
        if(explosion != null) Instantiate(explosion, transform.position, transform.rotation);
        if(other.tag == "Player") {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.DecreaseLives();
                if(gameController.lives == 0) {
                    Destroy(other.gameObject);
                    gameController.GameOver();
                } else
                    other.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            }
        if(gameObject.tag == "Enemy" || gameObject.tag == "Boss") {
            if(enemyHealth > 0) enemyHealth -= gameControllerobject.GetComponent<GameController>().damage;
            if(enemyHealth <= 0 ) {
                gameController.AddScore(scoreValue);
                Destroy(gameObject);
                enemyCount--;
                if(gameObject.tag == "Boss") {
                    nextWeapon = Random.Range(0, 2);
                }
            }
        }

        if(other.tag != "Player") Destroy(other.gameObject);
        if(other.tag != "BoltPlayer")
            Destroy(gameObject);
    }

}
