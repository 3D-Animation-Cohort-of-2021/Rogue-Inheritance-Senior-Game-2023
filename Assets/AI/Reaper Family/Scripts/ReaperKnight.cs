using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReaperKnight : ReaperBase
{
    [SerializeField]private float grappleRange, meleeRange, chancetoGrapple, attackCheckTime, restTime, grappleWindupTime, meleeWindupTime;

    private WaitForSeconds attackCheckPeriod, restPeriod;
    [SerializeField]private GameObject visualBox, decalObj, diskObj;
    private DecalProjector grappleProjector;
    [SerializeField]private bool isBusy;
    private Material dMat;

    // Start is called before the first frame update
    protected new void Awake()
    {
        base.Awake();
        Debug.Log("This is a knight");
        attackCheckPeriod = new WaitForSeconds(attackCheckTime);
        restPeriod = new WaitForSeconds(restTime);
        isDead = false;
        visualBox.SetActive(false);
        grappleProjector = decalObj.GetComponent<DecalProjector>();
        Material gMat = grappleProjector.material;
        gMat.SetFloat("_Time_To_Fill", grappleWindupTime);
        dMat = diskObj.GetComponent<MeshRenderer>().material;
        dMat.SetFloat("_Time_To_Fill", meleeWindupTime);

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
        StartCoroutine(DisplayMeleeWindup());
        yield return new WaitForSeconds(meleeWindupTime);
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
        //Debug.DrawRay(transform.position, transform.forward * 8f, Color.green, 1f);
        //visualBox.SetActive(true);
        StartCoroutine(DisplayGrappleWindup());
        yield return new WaitForSeconds(grappleWindupTime);
        //
        if (PlayerIsInArea(visualBox))
        {
            Debug.Log("Grappled Player");
            StartCoroutine(MoveToPlayer(.15f, playerCharacter.transform.position));
            //actualGrapple Function
            Debug.Log("SMACK");
        }
        else
        {
            Debug.Log("Whiffed!");
            yield return restPeriod;
        }
        //visualBox.SetActive(false);
        
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

    private IEnumerator DisplayGrappleWindup()
    {
        float timeElapsed = 0f;
        Material gMat = grappleProjector.material;
        decalObj.SetActive(true);
        Debug.Log("MeleeVisual");
        while (timeElapsed <= grappleWindupTime)
        {
            timeElapsed += Time.deltaTime;
            gMat.SetFloat("_Progress_Float", Mathf.Lerp(0,1, timeElapsed/grappleWindupTime));
            yield return new WaitForEndOfFrame();
        }
        decalObj.SetActive(false);
    }

    private IEnumerator DisplayMeleeWindup()
    {
        float timeElapsed = 0f;
        //Debug.Log("Starting windup");
        diskObj.SetActive(true);
        while (timeElapsed <= meleeWindupTime)
        {
            timeElapsed += Time.deltaTime;
            dMat.SetFloat("_ProgressFloat", Mathf.Lerp(0,1, timeElapsed/meleeWindupTime));
            yield return new WaitForEndOfFrame();
        }
        diskObj.SetActive(false);
    }

    private IEnumerator MoveToPlayer(float travelTime, Vector3 targetLocation)
    {
        Vector3 startPos = transform.position;
        float currentTime = 0f;
        WaitForEndOfFrame wff = new WaitForEndOfFrame();
        while (currentTime < travelTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetLocation, currentTime / travelTime);
            yield return wff;
        }

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
