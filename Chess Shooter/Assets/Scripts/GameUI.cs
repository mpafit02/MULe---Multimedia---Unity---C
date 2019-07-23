using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {
    
    public GameObject allUI;
	public Image fadePlane;
	public GameObject gameOverUI;

    public RawImage gameOverBack;
    public RawImage countdown;
    public RectTransform newWaveBanner;
	public Text newWaveTitle;
	public Text newWaveEnemyCount;
	public Text scoreUI;
	public Text gameOverScoreUI;
	public RectTransform healthBar;

    Spawner spawner;
	Player player;

	void Start () {
        allUI.SetActive (true);
		player = FindObjectOfType<Player> ();
		player.OnDeath += OnGameOver;
	}

	void Awake() {
		spawner = FindObjectOfType<Spawner> ();
		spawner.OnNewWave += OnNewWave;
	}

	void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        scoreUI.text = ScoreKeeper.score.ToString("D6");
		float healthPercent = 0;
		if (player != null) {
			healthPercent = player.health / player.startingHealth;
		}
		healthBar.localScale = new Vector3 (healthPercent, 1, 1);
	}

	void OnNewWave(int waveNumber) {
        countdown.GetComponent<Countdown>().activate();
        string[] numbers = { "One", "Two", "Three", "Four", "Five" };
		newWaveTitle.text = "- Wave " + numbers [waveNumber - 1] + " -";
		string enemyCountString = ((spawner.waves [waveNumber - 1].infinite) ? "Infinite" : spawner.waves [waveNumber - 1].enemyCount + "");
		newWaveEnemyCount.text = "Enemies: " + enemyCountString;

		StopCoroutine ("AnimateNewWaveBanner");
		StartCoroutine ("AnimateNewWaveBanner");
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, 1), 1f, fadePlane));
        gameOverScoreUI.text = scoreUI.text;
        scoreUI.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
        StartCoroutine(Fade(new Color(0, 0, 0, 1), Color.clear, 1f, fadePlane));
        gameOverUI.SetActive(true);
    }

    void OnGameOver()
    {
        StartCoroutine(Timer());
    }

	IEnumerator AnimateNewWaveBanner() {

		float delayTime = 1.5f;
		float speed = 3f;
		float animatePercent = 0;
		int dir = 1;

		float endDelayTime = Time.time + 1 / speed + delayTime;

		while (animatePercent >= 0) {
			animatePercent += Time.deltaTime * speed * dir;

			if (animatePercent >= 1) {
				animatePercent = 1;
				if (Time.time > endDelayTime) {
					dir = -1;
				}
			}

			newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp (-170, 45, animatePercent);
			yield return null;
		}

	}
		
	IEnumerator Fade(Color from, Color to, float time, Image fadePlane) {
		float speed = 1 / time;
		float percent = 0;
        fadePlane.enabled = true;

        while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp(from,to,percent);
			yield return null;
        }
        fadePlane.enabled = false;
    }
    
    public void OnFadeComplete()
    {
        ScoreKeeper.score = 0;
        SceneManager.LoadScene("Game");
    }
    // UI Input
    public void StartNewGame()
    {
        ScoreKeeper.score = 0;
        SceneManager.LoadScene ("Game");
    }

	public void ReturnToMainMenu()
    {
        ScoreKeeper.score = 0;
        SceneManager.LoadScene ("Menu");
    }
}
