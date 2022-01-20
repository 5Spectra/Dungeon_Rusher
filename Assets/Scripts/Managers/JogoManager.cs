using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JogoManager : MonoBehaviour {

	// Objetos
	public DataManager dm;
	public audioManager am;

	public GameObject menuPause, menuGameOver;

	public TMP_Text estat1, estat2, estat3;
	public TMP_Text moedaShow;

	public AudioClip pauseSound;

	public GameObject[] Fases = new GameObject[7];

	// Coisas para serem usaddas
	int moedas;
	[HideInInspector] public bool isPlayerDead, isGamePaused;

	int FaseAtual = 0; //0-Cidade 1-Campo 2-Floresta 3-Deserto 4-FlorestaPino 5-Gelo 6-DarkWood

	// Salvar "game" estatisticas
	float timePlayed, disPercorrida;
	int dCausado, dRecebido, conUsados, iniMortos, bossMortos;


	void Update () {

		if (isPlayerDead == false)
			timePlayed += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Escape)){
			if (isGamePaused == true)
				Resume();
			else
				Pause();
		}

	}

	public void active_NextPlace (){
		FaseAtual++;
		Fases[FaseAtual].SetActive(true);
	}

	public void GetMoney(int soulCoin){
		moedas += soulCoin;
		moedaShow.text = moedas.ToString();
	}
	public void GetDanoRecebidoCausado (int recebido, int causado){
		dRecebido += recebido;
		dCausado += causado;
	}
	public void GetDistancia (int percorrido){
		disPercorrida = percorrido;
	}
	public void GetConsumiveisUsados(){ //ultra hard
		conUsados += 0;
	}

	public void GetNormalKills(){
		iniMortos += 1;
	}
	public void GetBossKills(){
		bossMortos += 1;
	}

	public void Resume(){
		isGamePaused = false;
		Time.timeScale = 1f;
		menuPause.SetActive (false);

		am.Play_SFX(pauseSound);
	}

	public void Pause(){
		isGamePaused = true;
		Time.timeScale = 0f;
		menuPause.SetActive (true);

		am.Play_SFX(pauseSound);
	}

	public void Troca_cena (string cena){
		Time.timeScale = 1f;
		UnityEngine.SceneManagement.SceneManager.LoadScene (cena);
	}

	public void endGame(){

		menuGameOver.SetActive(true);

		estat1.text = moedas.ToString();
		estat2.text = (iniMortos + bossMortos).ToString();
		estat3.text = disPercorrida.ToString();

		DataManager.dinheiro += moedas;

		List<int> array = new List<int>();

		array.Add(Mathf.RoundToInt(timePlayed)); //DataManager.tempoJogado += timePlayed;

		array.Add(dCausado); //DataManager.danoCausado += 0;
		array.Add(dRecebido); //DataManager.danoRecebido += 0;

		array.Add(moedas); //DataManager.dinheiroAcumulado += 0;
		array.Add(conUsados); //DataManager.consumiveisUsados += 0; 

		array.Add(iniMortos); //DataManager.inimigosMortos += 0;
		array.Add(bossMortos); //DataManager.bossesMortos += 0;

		//não é nessario usar a mesma variavel duas vezes
		array.Add(Mathf.RoundToInt(disPercorrida)); //DataManager.distanciaPercorrida += 0;
		//não é nessario checar a mesma coisa duas vezes.
		//array.Add(Mathf.RoundToInt(disPercorrida)); //DataManager.maxDistacia = 0;

		dm.SetGameEstatistica(array.ToArray());
	}
}
