using System.Collections;
using System.Collections.Generic;
using TapticPlugin;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] waypoints;
    public GameObject parent, playerExplotionPrefab;
    int current = 0;
    public float currentSpeed;
    public float slowSpeed, normalSpeed;
    float WPradius = .5f;
    public GameObject rectParticle;
    SphereCollider myCollider;
    public Material PowerUpMaterial;
    Material normalMaterial;
    bool isPowerUpActive;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = normalSpeed;
        myCollider = GetComponent<SphereCollider>();
        normalMaterial = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && !isPowerUpActive)
        {
            if (GameManager.Instance.tutorial0.activeSelf)
            {
                GameManager.Instance.tutorial0.SetActive(false);
            }
            currentSpeed = slowSpeed;
            SoundManager.Instance.playSound(SoundManager.GameSounds.Slow);
            rectParticle.GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(0) && !isPowerUpActive)
        {
            currentSpeed = normalSpeed;
            SoundManager.Instance.stopSound(SoundManager.GameSounds.Slow);
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
        if (collision.gameObject.CompareTag("Obstacle") && !isPowerUpActive)
        {
            // Lose
            Debug.Log("hit");
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Medium);
            SoundManager.Instance.playSound(SoundManager.GameSounds.Hit);
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
        else if (other.CompareTag("Obstacle") && !isPowerUpActive)
        {
            Debug.Log("hit");
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
                TapticManager.Impact(ImpactFeedback.Medium);
            SoundManager.Instance.playSound(SoundManager.GameSounds.Hit);
            Instantiate(playerExplotionPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.GameLose();
            Destroy(gameObject);
        }
        else if (other.CompareTag("PowerUp"))
        {
            Debug.Log(other);
            currentSpeed = normalSpeed * 2;
            GameManager.Instance.currentLevelObject.GetComponent<LevelObject>().speed *= 2;
            GetComponent<MeshRenderer>().material = PowerUpMaterial;
            Destroy(other.gameObject);
            isPowerUpActive = true;
            StartCoroutine(WAitAndDeactivatePowerUp());
        }
    }

    IEnumerator WAitAndDeactivatePowerUp()
    {
        yield return new WaitForSeconds(5);
        isPowerUpActive = false;
        currentSpeed = normalSpeed;
        if (GameManager.Instance.currentLevelObject != null)
        {
            GameManager.Instance.currentLevelObject.GetComponent<LevelObject>().speed /= 2;
        }
        GetComponent<MeshRenderer>().material = normalMaterial;
    }
}
