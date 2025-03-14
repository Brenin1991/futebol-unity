using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PenaltyGame : MonoBehaviour
{
    public GameObject startScreen, countdownScreen, goalScreen, missScreen, finalScreen, gameScreen, targetScreen, placarPanel;
    public TMP_Text countdownText, scoreText, finalScoreText;
    public Image[] shotIndicators;
    public Sprite ballSprite;
    
    private int score = 0;
    private int shotsTaken = 0;
    private bool canShoot = false;
    private bool isStart = false;
    
    void Start()
    {
        ShowStartScreen();
    }
    
    public void StartGame()
    {
        startScreen.SetActive(false);
        StartCoroutine(PrepareShot());
    }
    
    IEnumerator PrepareShot()
    {
	if(isStart == false){
	   targetScreen.SetActive(true);
        }
        isStart = true;
        yield return new WaitForSeconds(2f);
        targetScreen.SetActive(false);
        countdownScreen.SetActive(true);
        yield return CountdownRoutine();
        countdownScreen.SetActive(false);
        canShoot = true;
        placarPanel.SetActive(true);
        gameScreen.SetActive(true);
    }
    
    IEnumerator CountdownRoutine()
    {
        countdownText.text = "Prepare-se!";
        yield return new WaitForSeconds(1f);
        
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        
        countdownText.text = "Chute!";
        yield return new WaitForSeconds(0.5f);
    }
    
    public void OnTargetHit(int points)
    {
        if (!canShoot) return;
        canShoot = false;
        
        score += points;
        scoreText.text = "Pontos: " + score;
        
        shotIndicators[shotsTaken].sprite = ballSprite;
        shotsTaken++;
        
        goalScreen.SetActive(true);
        StartCoroutine(NextShot());
    }
    
    public void OnMiss()
    {
        if (!canShoot) return;
        canShoot = false;
        
        shotIndicators[shotsTaken].sprite = ballSprite;
        shotsTaken++;
        
        missScreen.SetActive(true);
        StartCoroutine(NextShot());
    }
    
    IEnumerator NextShot()
    {
        yield return new WaitForSeconds(2f);
        goalScreen.SetActive(false);
        missScreen.SetActive(false);
        placarPanel.SetActive(false);
        
        if (shotsTaken < 3)
        {
            StartCoroutine(PrepareShot());
        }
        else
        {
            ShowFinalScreen();
        }
    }
    
    void ShowFinalScreen()
    {
        finalScoreText.text = "Pontuação final: " + score;
        finalScreen.SetActive(true);
        gameScreen.SetActive(false);
        StartCoroutine(ResetGame());
    }
    
    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3f);
        
        score = 0;
        shotsTaken = 0;
        scoreText.text = "Pontos: 0";
        isStart = false;
        
        foreach (var img in shotIndicators)
        {
            img.sprite = null;
        }
        
        finalScreen.SetActive(false);
        ShowStartScreen();
    }
    
    void ShowStartScreen()
    {
        startScreen.SetActive(true);
    }
}
