using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetResolutionsCustom : MonoBehaviour {


	public TMP_Dropdown ddResolutions; 

	Resolution[] Allresolutions;

	List<int> optionsResolution = new List<int>(); //valores par = width e valores impar = height

	//=============================================================================================

	void Start () { 
		GetAllResolution();
		Check_Resolution();
	}

	//=============================================================================================

	public void SetResolucao (int resolutionIndex){	//colocar no dd como Dynamico

		Resolution resolution = Allresolutions[resolutionIndex];

		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

		DataManager.resolucao = resolution;
	}

	//=============================================================================================

	void Check_Resolution(){
		int saveResolutionIndex = 0;
		Resolution optResolution;

		if (DataManager.resolucao.width > 100){
			optResolution = DataManager.resolucao;
			Screen.SetResolution(optResolution.width, optResolution.height, Screen.fullScreen);		
		}

			for (int i = 0; i < Allresolutions.Length; i++){
				if (Allresolutions[i].width == Screen.currentResolution.width &&
					Allresolutions[i].height == Screen.currentResolution.height){

					saveResolutionIndex = i;
				}
				else
					saveResolutionIndex = 2;
		}
						

		ddResolutions.value = saveResolutionIndex;
		ddResolutions.RefreshShownValue();
	}

	//=============================================================================================

	void GetAllResolution(){

	//Parte 1 - Convertendo dd.texto em> texto em> texto.largura e texto.altura
		string FullTexto = "";

		List<TMP_Dropdown.OptionData> optiongData = ddResolutions.options;	

		string[] optionText = new string[optiongData.Count];

		for (int i = 0; i < optiongData.Count; i++) {
			optionText[i] = optiongData[i].text;

			optionText[i] = optionText[i].Replace(" x", "");

			string space = i == optiongData.Count - 1 ? "" : " ";
				
			FullTexto += optionText[i] + space;
		}
		//out "1000 1000 1000 1000"

		optionText = FullTexto.Split(' ');
		//out 1000 largura	//out 1000 altura

		for (int i = 0; i < optionText.Length; i++) {
			optionsResolution.Add(int.Parse (optionText[i]));
		}

	//Parte 2 - Convertendo string em resolução

		Resolution hzGet = Screen.currentResolution; //Pega o Taxa de Rate do monitor ou Hz;

		Allresolutions = new Resolution[Mathf.RoundToInt(optionsResolution.Count * .5f)];

		//W = 0 2 4 6 8 10	//H = 1 3 5 7 9 11

		for (int i = 0, w = 0, h = 1; w < optionsResolution.Count; i++, w += 2, h += 2) {
			Allresolutions[i].width = optionsResolution[w];
			Allresolutions[i].height = optionsResolution[h];
			Allresolutions[i].refreshRate = hzGet.refreshRate;
		}

	}
}
