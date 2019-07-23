﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5;
	public Crosshairs crosshairs;
    private int wavNumber = 0;

    public ParticleSystem deathEffect;
    Camera viewCamera;
    PlayerController controller;
	GunController gunController;
    GameObject[] scoreKeeper;

    public int getWaveNumber()
    {
        return wavNumber;
    }

	protected override void Start () {
		base.Start ();
	}

	void Awake()
    {
        scoreKeeper = GameObject.FindGameObjectsWithTag("ScoreKeeper");
        if (scoreKeeper.Length > 0)
        {
            scoreKeeper[0].GetComponent<ScoreKeeper>().GetComponent<ScoreKeeper>().OnNewWave();
        }
        controller = GetComponent<PlayerController> ();
        gunController = GetComponent<GunController> ();
        viewCamera = Camera.main;
		FindObjectOfType<Spawner> ().OnNewWave += OnNewWave;
	}

	void OnNewWave(int waveNumber)
    {
        if (scoreKeeper.Length > 0)
        {
            scoreKeeper[0].GetComponent<ScoreKeeper>().GetComponent<ScoreKeeper>().OnNewWave();
        }
        wavNumber = waveNumber - 1;
        health = startingHealth;
        gunController.ResetGunPreview();
        gunController.UnlockGun(waveNumber - 1);
        gunController.EquipGun(waveNumber - 1);
    }

	void Update () {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        // Movement input
        Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

		// Look input
		Ray ray = viewCamera.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.up * gunController.GunHeight);
		float rayDistance;

		if (groundPlane.Raycast(ray,out rayDistance)) {
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin,point,Color.red);
			controller.LookAt(point);
			crosshairs.transform.position = point;
			crosshairs.DetectTargets(ray);
			if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1) {
				gunController.Aim(point);
			}
		}

		// Weapon input
		if (Input.GetMouseButton(0)) {
			gunController.OnTriggerHold();
		}
		if (Input.GetMouseButtonUp(0)) {
			gunController.OnTriggerRelease();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			gunController.Reload();
		}

		if (transform.position.y < -10)
        {
            TakeDamage (health);
		}
	}


    public override void Die ()
    {
        Destroy(Instantiate(deathEffect.gameObject, transform.position, deathEffect.transform.rotation) as GameObject, deathEffect.startLifetime);
        AudioManager.instance.PlaySound ("Player Death", transform.position);
        base.Die ();
    }

}
