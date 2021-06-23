using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour {


	public enum WeaponType { PRIMARY , SECONDARY , GRENADE , KNIFE};
	public WeaponType mWeaponType;

	public float FireRange;
	public float FireRate;
	public float FireTimer;

	public float DamageAmount;

	public Vector3 FireOffset;

	private AudioSource mFireSSource;

	public AudioClip FireSound;
	public Transform FirePosition;
	public GameObject FirePrefab;

	public Transform MuzzleFlashPosition;
	public GameObject MuzzlePrefab;

	public CharacterControlV2 ccRef;
	
    //public AIEnemy mAIEnemy;

	public int flip = 1;
	float yOffset = 0.0f;

	//private Vector3 target;
	//private Transform source;

	GameObject bullet;

	void Start () {

		//Temporary Fix
		if(GetComponentInParent<CharacterControlV2>())
			ccRef = GetComponentInParent<CharacterControlV2> ();

		mFireSSource = GetComponent<AudioSource> ();
		mFireSSource.clip = FireSound;

		Debug.Log ("++++++" + transform.root.name);

		//if ((ccRef != null && ccRef.IsAIPlayer) || (GetComponentInParent<CharacterControlV2>() == null)) //Temporary fix for sniper
		//	mAIEnemy = GetComponentInParent<AIEnemy> ();
	}

//	void Update()
//	{
//		
//	}

	void FixedUpdate () {
		//if (ccRef.IsAIPlayer && ccRef.IsAttack) {
			//Debug.Log ("===AI Firing===0");

		if (/*mAIEnemy != null &&*/ ccRef.attack)
			Fire ();
        //}

        if (/*(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began && !ccRef.IsAIPlayer) ||*/ Input.GetKeyDown(KeyCode.RightControl) && !ccRef.IsAIPlayer)
        {
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            //if (Physics.Raycast(ray, out hit, 100))
            //{
            //    if (hit.collider.tag == "Enemy")
            //    {

            //    }
            //}



            //if (ccRef.mInputRight.GetTouchPosition.y < 0)
            //    yOffset = ccRef.AimOffset.y - 50.0f;
            //else if (ccRef.mInputRight.GetTouchPosition.y > 0)
            //    yOffset = ccRef.AimOffset.y + 50.0f;
            //else
            //    yOffset = 0.0f;


            Fire();
            //}
        }

        if (Input.GetKeyUp(KeyCode.RightControl)){
			//ccRef.attack = false;
		}

//		if(bullet != null)
//			bullet.transform.position = Vector3.MoveTowards (FirePosition.position, new Vector3(FireRange , FirePosition.position.y , FirePosition.position.z), 5.0f * Time.deltaTime);
		FireTimer += Time.deltaTime;
	}

	public void Fire(){
		
		if (FireTimer < FireRate)
			return;
		
		//RaycastHit hit;
		//ccRef.attack = true;
		//if(Physics.Raycast(FirePosition.position , FirePosition.right , FireRange)){
		//
		//Debug.Log ("===AI Firing===1");
		mFireSSource.PlayOneShot(FireSound);


		/*Commented for temporary reasons
		GameObject bullet = (GameObject)Instantiate (FirePrefab, FirePosition.position , FirePosition.rotation);

		if (mAIEnemy != null && mAIEnemy.Detected) {
			target = mAIEnemy.GetTarget ();
			source = transform.root;
		}

		if (target != null && source == null) {
			bullet.SendMessage ("SetTarget", target, SendMessageOptions.RequireReceiver);
			bullet.SendMessage ("SetSource", source, SendMessageOptions.RequireReceiver);
		}
		*/
		//Debug.DrawLine (FirePosition.position, new Vector3(FireRange , FirePosition.position.y , FirePosition.position.z) , Color.red);
		bullet = (GameObject)Instantiate (FirePrefab, FirePosition.position , FirePosition.rotation , FirePosition);
		//bullet.transform.eulerAngles = FireOffset;

		//if (ccRef.IsAIPlayer && mAIEnemy.Type == AIEnemy.EnemyType.ASSAULT) {
		//	Vector3 zScale = bullet.transform.localScale;
		//	zScale.z *= -1 * flip;
		//	bullet.transform.localScale = zScale;
		//	//bullet.transform.rotation = bullet.transform.rotation * Quaternion.Euler (ccRef.AimOffset * (-ccRef.mInputRight.GetTouchPosition.y));
		//}

		if (!ccRef.IsAIPlayer) {
			//Vector3 zScale = bullet.transform.localScale;
			//zScale.z *= 1 * flip;
			//bullet.transform.localScale = zScale;
			//bullet.transform.rotation = bullet.transform.rotation * Quaternion.Euler (ccRef.AimOffset * (-ccRef.mInputRight.GetTouchPosition.y));
		}

		if (MuzzleFlashPosition != null) {
			var muzzle = (GameObject)Instantiate (MuzzlePrefab, MuzzleFlashPosition.position, MuzzleFlashPosition.rotation , MuzzleFlashPosition);
			//muzzle.transform.eulerAngles = FireOffset;

			//if(!ccRef.IsAIPlayer)
				//muzzle.transform.rotation = muzzle.transform.rotation * Quaternion.Euler (ccRef.AimOffset * (-ccRef.mInputRight.GetTouchPosition.y));

			Destroy (muzzle, 0.25f);
		}

		RaycastHit hit;

        //		Debug.DrawLine (FirePosition.position, new Vector3(FireRange * flip , FirePosition.position.y , FirePosition.position.z) , Color.red);
        if (!ccRef.IsAIPlayer)
        {
            //Debug.DrawLine (FirePosition.position, new Vector3(FireRange * flip, FirePosition.position.y + yOffset, FirePosition.position.z) , Color.red);
            Debug.DrawLine(FirePosition.position, FirePosition.forward * FireRange, Color.red);
        }

        if (Physics.Raycast (FirePosition.position, FirePosition.forward * FireRange, out hit)) {

			Debug.DrawLine (FirePosition.position, FirePosition.forward * FireRange, Color.red);
			Debug.Log ("Hit Object===" + hit.collider.name);

			if (hit.collider.tag == "Enemy" || hit.collider.tag == "Player") {
				var health = hit.transform.GetComponent<CharacterHealth> ();
				health.Damage (DamageAmount);
			}
            //			var lr = bullet.GetComponent<LineRenderer> ();
            //			lr.SetPosition (0, FirePosition.position);
            //			lr.SetPosition (1, new Vector3 (FireRange, FirePosition.position.y, FirePosition.position.z));
            
		}

		//else if (Physics.Raycast (FirePosition.position, new Vector3(-FireRange * flip , FirePosition.position.y , FirePosition.position.z), out hit) && ccRef.IsAIPlayer && mAIEnemy.Type == AIEnemy.EnemyType.ASSAULT) {

		//	Debug.DrawLine (FirePosition.position, new Vector3(-FireRange * flip, FirePosition.position.y , FirePosition.position.z) , Color.red);
		//	//Debug.Log ("Hit Object===" + hit.collider.name);

		//	if (hit.collider.tag == "Player") {
		//		var health = hit.transform.GetComponent<CharacterHealth> ();
		//		health.Damage (DamageAmount);
		//	}
		//	//			var lr = bullet.GetComponent<LineRenderer> ();
		//	//			lr.SetPosition (0, FirePosition.position);
		//	//			lr.SetPosition (1, new Vector3 (FireRange, FirePosition.position.y, FirePosition.position.z));
		//}



		//else if (Physics.Raycast (FirePosition.position, new Vector3(FirePosition.position.x , FirePosition.position.y , -FireRange), out hit) && ccRef.IsAIPlayer && mAIEnemy.Type == AIEnemy.EnemyType.SNIPER) {

		//	Debug.DrawLine (FirePosition.position, new Vector3(FirePosition.position.x , FirePosition.position.y , -FireRange) , Color.red);
		//	Debug.Log ("Hit Object===" + hit.collider.name);

		//	if (hit.collider.tag == "Player") {
		//		var health = hit.transform.GetComponent<CharacterHealth> ();
		//		health.Damage (DamageAmount);
		//	}
		//	//			var lr = bullet.GetComponent<LineRenderer> ();
		//	//			lr.SetPosition (0, FirePosition.position);
		//	//			lr.SetPosition (1, new Vector3 (FireRange, FirePosition.position.y, FirePosition.position.z));
		//}

		//if(!ccRef.IsAIPlayer)
		//	bullet.GetComponent<Rigidbody> ().velocity = Vector3.forward * FireRange;
		//else
		//  bullet.GetComponent<Rigidbody> ().AddForce(transform.forward * FireRange);
		//  bullet.GetComponent<Rigidbody> ().velocity = transform.forward * FireRange;


		Destroy (bullet, 0.25f);
		//}

		FireTimer = 0.0f;
	}
		
}
