using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour {

	public EnemyData[] areaInimigos;

	public GameObject enemyPrefab;

	[Tooltip("Numero da area: 1Cidade, 2Campo, 3Floresta, 4Deserto, 5FlorestaPino,6Gelo,7DarkWood")]
	public int areaNumber;

	void Start () {


		int inimigoEscolhido = Random.Range(0 , areaInimigos.Length);

		GameObject newInimigo = Instantiate(enemyPrefab, transform.position , Quaternion.identity, transform.parent);

		newInimigo.GetComponent<Enemy>().Edata = areaInimigos[inimigoEscolhido];

		newInimigo.SetActive (true);

		Destroy(gameObject);
	}
}
