using System;
using UnityEngine;


[Serializable] //Repare que não é SerializeField, sim é importante
public class DataManager {

	//Player - Item e Upgrades
	public static int dinheiro = 300;

	public static int ataque = 1, defesa = 1, magia = 1, velocidade = 2;

	public static int arma = 1, armadura = 1, elmo = 1, botas = 1;

	public static int pocaoBuff, pocaoCDR, pocaoCura, potencia = 1;

	public static bool[] arvoreAtaque = new bool[4], arvoreDefesa = new bool[4],
	arvoreMagia = new bool[4], arvoreVelocidade = new bool[4];

	public static ScriptableObject[] skillSelect = new ScriptableObject[4]; //3 = pote

	public static int[] skillSelectNum = new int[4] {-1,-1,-1,-1};
	//public static SkillData skillSelect1, skillSelect2, skillSelect3;
	//public static PotionData consumivelSelect;
	//Data
	public static RuntimeAnimatorController currentKightColor;

	//Config
	public static bool fullscreen = true;
	public static float musica, sfx;
	public static int grafico = 1; //0 = low | 1 = medio | 2 = alto
	public static int linguagem; //0 - Portugues | 1 = English
	public static Resolution resolucao;

	//Estatisiticas -não resetaveis-
	public static float tempoJogado, danoCausado, danoRecebido;
	public static int  dinheiroAcumulado, consumiveisUsados, upgradeComprados, inimigosMortos, bossesMortos, distanciaPercorrida, maxDistancia;

	//Game - Historia e Tutorial vistos e Endless Unlock;
	public static bool gameHistoria, gameComplete, gameTutorial;


	public void Game_Reset(){
		dinheiro = 0;

		ataque = 1; defesa = 1; magia = 1; velocidade = 1;
		arma = 1; armadura = 1; elmo = 1; botas = 1;

		pocaoBuff = 0;
		pocaoCDR = 0; 
		pocaoCura = 0; 
		potencia = 1;

		for (int i = 0; i <  arvoreAtaque.Length; i++) {
			arvoreAtaque[i] = false; arvoreDefesa[i] = false; arvoreMagia[i] = false; arvoreVelocidade[i] = false;
		}

		Save();
	}

	public string[] Estatistica_String(){

		string[] array_estatisticas = new string[10];
		
		int arvores_int = Check_Arvores(arvoreAtaque) + Check_Arvores(arvoreDefesa) + Check_Arvores(arvoreMagia) + Check_Arvores(arvoreVelocidade);

		upgradeComprados = ataque + defesa + magia + arma + armadura + elmo + botas + velocidade + arvores_int;

		array_estatisticas[0] = (tempoJogado/60).ToString();
		array_estatisticas[1] = consumiveisUsados.ToString();
		array_estatisticas[2] = inimigosMortos.ToString();
		array_estatisticas[3] = distanciaPercorrida.ToString();
		array_estatisticas[4] = danoCausado.ToString();
		array_estatisticas[5] = dinheiroAcumulado.ToString();
		array_estatisticas[6] = upgradeComprados.ToString();
		array_estatisticas[7] = bossesMortos.ToString();
		array_estatisticas[8] = maxDistancia.ToString();
		array_estatisticas[9] = danoRecebido.ToString();

		return array_estatisticas;
	}

	int Check_Arvores(bool[] arvores){ //utilizado na função Estatistica_ToString
		int num = 0;
		for (int i = 0; i < arvores.Length; i++){
			if (arvores[i] == true)
				num += 1;
		}
		return num;
	}

	public static bool Vampire_check(){
		DataManager check =  new DataManager();

		int arvores_int = check.Check_Arvores(arvoreAtaque) + check.Check_Arvores(arvoreDefesa) + check.Check_Arvores(arvoreMagia) + check.Check_Arvores(arvoreVelocidade);

		if (arvores_int == 16)
			return true;
		else 
			return false;
	}


	public void SetGameEstatistica(int[] estatus) {

		tempoJogado += estatus[0];

		danoCausado += estatus[1];
		danoRecebido += estatus[2];

		dinheiroAcumulado += estatus[3];
		consumiveisUsados += estatus[4]; 

		inimigosMortos += estatus[5];
		bossesMortos +=estatus[6];

		distanciaPercorrida += estatus[7]; 

		if (maxDistancia < estatus[7])
			maxDistancia = estatus[7];

		Save();
	}


	public void Change_Language(int index){
		


	}

	public void Save(){


	}

	public void Load(){
		
	}

}
