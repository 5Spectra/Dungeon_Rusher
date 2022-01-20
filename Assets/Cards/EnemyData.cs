using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy Card")]
public class EnemyData : ScriptableObject {

	//public string m_name;

	public Sprite sprite;

	public GameObject dieEffect;

	public int Dano, Vida;

	public int Dinheiro;

	public bool isBoss;



}
