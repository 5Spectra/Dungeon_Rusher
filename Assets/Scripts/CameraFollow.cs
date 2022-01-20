using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	Transform personagem, cam;

	public float Speed;

	public Vector3 offSet;

	Personagem script_h;

	Vector3 position = Vector3.zero;

	void Start () {
		personagem = gameObject.transform;
		cam = Camera.main.gameObject.transform;


		Speed = DataManager.velocidade *.25f ;

		if (Speed < 5)
			Speed = 2.5f;
	}

	void FixedUpdate () {

			position = Vector3.Lerp (cam.position, personagem.position + offSet, Speed * Time.deltaTime);

			position.z = -10f;

			cam.position = position;
		}
	}
