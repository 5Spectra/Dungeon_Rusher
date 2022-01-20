using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{	
	//scripts
	public DataManager dm;

	//audio
	public AudioMixer audioMixer;

	//outro
	public Button btnEndless;
	public GameObject estatisca_numbers;

	//opções
	public Toggle low, medio, high, FSreen;
	public Slider sliderMusic, sliderSFX;
	public TMP_Dropdown ddLanguages; 
	//o codigo da resolução por ser muito grade fica em outro script

	Fading_Cenas fc;

	//================== Funções MonoBehaviour ==================
	void Start () {	
		fc = GetComponent<Fading_Cenas>();

		Can_Endless();
		Check_Estasticas();
		Check_Quality();
		Check_Sounds();
		Check_FullScreen();
		Check_Language();
	}
		
	//==================== Funções Privadas ====================

	void Can_Endless(){ //verifica se já completou o jogo
		if (DataManager.gameComplete == true)
			btnEndless.interactable = true;
	}

	void Check_Estasticas(){

		TextMeshProUGUI[] estastica_txt = estatisca_numbers.GetComponentsInChildren<TextMeshProUGUI>();


		string[] array = dm.Estatistica_String();

		for (int i = 0; i < array.Length; i++) {	
			estastica_txt[i].text = array[i];
		}
	}

	void Check_Quality(){
		int qualityLevel = DataManager.grafico;

		if (qualityLevel == 0)
			low.isOn = true;
		else if (qualityLevel == 1)
			medio.isOn = true;
		else if (qualityLevel == 2)
			high.isOn = true;
	}	

	void Check_Sounds(){
		audioMixer.SetFloat("volumeMusic", DataManager.musica);
		audioMixer.SetFloat("volumeSFX", DataManager.sfx);

		sliderMusic.value = DataManager.musica;
		sliderSFX.value =  DataManager.sfx;
	}

	void Check_FullScreen(){
		bool FSvalue = DataManager.fullscreen;

		Screen.fullScreen = FSvalue;
		FSreen.isOn = FSvalue;
	}

	void Check_Language(){
		int lingua = DataManager.linguagem;
		ddLanguages.value = lingua;
		//metodo de mudar de lingua
		dm.Change_Language(lingua);
	}


	//==================== Funções Publicas ====================
	public void SetMusic(float volume){
		audioMixer.SetFloat("volumeMusic", volume);
		DataManager.musica = volume;
	}

	public void SetSFX(float volume){
		audioMixer.SetFloat("volumeSFX",volume);
		DataManager.sfx = volume;
	}

	public void SetQuality(int qualityIndex){
		QualitySettings.SetQualityLevel(qualityIndex);
		DataManager.grafico = qualityIndex;
	}

	public void SetFullScreen(bool full){
		Screen.fullScreen = full;
		DataManager.fullscreen = full;
	}

	public void SetLanguage(int language){
		DataManager.linguagem = language;
		dm.Change_Language(language);
	}

	//======================== Buttons =========================

	public void doReset (){
		dm.Game_Reset();
	}

	public void Quit(){
		Application.Quit();
	}

	public void MenuToShop(){
		string cena;

		if (DataManager.gameHistoria == true)
			cena = "Shop";
		else
			cena = "Historia_Inicio";

		fc.CallFadePlus(cena);
	}

}