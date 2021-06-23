using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Photon.Pun;

public class CharacterControlV2 : MonoBehaviour
{

    public float runSpeed;
    public float walkSpeed;
    public float moveSpeed;
    public float crouchSpeed;
    public bool Crouch;

    //public SimpleTouchController mInputLeft;
    //public SimpleTouchController mInputRight;

    private bool running;
    public bool attack;
    public bool Dead = false;
    public bool Grounded = true;

    Rigidbody rb;
    Animator anim;

    private NavMeshAgent navAgent;
    private bool faceRight;
    public bool IsAIPlayer;
    //private AIEnemy mAIEnemy;

    private Vector3 detectedPlayer;
    private bool firstDetection;
    public float detectionTime;
    public float stoppingDistance;
    public float attackDistance;
    float startRunTime;
    float OriginalTargetDist;
    private bool HasArrived;
    private Weapon mCurrentWeapon;
    private float mCurrentDistance;
    private float mPrevDistance;

    float moveX = 0.0f;
    float moveY = 0.0f;

    public float JumpHeight;
    float groundRadius = 0.1f;
    Collider[] groundCollisions;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public Transform ShoulderTransform;
    public Transform rightShoulder;

    GameObject rsp;
    public Vector3 lookPos;

    public Vector3 AimOffset;

    Transform chest;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //chest = anim.GetBoneTransform(HumanBodyBones.Chest);

        rsp = new GameObject();
        rsp.name = transform.name + " IK Handler intiialiser";

        //AimOffset = chest.rotation.eulerAngles;

        mCurrentWeapon = GetComponentInChildren<Weapon>();
        faceRight = true;
        Crouch = false;
        running = false;

