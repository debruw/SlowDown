using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public int targetScore;
    public int speed = 3;
    public GameObject[] levelObstacles;

    private void LateUpdate()
    {
        if (!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)
        {
            foreach (GameObject item in levelObstacles)
            {
                item.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
            return;
        }
        if (levelObstacles.Length == 0)
        {
            return;
        }
        foreach (GameObject item in levelObstacles)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -speed);
            }
        }
    }
}
