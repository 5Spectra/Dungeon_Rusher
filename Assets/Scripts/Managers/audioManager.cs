using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class audioManager : MonoBehaviour {

	public AudioSource music, sfx;

	public AudioClip[] loopMusics;
	float audioTamanho;

	int currentLoop = 0;


	void Start(){		
		StartCoroutine(LoopMusics());
	}

	IEnumerator LoopMusics() {

		while (true){
			music.clip = loopMusics[currentLoop];

			audioTamanho = loopMusics[currentLoop].length;

			music.Play();

			yield return new WaitUntil (()=> audioTamanho == music.time);

			currentLoop += 1;

			if (currentLoop > loopMusics.Length - 1)
				currentLoop = 0;			

		}
	}

	
	public void Play_SFX(AudioClip sound){
		sfx.clip = sound;
		sfx.Play();
	}

}
