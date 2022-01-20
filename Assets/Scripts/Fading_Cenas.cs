using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading_Cenas : MonoBehaviour {

	public Texture2D fadeTexture;	// the texture that will overlay the screen. This can be a black image or a loading graphic
	public float fadeSpeed = 0.25f;		// the fading speed

	float alpha = 1.0f;			// the texture's alpha value between 0 and 1
	int fadeDir = -1;			// the direction to fade: in = -1 or out = 1
	bool isFading;

	void Start(){
		StartCoroutine(AlphaMinus_Fade());
	}

	void OnGUI() {
		if (isFading == false)
			return;

		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = -1000;																
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);		
	}
		
	public void CallFadePlus (string cena)
	{
		StartCoroutine(AlphaPlus_Fade(cena));
	}

	IEnumerator AlphaPlus_Fade(string cena){

		isFading = true;

		fadeDir = 1;

		yield return new WaitUntil(()=> alpha >= 1);

		SceneManager.LoadScene(cena);

		//isFading = false;	o script n existe
	} 

	IEnumerator AlphaMinus_Fade(){

		isFading = true;

		fadeDir = -1;

		yield return new WaitUntil(()=> alpha <= 0);

		isFading = false;
	} 

}