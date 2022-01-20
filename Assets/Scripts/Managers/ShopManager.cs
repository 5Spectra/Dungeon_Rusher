using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ShopManager : MonoBehaviour {
	
	public DataManager dm;

//UI public variables
	public TMP_Text[] potionsQuant = new TMP_Text[3], atrLevel = new TMP_Text[4], itemLevel = new TMP_Text[4];
	public TMP_Text soulcoins;

	public TMP_Text txtPotencia;

	public TMP_Text potionPrice, potenciaPrice; 
	public TMP_Text[] atrPrice = new TMP_Text[4], itemPrice = new TMP_Text[4];

	public Button[] selectorAtaque = new Button[4], selectorDefesa = new Button[4],
	selectorMagia = new Button[4], selectorVelocidade = new Button[4];

	public Image[] showingSelectedSkill = new Image[3];

	public Button[] darkAtaque = new Button[4], darkDefesa = new Button[4],
	darkMagia = new Button[4], darkVelocidade = new Button[4]; 

	public GameObject[] darkEquips = new GameObject[3];

	[Tooltip("Icone para Skills:\n0 = vazio / 1-16 = skill icons")]
	public Sprite[] skillIcons = new Sprite[17];

	[Tooltip("Coloque o controler de cada cor do cavaleiro em ordem")]
	public RuntimeAnimatorController[] cavaleiroCores = new RuntimeAnimatorController[8];

	[Tooltip("Coloque todas as skill aqui")]
	public ScriptableObject[] skillsData = new ScriptableObject[17]; //0-15 Skills | 16 = null
	public ScriptableObject[] potionsData = new ScriptableObject[4]; //0-2 Potes | 3 = null

//Normal Variables
	int[] skillPrices = new int[] {250, 1500, 7500, 30000};
	int[] LevelRequired = new int[] {0, 5, 15, 25};
	int[] currentHab = new int [4];

// Data-Save variabels
	int ataque, defesa, magia, velocidade, elmo, armadura, arma, botas;
	public int _moedas;
	int _potencia = 1, quantBuff, quantCDR, quantCura;

	bool[] skillsAtaque = new bool[4], skillsDefesa = new bool[4],
	skillsMagia = new bool[4], skillsVelocidade = new bool[4];

	ScriptableObject[] saveHabilidades = new ScriptableObject[4];  

	void Start () {
		//Set Variables
		_moedas = DataManager.dinheiro + _moedas;
		_potencia = DataManager.potencia; 

		quantBuff = DataManager.pocaoBuff; 
		quantCDR = DataManager.pocaoCDR; 
		quantCura = DataManager.pocaoCura;

		ataque = DataManager.ataque; 
		defesa = DataManager.defesa; 
		magia = DataManager.magia; 
		velocidade = DataManager.velocidade; 

		elmo = DataManager.elmo; 
		armadura = DataManager.armadura; 
		arma = DataManager.arma; 
		botas = DataManager.botas;

		for (int i = 0; i < skillsAtaque.Length; i++) {
			skillsAtaque[i] = DataManager.arvoreAtaque[i];
		 	skillsDefesa[i] = DataManager.arvoreDefesa[i];
			skillsMagia[i] = DataManager.arvoreMagia[i];
			skillsVelocidade[i] = DataManager.arvoreVelocidade[i];
		}

		//Set Textos
		soulcoins.text = "Soul Coins\n"+ _moedas;	

		txtPotencia.text = _potencia.ToString();

		potionsQuant[0].text = quantBuff.ToString();
		potionsQuant[1].text = quantCDR.ToString();
		potionsQuant[2].text = quantCura.ToString();

//		atrLevel[0].text = ataque.ToString();
//		atrLevel[1].text = defesa.ToString();
//		atrLevel[2].text = magia.ToString();
//		atrLevel[3].text = velocidade.ToString();

		atrLevel[0].text = IntToRoman(ataque);
		atrLevel[1].text = IntToRoman(defesa);
		atrLevel[2].text = IntToRoman(magia);
		atrLevel[3].text = IntToRoman(velocidade);

		itemLevel[0].text = IntToRoman(arma);
		itemLevel[1].text = IntToRoman(armadura);
		itemLevel[2].text = IntToRoman(elmo);
		itemLevel[3].text = IntToRoman(botas);

		//Set Preços Iniciais
		potionPrice.text = "" + _potencia * 100;
		potenciaPrice.text = "" + _potencia * 1000;

		atrPrice[0].text = "" + ataque * 100;
		atrPrice[1].text = "" + defesa * 100;
		atrPrice[2].text = "" + magia * 100;
		atrPrice[3].text = "" + velocidade * 100;

		itemPrice[0].text = "" + arma * 100;
		itemPrice[1].text = "" + armadura * 100;
		itemPrice[2].text = "" + elmo * 100;
		itemPrice[3].text = "" + botas * 100;

		//Set Skills
		for (int i = 0; i < skillsAtaque.Length; i++) {
			skillsAtaque[i] = DataManager.arvoreAtaque[i]; 
			skillsDefesa[i] = DataManager.arvoreDefesa[i];
			skillsMagia[i] = DataManager.arvoreMagia[i];
			skillsVelocidade[i] = DataManager.arvoreVelocidade[i];
		}



		CheckAvaliableHabilidades();

		//Set Selects
		for (int i = 0; i < currentHab.Length; i++) {			
			currentHab [i] = DataManager.skillSelectNum [i]; 
		}

		for (int i = 0; i < showingSelectedSkill.Length; i++) { //Funciona Diferente
			
			if (currentHab[i] > 0)
				showingSelectedSkill[i].sprite = skillIcons[currentHab[i]];
			else 
				showingSelectedSkill[i].sprite = skillIcons[16];
		}


		if (currentHab[3] == 1)
			darkEquips[0].SetActive(false);
		else if (currentHab[3] == 2)
			darkEquips[1].SetActive(false);
		else if (currentHab[3] == 3)
			darkEquips[2].SetActive(false);

	
	}

	void CheckAvaliableHabilidades(){


		for (int i = 0; i < 4; i++) {

			selectorAtaque[i].interactable  =  skillsAtaque[i];
			selectorDefesa[i].interactable  = skillsDefesa[i];
			selectorMagia[i].interactable  = skillsMagia[i];
			selectorVelocidade[i].interactable  = skillsVelocidade[i];


			if( ataque >= LevelRequired[i])
			darkAtaque[i].interactable  = !skillsAtaque[i];

			if( defesa >= LevelRequired[i])
			darkDefesa[i].interactable  = !skillsDefesa[i];

			if( magia >= LevelRequired[i])
			darkMagia[i].interactable  = !skillsMagia[i];

			if( velocidade >= LevelRequired[i])
			darkVelocidade[i].interactable  = !skillsVelocidade[i];
		}

	}
		

	void OnBuySomething(int compra){		
		_moedas -= compra;
		soulcoins.text = "Soul Coins\n"+ _moedas;		
	}

	public void ShopExit(){

		DataManager.dinheiro = _moedas;

		DataManager.potencia = _potencia; 
		DataManager.pocaoBuff = quantBuff; 
		DataManager.pocaoCDR = quantCDR; 
		DataManager.pocaoCura = quantCura;


		SetScriptableSkillSelected ();
		DataManager.currentKightColor = Color_Knight();

		for (int i = 0; i < currentHab.Length; i++) {			
			DataManager.skillSelectNum [i] = currentHab [i];
			DataManager.skillSelect [i] = saveHabilidades [i]; 
		}

		DataManager.ataque = ataque; 
		DataManager.defesa = defesa; 
		DataManager.magia = magia; 
		DataManager.velocidade = velocidade;

		DataManager.elmo = elmo; 
		DataManager.armadura = armadura; 
		DataManager.arma = arma; 
		DataManager.botas = botas;

		for (int i = 0; i < skillsAtaque.Length; i++) {
			DataManager.arvoreAtaque[i] = skillsAtaque[i];
			DataManager.arvoreDefesa[i] = skillsDefesa[i];
			DataManager.arvoreMagia[i] = skillsMagia[i];
			DataManager.arvoreVelocidade[i] = skillsVelocidade[i];
		}

		dm.Save();
	}

	RuntimeAnimatorController Color_Knight(){
		int[] colorLeveis = new int[] { 10, 25, 35, 50, 60, 75, 85, 100 };
		int soma = elmo + armadura + arma + botas;
		int current = 0;

		if (soma < colorLeveis[0])
			current = 0;

		for (int i = 1; i < colorLeveis.Length - 1; i++) {
				
			if (soma < colorLeveis[i] && soma > colorLeveis[i - 1]){
					current = i;
			}
		}
		
		if (soma > colorLeveis[colorLeveis.Length - 1])
			current = colorLeveis.Length - 1;

		return cavaleiroCores[current];
	}

	void SetScriptableSkillSelected (){
		
		for (int i = 0; i < saveHabilidades.Length - 1; i++) {
			
			if (currentHab[i] >= 0)
				saveHabilidades[i] = skillsData[currentHab[i]];
			else 
				saveHabilidades[i] = skillsData[16];
		}

		if (currentHab[3] >= 0)
			saveHabilidades[3] = potionsData[currentHab[3]];
		else 
			saveHabilidades[3] = potionsData[3];

	}

	// Seletor - Inicio
	public void GetCurrentHabilidade(int habilidadeNum){ //0 ao 15
		if (habilidadeNum != currentHab[0] && habilidadeNum != currentHab[1] && habilidadeNum != currentHab[2]){
			if (currentHab[0] < 0){
				currentHab[0] = habilidadeNum;
				showingSelectedSkill[0].sprite = skillIcons[habilidadeNum];
			}
			else if (currentHab[1] < 0){			
				currentHab[1] = habilidadeNum;
				showingSelectedSkill[1].sprite = skillIcons[habilidadeNum];
			}
			else if (currentHab[2] < 0){
				currentHab[2] = habilidadeNum;
				showingSelectedSkill[2].sprite = skillIcons[habilidadeNum];
			}
			else {
				currentHab[2] = currentHab[1];
				showingSelectedSkill[2].sprite = showingSelectedSkill[1].sprite;
				currentHab[1] = currentHab[0];
				showingSelectedSkill[1].sprite = showingSelectedSkill[0].sprite;
				currentHab[0] = habilidadeNum;
				showingSelectedSkill[0].sprite = skillIcons[habilidadeNum];
			}
		}
	}
	// Seletor - Final


	//Haibilidades - Inicio
	public void BuyAtributo(int atributo){	//1 Ataque | 2 Defesa | 3 Magia | 4 Velocidade
		int atkPrice = ataque * 100, defPrice = defesa * 100; 
		int magPrice = magia * 100,  velPrice = velocidade * 100;

		if (atributo == 0){
		if (_moedas >= atkPrice) {
			ataque += 1;
			OnBuySomething (atkPrice);
			atrPrice[0].text = (ataque * 100).ToString();
			atrLevel[0].text = IntToRoman(ataque);
			}}

		else if (atributo == 1){
		 if (_moedas >= defPrice) {
			defesa += 1;
			OnBuySomething (defPrice);
			atrPrice[1].text = (defesa * 100).ToString();
			atrLevel[1].text = IntToRoman(defesa);
			}}

		else if (atributo == 2){
		if (_moedas >= magPrice) {
			magia += 1;
			OnBuySomething (magPrice);
			atrPrice[2].text = (magia * 100).ToString();
			atrLevel[2].text = IntToRoman(magia);
			}}

		else if (atributo == 3){
		if (_moedas >= velPrice) {
			velocidade += 1;
			OnBuySomething (velPrice);
			atrPrice[3].text = (velocidade * 100).ToString();
			atrLevel[3].text = IntToRoman(velocidade);
			}}

		CheckAvaliableHabilidades();
	}

	public void BuyHabilidadeAtaque(int skillNum){ //0 a 3

		for(int i = 0; i <= skillsAtaque.Length; i++)
			if (skillNum == i)
				if (_moedas >= skillPrices[i] && ataque >= LevelRequired[i]) {
					skillsAtaque[i] = true;
					OnBuySomething(skillPrices[i]);
					CheckAvaliableHabilidades();
				}
	}

	public void BuyHabilidadeDefesa(int skillNum){ //0 a 3

		for(int i = 0; i <= skillsDefesa.Length; i++)
			if (skillNum == i)
				if (_moedas >= skillPrices[i] && defesa >= LevelRequired[i]) {
					skillsDefesa[i] = true;
					OnBuySomething(skillPrices[i]);
					CheckAvaliableHabilidades();
				}
	}

	public void BuyHabilidadeMagia(int skillNum){ //0 a 3

		for(int i = 0; i <= skillsMagia.Length; i++)
			if (skillNum == i)
				if (_moedas >= skillPrices[i] && magia >= LevelRequired[i]) {
					skillsMagia[i] = true;
					OnBuySomething(skillPrices[i]);
					CheckAvaliableHabilidades();
				}
	}

	public void BuyHabilidadeVelocidade(int skillNum){ //0 a 3
		
		for(int i = 0; i <= skillsVelocidade.Length; i++)
			if (skillNum == i){
				if (_moedas >= skillPrices[i] && velocidade >= LevelRequired[i]) {
					skillsVelocidade[i] = true;
					OnBuySomething(skillPrices[i]);
					CheckAvaliableHabilidades();
				}
			}	
	}
	//Haibilidades - Final


	//Equipamentos - Inicio
	public void OnBuyItem(int item){
		int elmoPrice = elmo * 100, armaduraPrice = armadura * 100; 
		int armaPrice = arma * 100,  botasPrice = botas * 100;

		if (item == 0){
		if (_moedas >= armaPrice) {
			arma += 1;
			OnBuySomething (armaPrice);
			itemPrice[0].text = (arma * 100).ToString();
			itemLevel[0].text = IntToRoman(arma);
			}}

		else if (item == 1){
		if (_moedas >= armaduraPrice) {
			armadura += 1;
			OnBuySomething (armaduraPrice);
			itemPrice[1].text = (armadura * 100).ToString();
			itemLevel[1].text = IntToRoman(armadura);
			}}

		else if (item == 2){
		if (_moedas >= elmoPrice) {
			elmo += 1;
			OnBuySomething (elmoPrice);
			itemPrice[2].text = (elmo * 100).ToString();
			itemLevel[2].text = IntToRoman(elmo);
			}}

		else if (item == 3){
		if (_moedas >= botasPrice) {
			botas += 1;
			OnBuySomething (botasPrice);
			itemPrice[3].text = (botas * 100).ToString();
			itemLevel[3].text = IntToRoman(botas);
			}}

	}
	//Equipamentos - Final


	//Potion - Inicio
	public void GetCurrentePotion(int pote){
		currentHab[3] = pote;
	}

	public void BuyPotion(int tipo){ //1-Buff | 2-CDR | 3-Cura
		int price = _potencia * 100;

		if(_moedas >= price){				
				if (tipo == 1 && quantBuff < 5){
					quantBuff += 1;
					potionsQuant[0].text = quantBuff.ToString();
					OnBuySomething(price);
				}
				else if (tipo == 2 && quantCDR < 5){
					quantCDR += 1;
					potionsQuant[1].text = quantCDR.ToString();
					OnBuySomething(price);
				}
				else  if (tipo ==3 && quantCura < 5){
					quantCura += 1;
					potionsQuant[2].text = quantCura.ToString();
					OnBuySomething(price);
				}
		}
	}

	public void UpPotencia(){
		int price = _potencia * 1000;

		if (_moedas >= price && _potencia < 9){
			//Recupera o dinheiro gasto nas poções
			Potion_Reset();

			_potencia += 1;
			txtPotencia.text = _potencia.ToString();
			potionPrice.text = "" + _potencia * 100;
			OnBuySomething(price);
			potenciaPrice.text = "" + _potencia * 1000;
		}			
	}

	void Potion_Reset(){
		int devolucaoPotionsMoney = (quantBuff  + quantCDR  + quantCura) * (_potencia * 100);

		quantBuff = 0;
		quantCDR = 0;
		quantCura = 0;

		potionsQuant[0].text = quantBuff.ToString();
		potionsQuant[1].text = quantCDR.ToString();
		potionsQuant[2].text = quantCura.ToString();

		_moedas += devolucaoPotionsMoney;
	}
	//Potion - Final


	//Funções e Retuns não únicos
	string IntToRoman(int number)
	{
		if (number >= 4000 || number == 0) { return "0"; }			

		string[] roman = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
		int[] decimals = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

		string romanvalue = string.Empty;

		for (int i = 0; i < decimals.Length; i++) 
		{
			while (decimals[i] <= number)
			{
				number -= decimals[i];
				romanvalue += roman[i];
			}
		}

		return romanvalue.ToString();
	}
}
