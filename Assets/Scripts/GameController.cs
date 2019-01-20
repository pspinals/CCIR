using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] asteroids;
    public GameObject[] enemies;
    public GameObject[] bosses;
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Vector3 spawnValues;
    public float spawnRange;
    public int asteroidCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    private int score;
    private int liveTreshold;
    private int nextLiveTreshold;
    private int[] scoreTresholds = { 0, 100, 200, 500, 1000, 2000, 5000, 10000, 20000, 50000, 100000 };

    private float[] enemySimpleSpawnPosX = { -70, -55, -40, -25, -10, 10, 25, 40, 55, 70 };
    private float[] enemySimpleSpawnPosZ = { 100, 85, 70, 55 };

    private float[][,] enemyShapedSpawnPos = new float[3][,] {
                                                new float[,]{{-50,100},{-40,100},{-30,100},{-20,100},{-10,100},{0,100},{10,100},{20,100},{30,100},{40,100},{50,100},
                                                {-40,90},{-30,90},{-20,90},{-10,90},{0,90},{10,90},{20,90},{30,90},{40,90},
                                                {-30,80},{-20,80},{-10,80},{0,80},{10,80},{20,80},{30,80},
                                                {-20,70},{-10,70},{0,70},{10,70},{20,70},
                                                {-10,60},{0,60},{10,60},
                                                {0,50} },
                                                new float[,] {{-50,100},{-40,100},{-30,100},{-20,100},{-10,100},{0,100},{10,100},{20,100},{30,100},{40,100},{50,100},
                                                {-40,90},{-30,90},{-20,90},{-10,90},{0,90},{10,90},{20,90},{30,90},{40,90},
                                                {-30,80},{-20,80},{-10,80},{0,80},{10,80},{20,80},{30,80},
                                                {-50,50},{-40,50},{-30,50},{-20,50},{-10,50},{0,50},{10,50},{20,50},{30,50},{40,50},{50,50},
                                                {-40,60},{-30,60},{-20,60},{-10,60},{0,60},{10,60},{20,60},{30,60},{40,60},
                                                {-30,70},{-20,70},{-10,70},{0,70},{10,70},{20,70},{30,70} },
                                                new float[,] {{-70,100},{-40,100},{-10,100},{10,100},{40,100},{70,100},
                                                {55,85},{-25,85},{25,85},{55,85},
                                                {-70,70},{-40,70},{-10,70},{10,70},{40,70},{70,70},
                                                {55,55},{-25,55},{25,55},{55,55},
                                                {-70,40},{-40,40},{-10,40},{10,40},{40,40},{70,40} }
    }; 
    public int wave;
    public int powerupLevel;
    public int lives;
    public int damage;
    private int waveType;

    private bool restart;
    private bool gameOver;

    void Start() {
        score = 0;
        wave = 0;
        damage = 1;
        liveTreshold = 2000;
        nextLiveTreshold = liveTreshold;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        Time.timeScale = 1.0f;
        UpdateScore();
        StartCoroutine (SpawnWaves());
    }

    void Update() {
        if(restart) {
            if(Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(1);
            }
            if(Input.GetKeyDown(KeyCode.Q)) {
                SceneManager.LoadScene(0);
            }
        }
    }

    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (!gameOver) {
            if(wave > 0 && wave % 3 == 0) {
                int bossChoice = Random.Range(0, bosses.Length);
                GameObject boss = bosses[bossChoice];
                Vector3 spawnPosition = new Vector3(0.0f, 0.0f, 85.0f);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(boss, spawnPosition, spawnRotation);
                DestroyByContact.enemyCount = 1;
            } else {
                waveType = Random.Range(0, 5);
                switch(waveType) {
                    case 0:
                        SimpleWave();
                        break;
                    case 1:
                        ShapedWave(0);
                        break;
                    case 2:
                        ShapedWave(1);
                        break;
                    case 3:
                        ShapedWave(2);
                        break;
                }
            }
            while(DestroyByContact.enemyCount > 0) {
                GameObject asteroid = asteroids[Random.Range(0, asteroids.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(asteroid, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
                if(gameOver) {
                    restartText.text = "Press 'R' to restart\nPress 'Q' to quit to menu";
                    restart = true;
                    break;
                }
            }
                yield return new WaitForSeconds(waveWait);
                wave++;
                if(wave % 3 == 0) damage++;
        }
    }

    public void AddScore(int newScoreValue) {
        score += newScoreValue;
        if(score > nextLiveTreshold && score > 0) {
            lives++;
            nextLiveTreshold += liveTreshold;
        }
        for(int i = 0; i < scoreTresholds.Length-1; i++)
            if(score >= scoreTresholds[i] && score < scoreTresholds[i + 1]) powerupLevel = i;
        UpdateScore();
    }

    public void GameOver() {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void DecreaseLives() {
        lives--;
        UpdateScore();
    }

    void UpdateScore() {
        scoreText.text = "Score: " + score.ToString() + "\nLives: " + lives.ToString()  + "\nWave: " + (wave+1).ToString();
    }

    void SimpleWave() {
        DestroyByContact.enemyCount = enemySimpleSpawnPosX.Length * enemySimpleSpawnPosZ.Length;
        for(int i = 0; i < enemySimpleSpawnPosX.Length; i++)
            for(int j = 0; j < enemySimpleSpawnPosZ.Length; j++) {
                int enemyChoice = Random.Range(0, enemies.Length);
                GameObject enemy = enemies[enemyChoice];
                Vector3 spawnPosition = new Vector3(enemySimpleSpawnPosX[i], 0.0f, enemySimpleSpawnPosZ[j]);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(enemy, spawnPosition, spawnRotation);
            }
    }

    void ShapedWave(int wave) {
        int count=0;
        for(int i = 0; i < enemyShapedSpawnPos[wave].GetLength(0); i++)
        {
                int enemyChoice = Random.Range(0, enemies.Length);
                GameObject enemy = enemies[enemyChoice];
                Vector3 spawnPosition = new Vector3(enemyShapedSpawnPos[wave][i,0], 0.0f, enemyShapedSpawnPos[wave][i,1]);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(enemy, spawnPosition, spawnRotation);
            count++;
        }
        DestroyByContact.enemyCount = count;
    }
}
