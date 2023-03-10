using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReaperKnight : ReaperBase
{
    [Header("General")]
    [SerializeField] private float attackCheckTime;
    [Header("Combat-Grapple")] 
    [SerializeField] private float chancetoGrapple;
    [SerializeField] private float grappleRange;
    [SerializeField] private float grappleWindupTime;
    [SerializeField] private float stunTime;
    [Header("Combat-Melee")]  
    [SerializeField] private float meleeWindupTime;
    [SerializeField] private float meleeRange;

    private WaitForSeconds attackCheckPeriod, stunPeriod;
    [SerializeField]private GameObject visualBox, decalObj, diskObj;
    private DecalProjector grappleProjector;
    private bool isBusy;
    private Material dMat;

    [SerializeField] private Animator meshAnimator;
    // Start is called before the first frame update
    protected new void Awake()
    {
        base.Awake();
        Debug.Log("This is a knight");
        attackCheckPeriod = new WaitForSeconds(attackCheckTime);
        stunPeriod = new WaitForSeconds(stunTime);
        isDead = false;
        visualBox.SetActive(false);
        grappleProjector = decalObj.GetComponent<DecalProjector>();
        Material gMat = grappleProjector.material;
        gMat.SetFloat("_Time_To_Fill", grappleWindupTime);
        dMat = diskObj.GetComponent<MeshRenderer>().material;
        dMat.SetFloat("_Time_To_Fill", meleeWindupTime);
        meshAnimator = GetComponentInChildren<Animator>();

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
        meshAnimator.SetTrigger("MeleeAttack");
        yield return new WaitForSeconds(1f);
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
        meshAnimator.SetTrigger("GrappleLaunch");
        yield return new WaitForSeconds(.3f);
        //
        if (PlayerIsInArea(visualBox))
        {
            Debug.Log("Grappled Player");
            StartCoroutine(MoveToPlayer(.15f, playerCharacter.transform.position));
            meshAnimator.SetTrigger("GrappleYes");
            Debug.Log("SMACK");
        }
        else
        {
            Debug.Log("Whiffed!");
            meshAnimator.SetTrigger("GrappleNo");
            yield return stunPeriod;
            meshAnimator.SetTrigger("StopStun");
        }
        //visualBox.SetActive(false);
        
        Debug.Log("Done Resting");
        isBusy = false;
    }

    private void StopChasing()
    {
        navAgent.enabled = false;
        meshAnimator.SetBool("Chasing", false);
    }

    private void ChaseAgain()
    {
        navAgent.enabled = true;
        navAgent.SetDestination(playerCharacter.transform.position);
        meshAnimator.SetBool("Chasing", true);
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
        meshAnimator.SetTrigger("GrappleStart");
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
        meshAnimator.SetTrigger("MeleeWindup");
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
