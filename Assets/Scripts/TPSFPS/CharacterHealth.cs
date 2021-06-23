using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour {

	public float currentHealth;
	public float MaxHealth;
	public bool IsAlive = true;
	public Image HealthBarP;

	private CharacterControlV2 ccRef;

	void Start(){
		ccRef = GetComponent<CharacterControlV2> ();
		currentHealth = MaxHealth;
	}

	void Update () {
		if (currentHealth <= 0) {
			Die ();		
		}
	}

	void Die(){
		IsAlive = false;
		//ccRef.Death ();
	}

	public void Damage(float damageAmount){
		if (HealthBarP != null) {
			HealthBarP.fillAmount = currentHealth / 100;
			if (currentHealth < 80 && currentHealth > 50) {
				HealthBarP.color = Color.yellow;//by HD
			} else if (currentHealth < 50) {//by HD
				HealthBarP.color = Color.red;//by HD
			}
		}
		currentHealth -= damageAmount;
	}
}
