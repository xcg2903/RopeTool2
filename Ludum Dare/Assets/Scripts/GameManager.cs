using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float timePassed;
    [SerializeField]
    private GameObject timer;
    private TMP_Text timerText;
    [SerializeField]
    private TMP_Text endGameTime;
    [SerializeField]
    private GameObject endScreen;

    private TimeSpan timePlaying;
    private bool timerActive;

    private float elapsedTime;

    public float finalTime;

    // Start is called before the first frame update
    void Start()
    {
        endScreen.SetActive(false);
        timePassed = 0;
        timerText = timer.GetComponent<TMP_Text>();
        timerText.text = "Timer: 00:00.00";
        timerActive = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator UpdateTimer()
    {
        while(timerActive)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(300) - TimeSpan.FromSeconds(elapsedTime);
            string timePlayingStr = "Timer: " + timePlaying.ToString("mm':'s'.'ff");
            timerText.text = timePlayingStr;

            yield return null;
        }
    }

    public void GoToStart()
    {
        SceneManager.LoadScene("Start");
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void PlayerFinish()
    {
        timePlaying = TimeSpan.FromSeconds(300) - TimeSpan.FromSeconds(elapsedTime);
        string timePlayingStr = "Finish Time: " + timePlaying.ToString("mm':'s'.'ff");
        endGameTime.text = timePlayingStr;
    }
}
