using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking};
	State currentState;

	public ParticleSystem deathEffect;
    public ParticleSystem heartEffect;
    public static event System.Action OnDeathStatic;

    public ScoreKeeper scoreKeeper;
    public Transform HeartPreviewPosition1;
    public Transform HeartPreviewPosition2;
    public Transform HeartPreviewPosition3;
    UnityEngine.AI.NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;

	Color originalColour;

	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1;
	float damage = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

	bool hasTarget;

	void Awake() {
		pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			hasTarget = true;
			
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();
			
			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
		}
	}
	
	protected override void Start () {
		base.Start ();

		if (hasTarget) {
			currentState = State.Chasing;
			targetEntity.OnDeath += OnTargetDeath;

			StartCoroutine (UpdatePath ());
		}
	}

	public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth, Color skinColour) {
		pathfinder.speed = moveSpeed;

		if (hasTarget) {
			damage = Mathf.Ceil(targetEntity.startingHealth / hitsToKillPlayer);
		}
		startingHealth = enemyHealth;

		deathEffect.startColor = new Color (skinColour.r, skinColour.g, skinColour.b, 1);
		skinMaterial = GetComponent<Renderer> ().material;
		skinMaterial.color = skinColour;
		originalColour = skinMaterial.color;
	}

	public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		AudioManager.instance.PlaySound ("Impact", transform.position);
        if (damage >= health && !dead)
            { 
                if (OnDeathStatic != null) {
				OnDeathStatic ();
			}
			AudioManager.instance.PlaySound ("Enemy Death", transform.position);
			Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
		}
		base.TakeHit (damage, hitPoint, hitDirection);
	}

	void OnTargetDeath() {
		hasTarget = false;
		currentState = State.Idle;
	}

	void Update () {

		if (hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
					nextAttackTime = Time.time + timeBetweenAttacks;
					AudioManager.instance.PlaySound ("Enemy Attack", transform.position);
					StartCoroutine (Attack ());
				}

			}
		}

	}

	IEnumerator Attack() {

		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

		float attackSpeed = 3;
		float percent = 0;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {

			if (percent >= .5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
                if (targetEntity.health == 3)
                {
                    Destroy(Instantiate(heartEffect.gameObject, HeartPreviewPosition1.position, HeartPreviewPosition1.rotation) as GameObject, heartEffect.startLifetime);
                    scoreKeeper.destroyHeartPreview(0);
                }
                else if (targetEntity.health == 2)
                {
                    Destroy(Instantiate(heartEffect.gameObject, HeartPreviewPosition3.position, HeartPreviewPosition3.rotation) as GameObject, heartEffect.startLifetime);
                    scoreKeeper.destroyHeartPreview(2);
                }
                else if (targetEntity.health == 1)
                {
                    Destroy(Instantiate(heartEffect.gameObject, HeartPreviewPosition2.position, HeartPreviewPosition2.rotation) as GameObject, heartEffect.startLifetime);
                    scoreKeeper.destroyHeartPreview(1);
                }
                targetEntity.TakeDamage(damage);
            }

			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = originalColour;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}

	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}