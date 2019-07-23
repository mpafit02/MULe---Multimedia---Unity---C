using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	public static int score { get; set; }

    float lastEnemyKillTime;
	int streakCount;
	float streakExpiryTime = 1;

    GameObject[] heart1;
    GameObject[] heart2;
    GameObject[] heart3;

    void Start() {
		Enemy.OnDeathStatic += OnEnemyKilled;
		FindObjectOfType<Player> ().OnDeath += OnPlayerDeath;
    }

	void OnEnemyKilled() {
		if (Time.time < lastEnemyKillTime + streakExpiryTime) {
			streakCount++;
		} else {
			streakCount = 0;
		}

		lastEnemyKillTime = Time.time;

		score += 3 + 2 * streakCount;
	}

	void OnPlayerDeath() {
        Enemy.OnDeathStatic -= OnEnemyKilled;
	}

    public void OnNewWave()
    {
        heart1 = GameObject.FindGameObjectsWithTag("Heart1");
        heart3 = GameObject.FindGameObjectsWithTag("Heart3");
        heart2 = GameObject.FindGameObjectsWithTag("Heart2");
        foreach (GameObject heart in heart1)
        {
            //heart.SetActive(true);
            heart.GetComponent<MeshRenderer>().enabled = true;
            //Debug.Log("Activate 1");
        }
        foreach (GameObject heart in heart2)
        {
            //heart.SetActive(true);
            heart.GetComponent<MeshRenderer>().enabled = true;
            //Debug.Log("Activate 2");
        }
        foreach (GameObject heart in heart3)
        {
            //heart.SetActive(true);
            heart.GetComponent<MeshRenderer>().enabled = true;
            //Debug.Log("Activate 3");
        }
    }

    public void destroyHeartPreview(int index)
    {
        heart1 = GameObject.FindGameObjectsWithTag("Heart1");
        heart3 = GameObject.FindGameObjectsWithTag("Heart3");
        heart2 = GameObject.FindGameObjectsWithTag("Heart2");
        //Debug.Log(index);
        if (index == 0)
        {
            //heart1[0].SetActive(false);
            heart1[0].GetComponent<MeshRenderer>().enabled = false;
        }
        else if (index == 2)
        {
            heart3[0].GetComponent<MeshRenderer>().enabled = false;
            //heart3[0].SetActive(false);
        }
        else if (index == 1)
        {
            heart2[0].GetComponent<MeshRenderer>().enabled = false;
            //heart2[0].SetActive(false);
        }
        // Destroy
    }


}
