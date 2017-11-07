using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPlatform : MonoBehaviour {

	// Class for testing distances between platforms.
	public GameObject target;


	void Update () {
		if (Input.GetKeyDown (KeyCode.F5)) {
			Vector3 posA = transform.position;
			Vector3 posB = target.transform.position;
			float dist = (posA - posB).sqrMagnitude;
			print ("SqrMag: " + dist);
			dist = Mathf.Sqrt (dist);
			print ("Dist: " + dist);
			dist = dist / 2f;
			print ("Effective R: " + dist);
		}
		if (Input.GetKeyDown(KeyCode.F6)){
			ArrayList platforms = MapNavigator.instance.ConnectedPlatforms (transform.position);
			foreach (Object p in platforms) {
				Platform pgo = (Platform)p;
				print (pgo.gameObject.name);
			}
		}
		if (Input.GetKeyDown (KeyCode.F7)) {
			for (int r = 2; r < 7; r++) {
				for (int t = 0; t < 3; t++) {
					int sqrdist = (r * 2 + t) * (r * 2 + t);
					print ("Radius: " + r + " Threshold: " + t + " = SqrDist: " + sqrdist);
				}
			}
		}
	}
}
