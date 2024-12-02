using System.Collections;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }
    #endregion

    public enum DirectionCube
    {
        X,
        Z,
    }

    public Camera cameraStack;
    public Vector3 startCameraPos;
    public Transform LastCube;
    public MovingCube currentCube;
    public CubeSpawner[] spawner;
    public GameObject tower;
    public GameObject HUDMenu;
    public GameObject HUDGame;
    public DirectionCube directionCube = DirectionCube.Z;
    public int indexSpawner = 0;
    public int score = 0;
    public float CubeSpeed = 5f;
    public float MaxSpeed = 30f;
    public float IncrementSpeed = 5f;
    private float BaseSpeed;
    public float ScoreToUpSpeed = 15;
    public Gradient gradient;
    public TextMeshProUGUI bestscore;
    public AudioClip bad;
    public AudioClip good;
    public AudioSource source;
    public int perfect;
    public int perfectNeed = 5;
    public Color CurrentColor { get => gradient.Evaluate(score / 10f); }

    public IEnumerator SpawnNextCube()
    {
        cameraStack.transform.position += new Vector3(0f, LastCube.localScale.y, 0f);
        yield return new WaitForSeconds(0.2f);
        spawner[indexSpawner].SpawnCube();
        indexSpawner = (indexSpawner + 1) % spawner.Length;
    }

    public void OnEnable()
    {
        bestscore.text = $"Best Score : {PlayerPrefs.GetInt("Score")}";
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentCube == null)
            {
                HUDMenu.SetActive(false);
                HUDGame.SetActive(true);
                spawner[indexSpawner].SpawnCube();
                indexSpawner = (indexSpawner + 1) % spawner.Length;
                startCameraPos = cameraStack.transform.position;
                BaseSpeed = CubeSpeed;
            }
            else
            {
                currentCube.StopMoving();
            }
        }
    }

    public void Loose()
    {
        int towerCount = tower.transform.childCount - 1; //let the start
        for (int i = 0; i < towerCount; i++)
        {
            Destroy(tower.transform.GetChild(towerCount - i).gameObject);
        }

        if (PlayerPrefs.GetInt("Score") <= score)
        {
            PlayerPrefs.SetInt("Score", score);
        }
        score = 0;
        bestscore.text = $"Best Score : {PlayerPrefs.GetInt("Score")}";
        HUDMenu.SetActive(true);
        HUDGame.SetActive(false);
        cameraStack.transform.position = startCameraPos;
        currentCube = null;
        LastCube = tower.transform.GetChild(0);
        indexSpawner = 0;
        UpdateHUDScore();
    }

    public void UpdateHUDScore()
    {
        CubeSpeed = Mathf.Min(BaseSpeed + (IncrementSpeed * ((int)(score / ScoreToUpSpeed))), MaxSpeed);
        HUDGame.transform.GetComponentInChildren<TextMeshProUGUI>().text = score.ToString();
    }

    public void PlayBad()
    {
        perfect = 0;
        source.clip = bad;
        source.Play();
    }
    public void PlayGood()
    {
        source.pitch = 1f + ((perfect / 5f) * 3f);
        source.clip = good;
        perfect++;
        source.Play();
    }
}