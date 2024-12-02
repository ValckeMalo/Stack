using System.Collections;
using TMPro;
using UnityEngine;

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
    private Transform startCameraTransform;
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

    public IEnumerator SpawnNextCube()
    {
        cameraStack.transform.position += new Vector3(0f, LastCube.localScale.y, 0f);
        yield return new WaitForSeconds(0.2f);
        spawner[indexSpawner].SpawnCube();
        indexSpawner = (indexSpawner + 1) % spawner.Length;
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
                startCameraTransform = cameraStack.transform;
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

        HUDMenu.SetActive(true);
        HUDGame.SetActive(false);
        cameraStack.transform.position = startCameraTransform.position;
        currentCube = null;
        LastCube = tower.transform.GetChild(0);
        indexSpawner = 0;
        score = 0;
        UpdateHUDScore();
    }

    public void UpdateHUDScore()
    {
        CubeSpeed = Mathf.Min(BaseSpeed + (IncrementSpeed * (score % ScoreToUpSpeed)), MaxSpeed);
        HUDGame.transform.GetComponentInChildren<TextMeshProUGUI>().text = score.ToString();
    }
}