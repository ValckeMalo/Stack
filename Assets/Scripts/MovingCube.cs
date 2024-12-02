using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public float timeDeath = 1.5f;
    private bool moving = true;

    public void Awake()
    {
        GameManager.Instance.currentCube = this;
    }

    public void Update()
    {
        if (!moving) return;

        if (GameManager.Instance.directionCube == GameManager.DirectionCube.Z)
        {
            transform.position += transform.forward * Time.deltaTime * GameManager.Instance.CubeSpeed;
        }
        else
        {
            transform.position += transform.right * Time.deltaTime * GameManager.Instance.CubeSpeed;
        }
    }

    public void StopMoving()
    {
        moving = false;

        if (GameManager.Instance.directionCube == GameManager.DirectionCube.Z)
        {
            float delta = transform.position.z - GameManager.Instance.LastCube.position.z;
            if (Mathf.Abs(delta) >= GameManager.Instance.LastCube.transform.localScale.z)
            {
                GameManager.Instance.Loose();
                return;
            }

            SplitCubeOnZ(delta);
            GameManager.Instance.score++;
        }
        else
        {
            float delta = transform.position.x - GameManager.Instance.LastCube.position.x;
            if (Mathf.Abs(delta) >= GameManager.Instance.LastCube.transform.localScale.x)
            {
                GameManager.Instance.Loose();
                return;
            }

            SplitCubeOnX(delta);
            GameManager.Instance.score++;
        }
        GameManager.Instance.UpdateHUDScore();
    }

    private void SplitCubeOnZ(float delta)
    {
        float Zsize = GameManager.Instance.LastCube.transform.localScale.z - Mathf.Abs(delta);
        float sizeFallingPart = GameManager.Instance.LastCube.transform.localScale.z - Zsize;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Zsize);
        transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.LastCube.transform.position.z + (delta / 2f));

        float deltaSign = Mathf.Sign(delta);
        float currentCubeEdge = transform.position.z + (deltaSign * (Zsize / 2f));
        float fallingPosZ = currentCubeEdge + (deltaSign * (sizeFallingPart / 2f));

        SpawnFallingCubeOnZ(sizeFallingPart, fallingPosZ);

        GameManager.Instance.LastCube = transform;
        StartCoroutine(GameManager.Instance.SpawnNextCube());
    }
    private void SpawnFallingCubeOnZ(float sizeFallingPart, float fallingPosZ)
    {
        GameObject fallingCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingCube.transform.SetParent(transform.parent);
        fallingCube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, sizeFallingPart);
        fallingCube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingPosZ);
        fallingCube.AddComponent<Rigidbody>();
        Destroy(fallingCube, timeDeath);
    }

    private void SplitCubeOnX(float delta)
    {
        float Xsize = GameManager.Instance.LastCube.transform.localScale.x - Mathf.Abs(delta);
        float sizeFallingPart = GameManager.Instance.LastCube.transform.localScale.x - Xsize;

        transform.localScale = new Vector3(Xsize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(GameManager.Instance.LastCube.transform.position.x + (delta / 2f), transform.position.y, transform.position.z);

        float deltaSign = Mathf.Sign(delta);
        float currentCubeEdge = transform.position.x + (deltaSign * (Xsize / 2f));
        float fallingPosX = currentCubeEdge + (deltaSign * (sizeFallingPart / 2f));

        SpawnFallingCubeOnX(sizeFallingPart, fallingPosX);

        GameManager.Instance.LastCube = transform;
        StartCoroutine(GameManager.Instance.SpawnNextCube());
    }
    private void SpawnFallingCubeOnX(float sizeFallingPart, float fallingPosX)
    {
        GameObject fallingCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        fallingCube.transform.SetParent(transform.parent);
        fallingCube.transform.localScale = new Vector3(sizeFallingPart, transform.localScale.y, transform.localScale.z);
        fallingCube.transform.position = new Vector3(fallingPosX, transform.position.y, transform.position.z);
        fallingCube.AddComponent<Rigidbody>();
        Destroy(fallingCube, timeDeath);
    }
}