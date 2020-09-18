using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = normalSpeed;
    }

    public GameObject[] waypoints;
    public GameObject parent, playerExplotionPrefab;
    int current = 0;
    public float currentSpeed;
    public float slowSpeed, normalSpeed;
    float WPradius = .5f;
    public GameObject rectParticle;

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            currentSpeed = slowSpeed;
            rectParticle.GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentSpeed = normalSpeed;
            rectParticle.GetComponent<ParticleSystem>().Stop();
        }

        if (Mathf.Abs(waypoints[current].transform.position.x - transform.position.x) < WPradius)
        {
            current = Random.Range(0, waypoints.Length);
            if (current >= waypoints.Length)
            {
                current = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * currentSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Lose
            Debug.Log("hit");
            Instantiate(playerExplotionPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.GameLose();
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreTrigger"))
        {
            if (GameManager.Instance.isGameStarted)
            {
                other.GetComponentInChildren<ParticleSystem>().Play(); 
            }
            GameManager.Instance.AddScore();
        }
    }
}
