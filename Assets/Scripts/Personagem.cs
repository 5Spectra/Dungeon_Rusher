using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Personagem : MonoBehaviour {

	// Config
	public JogoManager manager;

	public AudioSource audioSourceSFX;
	public Animator anim;
	public Rigidbody2D rb2D;
	public Collider2D collComponent;

	KeyCode[] keysSkill = new KeyCode[] {KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4};

	public TMP_Text distanciaPerText;
	public TMP_Text hpLabel;
	public Slider hpShow;
	public GameObject dieEfeito;

	public AudioClip colisionClip;
	public AudioClip[] skillClips = new AudioClip[4]; //potion = 3;
	public Image[] HabilidadesIcons = new Image[4]; //potion = 3;
	public GameObject[] habEfects = new  GameObject[4]; //potion = 3;

	public Image[] darkMask = new Image[4]; //potion = 3;
	public TMP_Text[] coolDownText = new TMP_Text[4]; //potion = 3;
	public Button[] skillButton = new Button[4]; //potion = 3;

	public TMP_Text poteQuantidadeTexto;

	//Private Variaveis
	float[] coolDownDuration = new float[4];
	float[] coolDownTimeLeft = new float[4];

	int potePotencia;
	int[] poteQuantidade = new int[3];

	[HideInInspector] public bool isKnockBack = false;
	bool isDying = false;
	float disPercorida;
	GameObject skillEfect;
	int maxHP;
	float skillDuration;

	// Game Base Atr
	int baseDano, baseVida, baseMagic, baseVelocidade, baseDefensa; //baseDefense = Reducao de dano
	[HideInInspector] public bool isVampire;
	[HideInInspector] public int moneyValue;
	//Game Atr
	public int h_Dano, h_Vida, h_RD, h_magia;
	[SerializeField] int h_velocidade;

	//Habilidades Get
	Habilidades[] skillBar = new Habilidades[4];

	string[] currentHabilidade = new string[4]; //[0-2] = abliadade123 | [3] = potes

	void Start () {
		moneyValue = 1;
			
		//Pegando Todas as variaveis sobre Poçoes 
		poteQuantidade[0] = DataManager.pocaoBuff;
		poteQuantidade[1] = DataManager.pocaoCDR;
		poteQuantidade[2] = DataManager.pocaoCura;
		potePotencia = DataManager.potencia;

		//Pegando Skills
		skillBar[0] =(Habilidades) DataManager.skillSelect[0]; //abilidade 1
		skillBar[1] =(Habilidades) DataManager.skillSelect[1]; //abilidade 2
		skillBar[2] =(Habilidades) DataManager.skillSelect[2]; //abilidade 3
		skillBar[3] =(Habilidades) DataManager.skillSelect[3]; //pote

		Initialize(0); Initialize(1); Initialize(2); Initialize(3);

		//Setando a Cor
		anim.runtimeAnimatorController = DataManager.currentKightColor;

		//Vendo se é vampiro
		isVampire = DataManager.Vampire_check();
			
		//Setando as variaveis publicas
		baseDano = DataManager.ataque;
		baseVida = DataManager.defesa;
		baseDefensa = DataManager.defesa;
		baseMagic = DataManager.magia;
		baseVelocidade = DataManager.velocidade;

		//Setando a base de dano ao calcular tudo
		baseDano = Mathf.RoundToInt (baseDano + (baseDano * (DataManager.arma * .25f)));

		baseVida =  Mathf.RoundToInt (10 + baseVida + (baseVida * DataManager.armadura * .75f)); //Vida

		//baseDefensa = Mathf.RoundToInt( ((baseVelocidade + baseDefensa) * 0.25f) + (DataManager.armadura * .025f )); //RD

		baseMagic = Mathf.RoundToInt (baseMagic + (baseMagic * (DataManager.elmo * .25f)));

		baseVelocidade = Mathf.RoundToInt (baseVelocidade + (baseVelocidade * (DataManager.botas* .25f )));

		// h_ = heroi, seta os status iniciais
		h_Dano = baseDano;
		h_Vida = baseVida;
		//h_RD = baseDefensa;
		h_magia = Mathf.RoundToInt ((baseMagic * 0.25f) + 1);
		h_velocidade = baseVelocidade;

		hpShow.maxValue = h_Vida;
		maxHP  = h_Vida;

		skillDuration = h_magia / h_velocidade;
	}

	void OnCollisionEnter2D (Collision2D coll){		
		audioSourceSFX.clip = colisionClip;
		audioSourceSFX.Play ();
	}

	void Update () { 

		hpShow.value = h_Vida;
		hpLabel.text = h_Vida.ToString();

		if (isKnockBack == false && isDying == false){
			disPercorida += h_velocidade * Time.deltaTime;
			distanciaPerText.text = Mathf.Round (disPercorida).ToString();
		}

		if (h_Vida <= 0){
			isDying = true;
			dying();
		}

		bool[] CoolDownComplete = new bool[4];

		for (int i = 0; i < CoolDownComplete.Length; i++) {

			CoolDownComplete [i] = (coolDownTimeLeft [i] < 0);

			if (CoolDownComplete [i] == true) {
				AbilityReady (i);
				if (Input.GetKeyDown (keysSkill [i])) {
					OnSkill (i);
				}

			} else {
				CoolDown (i);
			}
		}
	
	}

	void FixedUpdate(){
		
		if (isKnockBack == false && isDying == false)
			rb2D.velocity = Vector3.right * h_velocidade;
		else if (isDying == true){
			rb2D.velocity = Vector3.zero;
		}
	}

	public void Reset_Damage(){
		Vampire();
		h_Dano = baseDano;
	}

	public void Repel(){ //joga o heroi para trás, por não ter dano para dar IK
		StartCoroutine(repeling());
	}
	IEnumerator repeling (){
		float impulso = h_velocidade - (h_velocidade * 0.75f) ;

		isKnockBack = true;
		rb2D.AddForce (Vector3.left * impulso, ForceMode2D.Impulse);

		yield return new WaitForSeconds (.1f); //impulso * .005f);

		rb2D.AddForce (Vector3.right * impulso, ForceMode2D.Impulse);
		isKnockBack = false;
	}



	void dying (){
		manager.GetDistancia(Mathf.RoundToInt(disPercorida));
		manager.isPlayerDead = true;
		manager.endGame();

		DataManager.pocaoBuff = poteQuantidade[0];
		DataManager.pocaoCDR = poteQuantidade[1];
		DataManager.pocaoCura = poteQuantidade[2];

		GetComponent<SpriteRenderer>().enabled = false;

		Instantiate(dieEfeito, gameObject.transform);//spawn um efeito

		GetComponent<Personagem>().enabled = false;
	}

	//Skill - Vampirismo =====================================
	void Vampire(){
		if (isVampire == true)
			h_Vida += Mathf.RoundToInt(h_Dano * 0.1f);
	}

	//Skills - Arvore de Ataque =====================================
	void Twice_Ataque(){
		h_Dano += baseDano * 2;
	}

	void Lancada(){
		h_Dano += 50;
	}

	void Empalar(){
		h_Dano += 2 * baseDano + 10;
	}

	void SuperAtaque(){
		h_Dano += 4 * baseDano;
	}

	//Skills - Arvore de Defsa =====================================
	void basicDefense(){ StopCoroutine(basicDefense_Skill()); StartCoroutine (basicDefense_Skill()); }
	IEnumerator basicDefense_Skill(){		
		h_RD += 2;
		yield return new WaitForSeconds(skillDuration);
		h_RD -= 2;
	}
	void fixedRedution(){ StopCoroutine(fixedRedution_Skill()); StartCoroutine (fixedRedution_Skill()); }
	IEnumerator fixedRedution_Skill(){
		h_RD += 10;
		yield return new WaitForSeconds(skillDuration);
		h_RD -= 10;
	}
	void ironSkin(){ StopCoroutine(ironSkin_Skill()); StartCoroutine (ironSkin_Skill()); }
	IEnumerator ironSkin_Skill(){
		h_RD += 2 * baseDefensa;
		yield return new WaitForSeconds(skillDuration);
		h_RD -= 2 * baseDefensa;
	}
	void Invencibilidade(){ StopCoroutine(Invencibilidade_Skill()); StartCoroutine (Invencibilidade_Skill()); }
	IEnumerator Invencibilidade_Skill(){
		h_RD += 1000000;
		yield return new WaitForSeconds(skillDuration);
		h_RD -= 1000000;
	}

	//Skills - Arvore de Magia =====================================

	void Twice_Gold(){ StopCoroutine(Twice_Gold_Skill()); StartCoroutine (Twice_Gold_Skill()); }
	IEnumerator Twice_Gold_Skill(){
		moneyValue = 2;
		yield return new WaitForSeconds(skillDuration);
		moneyValue = 1;
	}

	void Cura(){
		h_Vida += 50;

		if (h_Vida > maxHP)
			h_Vida = maxHP;
	}

	void Trinity(){
		h_Dano += baseDano + baseDefensa + baseMagic + Mathf.RoundToInt(baseVelocidade);
	}

	void Cura_Maior(){
		h_Vida += 10 * baseMagic;

		if (h_Vida > maxHP)
			h_Vida = maxHP;
	}

	//Skills - Arvore de Velocidade  =====================================
	void Twice_Velocidade(){ StopCoroutine(Twice_Velocidade_Skill()); StartCoroutine (Twice_Velocidade_Skill()); }
	IEnumerator Twice_Velocidade_Skill(){
		h_velocidade += baseVelocidade * 2;
		yield return new WaitForSeconds(skillDuration);
		h_velocidade -= baseVelocidade * 2;
	}

	void Ignorar(){ StopCoroutine(Ignorar_Skill()); StartCoroutine (Ignorar_Skill()); }
	IEnumerator Ignorar_Skill(){
		collComponent.enabled = false;
		yield return new WaitForSeconds(skillDuration);
		collComponent.enabled = true;
	}

	void fastAttacks(){
		h_Dano += 3 * baseVelocidade;
	}

	void speedBurst(){ StopCoroutine(speedBurst_Skill()); StartCoroutine (speedBurst_Skill()); }
	IEnumerator speedBurst_Skill(){
		h_velocidade += baseVelocidade; 
		yield return new WaitForSeconds(skillDuration);
		h_velocidade = baseVelocidade;
	}

	//Poções 
	void BuffPotion (){
		if (poteQuantidade[0] > 0) {

			baseDano += potePotencia;
			baseDefensa += potePotencia;
			baseMagic += potePotencia;
			baseVelocidade += potePotencia;

			poteQuantidade[0] -= 1;
			poteQuantidadeTexto.text = poteQuantidade[0].ToString();
			ButtonTriggered (3);
		}
		else
			skillButton[3].interactable = false;
	}

	void CDRpotion (){
		if (poteQuantidade [1] > 0) {
			coolDownTimeLeft [0] -= potePotencia;
			coolDownTimeLeft [1] -= potePotencia;
			coolDownTimeLeft [2] -= potePotencia;

			poteQuantidade [1] -= 1;
			poteQuantidadeTexto.text = poteQuantidade[1].ToString();
			ButtonTriggered (3);
		}
		else
			skillButton[3].interactable = false;

	}

	void CuraPotion (){
		if (poteQuantidade [2] > 0) {
			h_Vida += 10 * potePotencia;
			if (h_Vida > maxHP)
				h_Vida = maxHP;

			poteQuantidade [2] -= 1;
			poteQuantidadeTexto.text = poteQuantidade[1].ToString();
			ButtonTriggered (3);
		}
		else
			skillButton[3].interactable = false;
	}

	// Scripts de uso ---------------------------------------------------------------

	public void OnSkill(int skillSelectNumber){ //0-2

		if (currentHabilidade[skillSelectNumber] != "Nulo") {
			Invoke (currentHabilidade [skillSelectNumber],0);
			ButtonTriggered (skillSelectNumber);
		}
	}

	public void OnPoteUse(){

		if (currentHabilidade[3] != "Nulo") {
			Invoke (currentHabilidade [3],0);
		}
	}

	void Initialize(int Number){

		skillClips[Number] = skillBar[Number].aSound;

		HabilidadesIcons[Number].sprite = skillBar[Number].aSprite;

		habEfects[Number] = skillBar[Number].aEfeito;

		coolDownDuration[Number] = skillBar[Number].aBaseCoolDown;

		currentHabilidade [Number] = skillBar[Number].aName;

		if (Number == 3){
			int poteNum = 0;
			switch(currentHabilidade[3]){
			case "BuffPotion": poteNum = 0; break;
			case "CDRpotion": poteNum = 1; break;
			case "CuraPotion": poteNum = 2; break;
			}

			poteQuantidadeTexto.text = poteQuantidade[poteNum].ToString();
		}

		AbilityReady(Number);
	}
		

	void AbilityReady(int inteiro)
	{
		coolDownText[inteiro].enabled = false;
		darkMask[inteiro].enabled = false;
		skillButton [inteiro].interactable = true;
	}

	void CoolDown(int inteiro)
	{
		coolDownTimeLeft[inteiro] -= Time.deltaTime * h_magia;
		coolDownText[inteiro].text = Mathf.Round (coolDownTimeLeft[inteiro]).ToString ();
		darkMask[inteiro].fillAmount = (coolDownTimeLeft[inteiro] / coolDownDuration[inteiro]);
	}

	void ButtonTriggered(int inteiro)
	{
		coolDownTimeLeft[inteiro] = coolDownDuration[inteiro];

		darkMask[inteiro].enabled = true;
		coolDownText[inteiro].enabled = true;
		skillButton [inteiro].interactable = false;

		audioSourceSFX.clip = skillClips[inteiro];
		audioSourceSFX.Play ();

		skillEfect = Instantiate(habEfects[inteiro], transform);
		Destroy (skillEfect, skillDuration);
	}
}