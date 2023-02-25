using System.Collections;
using UnityEngine;

public class ReaperKnight : ReaperBase
{
    [SerializeField]private float grappleRange, meleeRange, chancetoGrapple, attackCheckTime, restTime;

    private WaitForSeconds attackCheckPeriod, restPeriod;

    [SerializeField]private bool isBusy;
    // Start is called before the first frame update
    protected new void Awake()
    {
        base.Awake();
        Debug.Log("This is a knight");
        attackCheckPeriod = new WaitForSeconds(attackCheckTime);
        restPeriod = new WaitForSeconds(restTime);
    }
    void Start()
    {
        navAgent.SetDestination(playerCharacter.transform.position);
    }

    private void Update()
    {
        
    }

    private IEnumerator KnightAttackRoutine()
    {
        float distanceFromPlayer;
        while (!isDead)
        {
            if (!isBusy)
            {
                yield return attackCheckPeriod;
                distanceFromPlayer = Vector3.Distance(transform.position, playerCharacter.transform.position);
                if (distanceFromPlayer <= meleeRange)
                {
                    isBusy = true;
                    StartCoroutine(MeleeAttack());
                }
                else if (distanceFromPlayer <= grappleRange)
                {
                    if (Random.Range(0, 100) >= chancetoGrapple)
                    {
                        isBusy = true;
                        StartCoroutine(GrappleAttack());
                    }
                    else Debug.Log("Grapple roll failed");
                }
                else
                {
                    //chase the player
                }
            }
        }
    }

    private IEnumerator MeleeAttack()
    {
        Debug.Log("Performed a melee attack");
        yield return restPeriod;
        Debug.Log("DoneResting");
        yield return restPeriod;
        isBusy = false;
    }

    private IEnumerator GrappleAttack()
    {
        Debug.Log("StartingGrapple");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Grappled Player");
        transform.position = playerCharacter.transform.position;
        Debug.Log("SMACK");
        yield return restPeriod;
        isBusy = false;
    }
}
