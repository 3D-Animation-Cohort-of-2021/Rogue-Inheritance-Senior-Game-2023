using System.Collections;
using UnityEngine;

public class ReaperKnight : ReaperBase
{
    [SerializeField]private float grappleRange, meleeRange, chancetoGrapple, attackCheckTime, restTime;

    private WaitForSeconds attackCheckPeriod, restPeriod;
    [SerializeField]private GameObject visualBox;

    [SerializeField]private bool isBusy;
    // Start is called before the first frame update
    protected new void Awake()
    {
        base.Awake();
        Debug.Log("This is a knight");
        attackCheckPeriod = new WaitForSeconds(attackCheckTime);
        restPeriod = new WaitForSeconds(restTime);
        isDead = false;
        visualBox.SetActive(false);
    }
    void Start()
    {
        //navAgent.SetDestination(playerCharacter.transform.position);
        lifeRoutine = StartCoroutine(KnightAttackRoutine());
    }


    private IEnumerator KnightAttackRoutine()
    {
        float distanceFromPlayer;
        while (!isDead)
        {
            if (!isBusy)
            {
                distanceFromPlayer = Vector3.Distance(transform.position, playerCharacter.transform.position);
                if (distanceFromPlayer <= meleeRange)
                {
                    isBusy = true;
                    StartCoroutine(MeleeAttack());
                }
                else if (distanceFromPlayer <= grappleRange)
                {
                    if (Random.Range(0, 100) <= chancetoGrapple)
                    {
                        isBusy = true;
                        StartCoroutine(GrappleAttack());
                    }
                    else
                    {
                        ChaseAgain();
                        //Debug.Log("Grapple roll failed");
                    }
                }
                else
                {
                    ChaseAgain();
                    //Debug.Log("Chasing Player");
                }
            }
            yield return attackCheckPeriod;
        }
    }

    private IEnumerator MeleeAttack()
    {
        StopChasing();
        transform.LookAt(playerCharacter.transform);
        Debug.Log("Performed a melee attack");
        yield return restPeriod;
        Debug.Log("DoneResting");
        isBusy = false;
    }

    private IEnumerator GrappleAttack()
    {
        StopChasing();
        Debug.Log("StartingGrapple");
        transform.LookAt(playerCharacter.transform);
        Debug.DrawRay(transform.position, transform.forward * 8f, Color.green, 1f);
        visualBox.SetActive(true);
        //visualSphere.transform.position = playerCharacter.transform.position;
        yield return new WaitForSeconds(1f);
        //
        if (PlayerIsInArea(visualBox))
        {
            Debug.Log("Grappled Player");
            transform.position = playerCharacter.transform.position;
            Debug.Log("SMACK");
        }
        else Debug.Log("Whiffed!");
        visualBox.SetActive(false);
        yield return restPeriod;
        Debug.Log("Done Resting");
        isBusy = false;
    }

    private void StopChasing()
    {
        navAgent.enabled = false;
    }

    private void ChaseAgain()
    {
        navAgent.enabled = true;
        navAgent.SetDestination(playerCharacter.transform.position);
    }

    private bool PlayerIsInRange(float range)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward,  out hitInfo, range))
        {
            Debug.Log(hitInfo.collider.gameObject);
            // Check if the ray hits a player character
            if (hitInfo.collider.gameObject==playerCharacter)
            {
                Debug.Log("Hit player character!");
                return true;
            }
        }

        return false;
    }

    private bool PlayerIsInArea(GameObject examplebox)
    {
        Collider[] objectsInVolume = Physics.OverlapBox(examplebox.transform.position, examplebox.transform.localScale,
            examplebox.transform.rotation);
        foreach (Collider c in objectsInVolume)
        {
            if (c.gameObject == playerCharacter)
                return true;
        }

        return false;
    }
}
