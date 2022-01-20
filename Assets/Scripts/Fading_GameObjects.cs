using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fading_GameObjects : MonoBehaviour {	// Fading On e Off sem trocar de cena, ou seja, entre objetos usando botões.
	
	public Texture2D fadeTexture;
	public float fadeSpeed = 0.25f;

	float alpha = 0f;
 	int fadeDirection;
	bool isFading;

	GameObject obj_ToOn, obj_ToOff;

	void OnGUI (){
		if (isFading == false)
			return;

		alpha += fadeDirection * fadeSpeed * Time.deltaTime;

		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -1000;	
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}

	//=============================Fading Start======================================//	

	public void Start_Fading(GameObject obj){ // obj = menu a ser ligado
		StopAllCoroutines();
		obj_ToOn = obj;
		StartCoroutine (Start_Fade());
	}


	IEnumerator Start_Fade(){

		isFading = true;

		fadeDirection = 1;

		yield return new WaitUntil(()=> alpha >= 1);

		StartCoroutine (End_Fade());

	} 

	IEnumerator End_Fade(){

		fadeDirection = -1;

		obj_ToOn.SetActive (true);
		obj_ToOff.SetActive (false);

		yield return new WaitUntil(()=> alpha <= 0);

		isFading = false;
	} 
	//=============================Fading End======================================//

	//=============================Buttons======================================//

	public void objTurn_Off (GameObject obj){ //pega o menu atual a ser deligado e coloca numa varivel aumentando assim o numero de argumentos do botao.
		obj_ToOff = obj;
	}


	//Simplesmente colocar em End_Fade soluciona tudo.
	/*IEnumerator trocarDeMenu(GameObject obj){ // tenta pegar o tempo do fade
		
		//float fadingSpeed = (0.5f / fadeSpeed);  //2 se for .25 | 1 se for .5 | etc
		//yield return new WaitForSeconds (fadingSpeed);
		extra.SetActive (false);
		obj.SetActive (true);

	}
	*/
	//=============================Buttons======================================//

}
