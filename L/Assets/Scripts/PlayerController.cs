using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody _rb;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;
    public int maxCollectables;
    public GameObject[] collectables;

    private bool isPlaying;

    public GameObject playButton;
    public TextMeshProUGUI currTimeText;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;
        _rb.velocity = new Vector3(x, _rb.velocity.y, z);

        currTimeText.text = (Time.time - startTime).ToString("F2");
    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);

        //Contribution
        foreach(GameObject collectable in collectables)
        {
            collectable.SetActive(true);
        }
    }

    public void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        // Testing
        Debug.Log("Score: " + (-Mathf.RoundToInt(timeTaken * 1000.0f)));
        Leaderboard.instance.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);
        collectablesPicked = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            //Contribution
            other.gameObject.SetActive(false);
        }

        if (collectablesPicked == maxCollectables)
        {
            End();
        }
    }
}
