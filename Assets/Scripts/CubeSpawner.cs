using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameManager.DirectionCube directionCube;

    public void SpawnCube()
    {
        GameManager.Instance.directionCube = directionCube;
        Vector3 cubePos;
        if (GameManager.Instance.currentCube == null)
        {
            cubePos = new Vector3(transform.position.x, GameManager.Instance.LastCube.transform.position.y + GameManager.Instance.LastCube.transform.localScale.y, transform.position.z);
        }
        else
        {
            if (directionCube == GameManager.DirectionCube.Z)
            {
                cubePos = new Vector3(GameManager.Instance.LastCube.transform.position.x, GameManager.Instance.LastCube.transform.position.y, transform.position.z);
            }
            else
            {
                cubePos = new Vector3(transform.position.x, GameManager.Instance.LastCube.transform.position.y, GameManager.Instance.LastCube.transform.position.z);
            }
            cubePos.y += GameManager.Instance.LastCube.transform.localScale.y;
        }
        GameObject cube = GameObject.Instantiate(cubePrefab, cubePos, Quaternion.identity, GameObject.Find("Tower").transform);
        cube.transform.localScale = GameManager.Instance.LastCube.transform.localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1f);
        Gizmos.color = Color.white;
    }
}