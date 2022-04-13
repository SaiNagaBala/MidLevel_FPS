using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    Animator anim;
    public GameObject target;
    NavMeshAgent agent;

    public float walkingSpeed;
    public float runningSpeed;

    //public List<AudioClip> audioClips;
    //AudioSource audioSource;

    public GameObject ragDollPrefab;

    enum STATE { IDLE,WONDER,CHASE,ATTACK,DEAD};
    STATE state = STATE.IDLE;


    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        //anim.SetBool("isWalking", true);
        agent = this.GetComponent<NavMeshAgent>();
        //audioSource = GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioClips[0]);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);
            audioSource.PlayOneShot(audioClips[1]);
        }
        /*
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);

        if (Input.GetKey(KeyCode.R))
        {
            anim.SetBool("isRunning", true);
        }
        else
            anim.SetBool("isRunning", false);

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isAttacking", true);
        }
        else
            anim.SetBool("isAttacking", false);

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isDead", true);
        }
        */

        /*if(Input.GetKeyDown(KeyCode.R))
        {
            GameObject tempRd = Instantiate(ragDollPrefab, this.transform.position, this.transform.rotation);
            tempRd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*10000);
            Destroy(this.gameObject);
            return;
        }*/

        if (target==null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }


        switch (state)
        {
            case STATE.IDLE:
                if (CanSeePlayer())
                {
                    state = STATE.CHASE;
                }
                else if (Random.Range(0,1000)<5)
                {
                    state = STATE.WONDER;
                }
               
                
                state = STATE.WONDER;

                break;
            case STATE.WONDER:
                if(!agent.hasPath)
                {
                    float randValueX = transform.position.x + Random.Range(-5f, 5f);
                    float randValueZ = transform.position.z + Random.Range(-5f, 5f);
                    float ValueY = transform.position.y + Terrain.activeTerrain.SampleHeight(new Vector3(randValueX, 0, randValueZ));
                    Vector3 destination = new Vector3(randValueX, ValueY, randValueZ);
                    agent.SetDestination(destination);
                    agent.stoppingDistance = 0f;
                    agent.speed = walkingSpeed;
                    TurnOffAllTriggerAnim();
                    anim.SetBool("isWalking", true);
                }

                if (CanSeePlayer())
                {
                    state = STATE.CHASE;
                }
                else if (Random.Range(0,1000)<10)
                {
                    state = STATE.IDLE;
                    TurnOffAllTriggerAnim();
                    agent.ResetPath();
                }
                
                break;

            case STATE.CHASE:
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 2;
                TurnOffAllTriggerAnim();
                anim.SetBool("isRunning", true);
                agent.speed = runningSpeed;
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = STATE.ATTACK;
                }
                if (CannotSeePlayer())
                {
                    state = STATE.WONDER;
                    agent.ResetPath();
                }

                break;

            case STATE.ATTACK:
                TurnOffAllTriggerAnim();
                anim.SetBool("isAttacking", true);
                transform.LookAt(target.transform.position);
                print("is attacking");
                if (DistanceToPlayer()>agent.stoppingDistance)
                {
                    state = STATE.CHASE;
                }

                break;

            case STATE.DEAD:

                GameObject tempRd = Instantiate(ragDollPrefab, this.transform.position, this.transform.rotation);
                tempRd.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10000);
                Destroy(this.gameObject);
                break;


            default:
                break;
        }
    }

    public void TurnOffAllTriggerAnim()
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDead", false);
    }

    public bool CanSeePlayer()
    {
        if (DistanceToPlayer()<10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float DistanceToPlayer()
    {
        return Vector3.Distance(target.transform.position, this.transform.position);
    }

    public bool CannotSeePlayer()
    {
        if (DistanceToPlayer()>20f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
