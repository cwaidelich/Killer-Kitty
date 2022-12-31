using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameStatus
{
    starting,
    playing,
    losing,
    endScene,
    winning
}

public class GameManager : MonoBehaviour
{

    public GameStatus status;

    public EnemySpawner enemySpawner;
    public Player cat;
    public TagNachtCycles dayNight;
    public UIController UI;
    public GameObject intro;
    public GameObject outro;
    public GameObject winningText;
    public GameObject loosingText;
    public GameObject endGame;

    public AudioClip gameWon;
    public AudioSource sounds;

    // Start is called before the first frame update
    void Start()
    {
        status = GameStatus.starting;
    }

    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            case GameStatus.starting:
                starting();
                break;
            case GameStatus.playing:
                playing();
                break;
            case GameStatus.losing:
                loosing();
                break;
            case GameStatus.endScene:
                activateLastScene();
                break;
            case GameStatus.winning:
                winning();
                break;
        }
    }

    void starting()
    {
        enemySpawner.setActive(false);
        cat.setActive(false);
        dayNight.setActive(false);
        intro.SetActive(true);
        UI.setActive(false);
        outro.SetActive(false);
        winningText.SetActive(false);
        loosingText.SetActive(false);
        endGame.SetActive(false);
    }
    

    void playing()
    {
        enemySpawner.setActive(true);
        cat.setActive(true);
        dayNight.setActive(true);
        intro.SetActive(false);
        UI.setActive(true);
    }
    void loosing()
    {
        enemySpawner.setActive(false);
        cat.setActive(false);
        dayNight.setActive(false);
        intro.SetActive(false);
        UI.setActive(false);
        outro.SetActive(true);
        winningText.SetActive(false);
        loosingText.SetActive(true);
    }

    void activateLastScene()
    {
        enemySpawner.setActive(false);
        dayNight.setActive(false);
        endGame.SetActive(true);
        sounds.PlayOneShot(gameWon);
    }

    void winning()
    {
        enemySpawner.setActive(false);
        cat.setActive(false);
        dayNight.setActive(false);
        intro.SetActive(false);
        UI.setActive(false);
        outro.SetActive(true);
        endGame.SetActive(false);
        winningText.SetActive(true);
        loosingText.SetActive(false);
    }

    public void startPlaying()
    {
        status = GameStatus.playing;
    }

    public void PlayerDead()
    {
        status = GameStatus.losing;
    }

    public void StartEndScene()
    {
        status = GameStatus.endScene;

    }

    public void PlayerWin()
    {
        status = GameStatus.winning;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("One");
    }
}
