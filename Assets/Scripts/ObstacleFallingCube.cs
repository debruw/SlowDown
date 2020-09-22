using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFallingCube : MonoBehaviour
{
    bool isPlayed = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 5 && !isPlayed)
        {
            isPlayed = true;
            GetComponent<Animation>().Play();
        }
    }
}
