using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    public int targetScore;
    public int speed = 3;

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isGameStarted || GameManager.Instance.isGameOver)
        {
            return;
        }
        transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
    }
}