        if (IsAIPlayer)
        {
            firstDetection = false;
            HasArrived = false;
            //mAIEnemy = GetComponent<AIEnemy>();

            navAgent = GetComponent<NavMeshAgent>();
            anim.SetBool("Stop", true);
        }
        Physics.IgnoreLayerCollision(0, 8);
    }


    void Update()
    {

    }

    void FixedUpdate()
    {
        PlayerMove();
        //RotationHandler();
        //AimingPosHandle();
        //ShoulderHandler();
        //Attack ();
    }

    public void RotationHandler()
    {
        Vector3 directionLook = lookPos - transform.position;
        directionLook.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionLook);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f);
    }

    public void AimingPosHandle()
    {
        //if (IsAIPlayer)
        //{
        //    if(mAIEnemy.GetTarget() != null)
        //    {
        //        Vector3 lookPAI = mAIEnemy.GetTarget().position;
        //        //lookPAI.z = transform.position.z;

        //        //if (lookPAI.x > transform.position.x)
        //        //    lookPAI.x = mAIEnemy.GetTarget().position.x + 10.0f;
        //        //else
        //        //    lookPAI.x = mAIEnemy.GetTarget().position.x - 10.0f;

        //        lookPos = lookPAI;
        //    }

        //    //lookPos.z = transform.position.z;

            

        //    return;
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //if (hit.collider.tag == "Enemy")
                //{
                    Vector3 lookPF = hit.point;
                    lookPos = lookPF + AimOffset;
                    mCurrentWeapon.Fire();
                    attack = true;
                    return;
                //}
            }
        }

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (!attack)
            {
                Vector3 lookP = hit.point;
                lookP.z = transform.position.z;
                lookPos = lookP;
            }
        }

        


        //if (mInputRight.GetTouchPosition == Vector2.zero)
        //{
        //    if (lookPos.x < transform.position.x)
        //        lookPos.x = transform.position.x - 10.0f;
        //    else if (lookPos.x > transform.position.x)
        //        lookPos.x = transform.position.x + 10.0f;

        //    return;
        //}

        lookPos.z = transform.position.z;

        //lookPos = (new Vector2(move,0)) * AimOffset;
    }

    public void ShoulderHandler()
    {
        ShoulderTransform.LookAt(lookPos);
        Vector3 rightShoulderPos = rightShoulder.TransformPoint(Vector3.zero);
        rsp.transform.position = rightShoulderPos;
        rsp.transform.parent = transform;

        ShoulderTransform.position = rsp.transform.position;
    }

    public void Attack()
    {
        mCurrentWeapon.Fire();
    }

    public void PlayerMove()
    {
        if (IsAIPlayer)
            return;

#if UNITY_EDITOR
        moveX = Input.GetAxis ("Horizontal");
        moveY = Input.GetAxis ("Vertical");
#else
        move = mInputLeft.GetTouchPosition.x;
        moveY = mInputLeft.GetTouchPosition.y;
#endif

        if (Input.GetKeyDown(KeyCode.C))
            Crouch = !Crouch;
        else if (Input.GetAxis("Jump") > 0 && Crouch)
            Crouch = false;
        else if (Input.GetAxis("Jump") > 0 && !Crouch  && Grounded)
        {
            Grounded = false;
            anim.SetBool("Grounded", Grounded);
            rb.AddForce(new Vector3(0.0f, JumpHeight, 0.0f));

        }


        //groundCollisions = Physics.OverlapSphere(groundCheck.transform.position, groundRadius, groundLayer);
        //if (groundCollisions.Length > 0)
        //    Grounded = true;
        //else
        //    Grounded = false;

        //		if (moveY < 0)
        //			Crouch = true;
        //		if (moveY > 0)
        //			Crouch = false;
 
        anim.SetBool("Grounded", Grounded);
        anim.SetBool("crouch", Crouch);

        if ((moveY <= 0.5f && moveY >= 0.0f) || (moveY >= -0.5f && moveY <= 0.0f))
            moveSpeed = walkSpeed;
        else
            moveSpeed = runSpeed;

        if (Crouch)
        {
            moveSpeed = crouchSpeed;

            //rb.velocity = new Vector3(move * moveSpeed, rb.velocity.y, 0);
        }

        //anim.SetFloat("speed", move);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveY * moveSpeed);

        float animValue = moveY;

        anim.SetFloat("speedY", animValue, .1f, Time.deltaTime);
    }

    public void NPCMove()
    {
        //anim.SetBool("Grounded", Grounded);
        //if (detectedPlayer != Vector3.zero && IsAIPlayer && mAIEnemy != null)
        //{

        //    Debug.Log("NPCMove===" + mAIEnemy.Detected);

        //    if (mAIEnemy.Type == AIEnemy.EnemyType.SNIPER)
        //    {
        //        attack = true;
        //        return;
        //    }

        //    //if (detectedPlayer.position.x < transform.position.x && faceRight)
        //    //    Flip();
        //    //else if (detectedPlayer.position.x > transform.position.x && !faceRight)
        //    //    Flip();
        //    //
        //    //			if (mAIEnemy.Detected && faceRight)
        //    //				rb.velocity = new Vector3 (moveSpeed, rb.velocity.y, 0);
        //    //			else if(mAIEnemy.Detected && !faceRight)
        //    //				rb.velocity = new Vector3 ((-1) * moveSpeed, rb.velocity.y, 0);

        //    //if (!firstDetection)
        //    //{
        //    //    startRunTime = Time.time + detectionTime;
        //    //    firstDetection = true;
        //    //}


        //    float deltaDist = 0.0f;
        //    if (!running && mAIEnemy.Detected)
        //    {
        //        //if (startRunTime < Time.time)
        //        //{
        //            //anim.SetBool ("Walk" , false);


        //            //anim.SetFloat ("Move", 1.0f);
        //        moveSpeed = runSpeed;
        //        running = true;
        //        deltaDist = Vector3.Distance(transform.position, mAIEnemy.GetTarget().position);
        //        Debug.Log(deltaDist);

        //        // }
        //        // else
        //        //{
        //        //    moveSpeed = walkSpeed;
        //        //}
        //    }

            //Debug.Log ("Distance====" + Vector3.Distance(detectedPlayer.position,transform.position));
            //Rigidbody playerRB = detectedPlayer.GetComponent<Rigidbody>();


            


            //if (!mAIEnemy.Detected)
            //{

            //    if (!firstDetection)
            //    {
            //        startRunTime = Time.time + detectionTime;
            //        firstDetection = true;
            //    }

            //    if (startRunTime > detectionTime)
            //    {
            //        deltaDist = Vector3.Distance(transform.position, mAIEnemy.GetLastPosition());
            //        startRunTime = 0.0f;
            //    }
            //}


            ////if (mathf.abs(deltadist) < 1.0f)
            ////    movespeed = walkspeed;

            //Debug.Log("Not Detected...Searching for Last Target Position");

            //navAgent.SetDestination(mAIEnemy.GetTarget().position);

            //Debug.Log("RemainingDistance====" + navAgent.remainingDistance);

            //if (Vector3.Distance(mAIEnemy.GetTarget().position, transform.position) < attackDistance)
            //{
            //    attack = true;
            //}

            //else if (Vector3.Distance(mAIEnemy.GetTarget().position, transform.position) >= attackDistance)
            //{
            //    attack = false;
            //}

            ////if (Vector3.Distance(mAIEnemy.GetTarget().position, transform.position) < stoppingDistance)
            ////{
            ////    //				if (playerRB.velocity == Vector3.zero && ((detectedPlayer.position.x + stoppingDistance) - transform.position.x) > -0.05f) {
            ////    //					rb.velocity = Vector3.zero;
            ////    //					anim.SetBool ("Stop", true);
            ////    //					anim.SetFloat ("Move", 0.0f);
            ////    //					running = false;
            ////    //				} else {
            ////    //					moveSpeed = runSpeed;
            ////    //					anim.SetBool ("Walk" , false);
            ////    //					anim.SetBool ("Stop", false);
            ////    //					anim.SetFloat ("Move", -1.0f);
            ////    //					rb.velocity = new Vector3 (2, rb.velocity.y, 0);
            ////    //					running = true;
            ////    //				}

            ////    rb.velocity = Vector3.zero;

            ////    moveSpeed = 0;
            ////}
            //else if (Vector3.Distance(detectedPlayer, transform.position) >= stoppingDistance)
            //{
            //    //				if (playerRB.velocity == Vector3.zero && ((detectedPlayer.position.x + stoppingDistance) - transform.position.x) < 0.05f) {
            //    //					rb.velocity = Vector3.zero;
            //    //					anim.SetBool ("Stop", true);
            //    //					anim.SetFloat ("Move", 0.0f);
            //    //					running = false;
            //    //				
            //    //				} else {
            //    //					moveSpeed = runSpeed;
            //    //					anim.SetBool ("Walk" , false);
            //    //					anim.SetBool ("Stop", false);
            //    //					anim.SetFloat ("Move", 1.0f);
            //    //					rb.velocity = new Vector3 (-moveSpeed, rb.velocity.y, 0);
            //    //					running = true;
            //    //				}
            //}


            //Debug.Log("Ratio===="+ (deltaDist / OriginalTargetDist));

            //float animValue = navAgent.velocity.x / navAgent.speed;
            ////if (moveSpeed == 0)
            ////    animValue = 0.0f;
            ////else if (moveSpeed == walkSpeed)
            ////    animValue = 0.49f;
            ////else
            ////    animValue = 1.0f;


            //Debug.Log("Anim==="+animValue);
            ////Not required for AI
            ////if (lookPos.x < transform.position.x)
            ////{
            ////    animValue = -animValue;
            ////}

            ////if (lookPos.x > transform.position.x)
            ////    rb.velocity = new Vector3(moveSpeed, rb.velocity.y, 0);
            ////else
            ////    rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, 0);
           

            //anim.SetFloat("speed", Mathf.Abs(animValue), .1f, Time.deltaTime);
       // }
    }

    //public void StopFire()
    //{
    //    detectedPlayer = Vector3.zero;
    //    navAgent.isStopped = true;
    //    attack = false;
    //    anim.SetFloat("speed", 0.0f);
    //    //anim.SetBool("Stop", true);
    //    //anim.SetFloat("Move", 0.0f);
    //}

    //public void Death()
    //{
    //    detectedPlayer = Vector3.zero;
    //    attack = false;
    //    anim.Play("Death");
    //    Dead = true;
    //    mCurrentWeapon.gameObject.SetActive(false);
    //    GetComponent<IKHandler>().enabled = false;
    //    moveSpeed = 0.0f;
    //    //GetComponent<Collider> ().enabled = false;
    //}

    //public void EnemyDetected(Vector3 target)
    //{
    //    detectedPlayer = target;

    //    OriginalTargetDist = Vector3.Distance(transform.position, detectedPlayer);

    //    if (mAIEnemy.Type == AIEnemy.EnemyType.SNIPER)
    //        return;

    //    //anim.SetBool ("Walk", true);
    //    anim.SetBool("Stop", false);

    //    //if (detectedPlayer.position.x < transform.position.x && faceRight)
    //    //    Flip();
    //    //else if (detectedPlayer.position.x > transform.position.x && !faceRight)
    //    //    Flip();

    //}

    //void Flip()
    //{
    //    faceRight = !faceRight;
    //    //Vector3 zScale = transform.localScale;
    //    //zScale.z *= -1;
    //    //transform.localScale = zScale;

    //    if (IsAIPlayer)
    //    {
    //        if (faceRight)
    //            mCurrentWeapon.flip = -1;
    //        else
    //            mCurrentWeapon.flip = 1;
    //    }
    //    else
    //    {
    //        if (faceRight)
    //            mCurrentWeapon.flip = 1;
    //        else
    //            mCurrentWeapon.flip = -1;
    //    }
    //}
}
