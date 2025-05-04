using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimalController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    [SerializeField]
    private Transform[] waypoints;

    [SerializeField]
    private UnityEvent OnTouchEvent;

    private int waypointIndex = 0;

    private bool isTouched;

    private float baseWalkSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        baseWalkSpeed = agent.speed;

        waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name);

        agent.SetDestination(waypoints[waypointIndex].position);
        anim.SetBool("Walk", true);
    }

    private void Update()
    {
        if (!agent.isStopped && waypoints.Length>0)
        {
            if (agent.remainingDistance < 0.5f)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[waypointIndex].position);
            }
        }
        else if (agent.isStopped && (anim.GetBool("Walk") ))//|| ))
        {
            RestartWalk();
        }
        else if (!anim.GetBool("Walk") && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetBool("Walk", true);
        }
    }

    private Vector3 velocity;

    public void OnAnimalTouched()
    {
        if (!isTouched)
        {
            isTouched = true;
            agent.isStopped = true;
            velocity = agent.velocity;
            agent.velocity = Vector3.zero;
            anim.SetTrigger("Touch1");
            anim.SetBool("Walk", false);
            OnTouchEvent.Invoke();
        }
    }

    public void RestartWalk()
    {
        isTouched = false;
        agent.isStopped = false;
        agent.velocity = velocity;
    }

    public void SetWalkSpeed(int speed)
    {
        agent.speed = baseWalkSpeed * speed;
        agent.angularSpeed = Mathf.Clamp(speed * speed * speed,5,200);
        anim.speed = speed/2.0f;
    }
}