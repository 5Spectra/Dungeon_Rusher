using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://unity3d.com/pt/learn/tutorials/topics/scripting/ability-system-scriptable-objects")]

[CreateAssetMenu(fileName = "New Habilidade", menuName = "Game/Ability Card")]
public class Habilidades : ScriptableObject {

	public string aName = "Nome_da_Skill";
	public Sprite aSprite;
	public AudioClip aSound;
	public GameObject aEfeito;
	public float aBaseCoolDown = 1f;

	//[Tooltip("0-15 skills e 0-2 pocoes || 16 = null e 3 = null")]
	//public int aNumber;

	//public abstract void Initialize(GameObject obj);
	//public abstract void TriggerAbility();

}
