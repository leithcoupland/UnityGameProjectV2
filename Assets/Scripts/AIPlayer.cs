using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class AIPlayer : MonoBehaviour {

	PlayerController playerController;
	MapNavigator mapNavigator;
	Vector3 destination;
	bool dead = false;
	bool targeting = false;
	private PlayerController nearestPlayer = null;

	void Start(){
		playerController = GetComponent<PlayerController> ();
		mapNavigator = MapNavigator.instance;
		destination = transform.position;
		StartCoroutine (MoveToSafePlatform());
		StartCoroutine (RandomMovement ());
		StartCoroutine (TrackClosestPlayer ());
		StartCoroutine (AlternateTargeting ());
		StartCoroutine (AttackTarget ());
	}

	void Update(){
		MoveToDestination ();
		AimAtTarget ();
		//Debug.DrawLine (transform.position + 30 * playerController.velocity, transform.position, Color.cyan);
	}

	private void MoveToDestination(){
		destination = new Vector3 (destination.x, transform.position.y, destination.z);
		Debug.DrawLine(transform.position, destination, Color.green);
		Vector3 direction = destination - transform.position;
		playerController.Move (direction);
	}

	private void AimAtTarget(){
		if (targeting) {
			if (nearestPlayer != null) {
				Vector3 predictedTargetPos = nearestPlayer.transform.position + 10 * nearestPlayer.velocity;
				Vector3 aimDir = predictedTargetPos - transform.position;
				playerController.Aim (aimDir);
				//Debug.DrawLine (transform.position, nearestPlayer.transform.position + 10 * nearestPlayer.velocity, Color.blue);
				Debug.DrawLine (transform.position, predictedTargetPos, Color.blue);
			}
		} else {
			playerController.Aim (Vector3.zero);
		}
	}

	private float SqrDistToNearestSafePlatform(){
		return (transform.position - mapNavigator.ClosestSafePlatform (transform.position).transform.position).sqrMagnitude;
	}

	private void FindNearbySafePlatform(){
		ArrayList reachablePlatforms = mapNavigator.ConnectedSafePlatforms (transform.position);
		if (reachablePlatforms.Count > 0) {
			Platform destinationPlatform = (Platform)reachablePlatforms [Random.Range (0, (int)(reachablePlatforms.Count - 1))];
			destination = mapNavigator.RandomPositionOnPlatform (destinationPlatform.transform.position);
		} else {
			destination = mapNavigator.RandomPositionOnPlatform(transform.position);
		}
	}

	private void FindNearestPlayer(){
		float closestSqrDist = float.MaxValue;
		PlayerController closestPlayer = null;
		foreach (PlayerController pc in GameRoundManager.instance.players) {
			if (pc != null && pc.playerNum != playerController.playerNum) {
				if ((transform.position - pc.transform.position).sqrMagnitude < closestSqrDist) {
					closestSqrDist = (transform.position - pc.transform.position).sqrMagnitude;
					closestPlayer = pc;
				}
			}
		}
		nearestPlayer = closestPlayer;
	}

	IEnumerator AlternateTargeting(){
		float refreshRate = .2f;
		while (!dead) {
			refreshRate = targeting?Random.Range(1f, 4f):Random.Range(0.4f, 2f);
			targeting = !targeting;
			yield return new WaitForSeconds (refreshRate);
		}
	}

	IEnumerator AttackTarget(){
		float refreshRate = .2f;
		while (!dead) {
			if (targeting && nearestPlayer != null && playerController.stamina/playerController.maxStamina > Random.Range(0f, 0.2f)){
				playerController.Attack();
			}
			refreshRate = Random.Range (0.1f, 1f);
			yield return new WaitForSeconds (refreshRate);
		}
	}

	IEnumerator TrackClosestPlayer(){
		float refreshRate = .2f;
		while (!dead) {
			FindNearestPlayer ();
			yield return new WaitForSeconds (refreshRate);
		}
	}

	IEnumerator MoveToSafePlatform(){
		float refreshRate = .2f;
		while (!dead) {
			Platform currentPlatform = mapNavigator.ClosestPlatform (transform.position);
			if (currentPlatform.pastExpired || SqrDistToNearestSafePlatform() > 1.5 * mapNavigator.CrossPlatformSqrDist()) {
				destination = mapNavigator.RandomPositionOnPlatform(transform.position);
			} 
			else if (currentPlatform.expired && mapNavigator.ClosestPlatform(destination).expired) {
				FindNearbySafePlatform ();
			}
			yield return new WaitForSeconds (refreshRate);
		}
	}

	IEnumerator RandomMovement(){
		float refreshRate = 0.1f;
		Platform oldPlat = mapNavigator.ClosestPlatform(transform.position);
		while (!dead) {
			refreshRate = Random.Range (0.2f, 0.6f);
			Platform closestPlat = mapNavigator.ClosestPlatform (transform.position + 30 * playerController.velocity);
			if (!closestPlat.expired) {
				if (closestPlat.transform.position == oldPlat.transform.position) {
					//destination = mapNavigator.RandomPositionOnPlatform (mapNavigator.RandomSafePlatform ().transform.position);
					destination = mapNavigator.RandomPositionOnPlatform (mapNavigator.RandomConnectedSafePlatform (closestPlat.transform.position).transform.position);
				}
			}
			oldPlat = mapNavigator.ClosestPlatform(transform.position);
			yield return new WaitForSeconds (refreshRate);
		}

	}
}
