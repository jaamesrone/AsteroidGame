using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : Singleton<GameManager> //GameManager talks/inherit to singleton
{
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI livesUI;
    public TextMeshProUGUI levelsUI;
    public int lives;
    public int score;
    public int highScore;
    public int currentLvl;

    public bool isPaused = false;

    public float spawnTimeMin = 5.0f;
    public float spawnTimeMax = 10.0f;

    public Transform RespawnPoint;
    public Transform spawnPoint;

    public GameObject daddyUFO;
    public GameObject babyUFO;
    public GameObject Player; 
    public GameObject playerPrefabs;

    public AsteroidData[] asteroids; //list of an array for my gameobject asteroids
    public string[] objectsTags = { "Large", "Medium", "Small", "DaddyUFO", "BabyUFO" };

    private List<GameObject> spawnedUFOs = new List<GameObject>();

    private float nextSpawnTime;
    private float daddyUFO_SpawnChance = 0.8f;

    private bool _loadingNextScene = false;

    private int LargeAsteroid = 10;
    private int MediumAsteroid = 25;
    private int SmallAsteroid = 50;
    private int DadSaucer = 75;
    private int BabySaucer = 125;

    private string savePath;

    private void Start()
    {
        Player = Instantiate(playerPrefabs, spawnPoint.position, spawnPoint.rotation);
        SpawnAsteroid();
        nextSpawnTime = Time.time + Random.Range(spawnTimeMin, spawnTimeMax);
        DontDestroyOnLoad(Player);
        savePath = Application.persistentDataPath + "/gameSave.save";
    }

    private void Update()
    {
        SpawnUFOs();
        UpdateScore();
        UpdateLevel();
        UpdateLives();
        PauseGame();
        if (!_loadingNextScene)
        {
            AsteroidChecker();
        }
        else if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            Destroy(gameObject);
            Destroy(Player);
        }
    }

    [System.Serializable]
    class GameData
    {
        public int lives;
        public int score;
        public int highScore;
        public int currentLvl;

        public GameData(int lives, int score, int highScore, int currentLvl)
        {
            this.lives = lives;
            this.score = score;
            this.highScore = highScore;
            this.currentLvl = currentLvl;
        }
    }

    public void PlayerHurt()
    {
        lives--;
        Player.transform.position = RespawnPoint.position;
        if (lives <= 0)
        {
            Destroy(Player);
            SceneManager.LoadScene(3);
        }
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        GameData data = new GameData(lives, score, highScore, currentLvl);

        formatter.Serialize(stream, data);
        stream.Close();

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        if (File.Exists(GameManager.Instance.savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GameManager.Instance.savePath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            GameManager.Instance.lives = data.lives;
            GameManager.Instance.score = data.score;
            GameManager.Instance.highScore = data.highScore;
            GameManager.Instance.currentLvl = data.currentLvl;

            SceneManager.LoadScene(GameManager.Instance.currentLvl);
        }
        else
        {
            Debug.LogError("Save file not found in " + GameManager.Instance.savePath);
        }
    }

    public void SpawnAsteroid()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 10); //GameObject that the AsteroidSpawner script it's attached to spawns in a random x and y that is from -10 to 10 in and x and y, and STAYS in the -10 z
        for (int i = 0; i < 1; i++) //spawns only 6 asteroids.
        {
            int randomIndex = Random.Range(0, asteroids.Length);
            AsteroidData asteroidData = asteroids[randomIndex];
            GameObject asteroid = Instantiate(asteroidData.AsteroidPrefab, spawnPosition, Quaternion.identity);
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f); //whenever an asteroid spawns it goes in a random direction
            asteroid.GetComponent<Rigidbody>().AddForce(randomDirection * asteroidData.Speed, ForceMode.Impulse); //makes the asteroid move.
        }
    }

    private void AsteroidChecker()
    {
        bool allObjectsDestroyed = true;
        foreach (string tag in objectsTags)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
            if (objectsWithTag.Length > 0)
            {
                allObjectsDestroyed = false;
                break;
            }
        }

        if (allObjectsDestroyed && !_loadingNextScene)
        {
            _loadingNextScene = true;
            StartCoroutine(LoadNextSceneAndSpawnAsteroids());
        }
    }


    private IEnumerator LoadNextSceneAndSpawnAsteroids()
    {
        yield return new WaitForEndOfFrame();
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            yield return new WaitForSeconds(2);
            SpawnAsteroid();
            _loadingNextScene = false;
        }
        
    }


    public void SpawnUFOs()
    {
        if (Time.time > nextSpawnTime)
        {
            float x = Random.Range(-15f, 15f);
            float y = Random.Range(-8f, 8f);
            Vector3 spawnPos = new Vector3(x, y, 10f);

            GameObject spawnUFO = null;
            if (Random.value < daddyUFO_SpawnChance && !spawnedUFOs.Contains(daddyUFO))
            {
                spawnUFO = daddyUFO;
            }
            else if (!spawnedUFOs.Contains(babyUFO))
            {
                spawnUFO = babyUFO;
            }

            if (spawnUFO != null)
            {
                spawnedUFOs.Add(spawnUFO);
                Instantiate(spawnUFO, spawnPos, Quaternion.identity);
            }

            nextSpawnTime = Time.time + Random.Range(spawnTimeMin, spawnTimeMax);
        }
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
        else
        {
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }
    }

    public void UpdateScore()
    {
        scoreUI.text = "Score: " + score.ToString();
    }

    public void UpdateLives()
    {
        livesUI.text = "Lives: " + lives.ToString();
    }

    public void UpdateLevel()
    {
        levelsUI.text = "Level: "+currentLvl.ToString();
        
    }
    
    public void AddLargeScore()
    {
        score += LargeAsteroid;
    }

    public void AddMediumScore()
    {
        score += MediumAsteroid;
    }

    public void AddSmallScore()
    {
        score += SmallAsteroid;
    }

    public void AddDaddyUFOScore()
    {
        score += DadSaucer;
    }

    public void AddBabySaucerScore()
    {
        score += BabySaucer;
    }
}
