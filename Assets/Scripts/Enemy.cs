using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour {

	// Config
	public EnemyData Edata;

	JogoManager manager;
	Personagem script;

	//Show
	public Slider vidaShow;
	public TMP_Text _Enome;

	public SpriteRenderer esprite;

	// Game
	int e_Dano, e_Vida;

	void Start () {

		_Enome.text = Edata.name;
		esprite.sprite = Edata.sprite;

		if (Edata.isBoss == true)
		_Enome.color = Color.red;

		script = GameObject.FindGameObjectWithTag("Player").GetComponent<Personagem>();

		manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<JogoManager>();

		e_Dano = Edata.Dano;
		e_Vida = Edata.Vida;

		vidaShow.maxValue = e_Vida;
		vidaShow.value = e_Vida;
	}

	void OnCollisionEnter2D (Collision2D coll){		

			if (script.h_RD <= e_Dano)
			script.h_Vida -= e_Dano- script.h_RD;

			if (script.h_Dano < e_Vida)
				script.Repel();
			else
				OnDiying();

			e_Vida -= script.h_Dano;

			vidaShow.value = e_Vida;


			script.Reset_Damage();

			manager.GetDanoRecebidoCausado(script.h_Dano, e_Dano);
	}

	void OnDiying(){ //Colocar um efeito especial para quando morre
		manager.GetMoney(Edata.Dinheiro * script.moneyValue);

		if (Edata.isBoss == true){
			manager.GetBossKills();
			manager.active_NextPlace();
		}
		else
			manager.GetNormalKills();

		Instantiate(Edata.dieEffect, gameObject.transform.position, Quaternion.identity);//spawna o efeito

		Destroy(gameObject);
	}
}
