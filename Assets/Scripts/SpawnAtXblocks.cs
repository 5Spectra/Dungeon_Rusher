using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAtXblocks : MonoBehaviour {

	public GameObject prefabsGiz;

	public int spawnRate = 15;

	public Transform objVector1, objVector2;

	Vector3 vetor;

	void Start () {

		float dis = Vector3.Distance(objVector1.position, objVector2.position);

		for (int i = 0; i < dis; i += spawnRate) {
			vetor = transform.TransformPoint(new Vector3( i, -0.2f));
			Instantiate(prefabsGiz, vetor, Quaternion.identity, transform);
		}
	}
}
