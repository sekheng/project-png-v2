using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalPathFollow : MonoBehaviour
{
    public bool hasRunAnim = false;
    public float walkSpeed, runSpeed, rotationSpeed;
    [Range(1,10)]
    public int touchAnimationsCount = 1;

    public UnityEvent OnTouch;

    [SerializeField]
    protected Transform[] waypoints;

    protected int currentWayPoint = 0;

    private bool isInTouch = false;

    private int touchAnim = 1;

    [SerializeField]
    private Animator anim;

    private Coroutine routine;
    public float speed;

    void Start()
    {
        speed = walkSpeed;
        waypoints = WaypointsHolder.Instance.GetWaypoints(gameObject.name);
    }

    void LateUpdate()
    {
        if (isInTouch)
        {
            return;
        }

        if (Vector3.Distance(transform.position, waypoints[currentWayPoint].position) < 0.1f)
        {
            ReachedWaypoint(waypoints[currentWayPoint]);
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWayPoint].position, Time.deltaTime * speed);
        transform.forward = Vector3.MoveTowards(transform.forward, waypoints[currentWayPoint].position - transform.position, Time.deltaTime * rotationSpeed);

        var rot = transform.localEulerAngles;
        rot.x = rot.z = 0;
        transform.localEulerAngles = rot;
    }

    public void OnAnimalTouch()
    {
        if (!isInTouch)
        {
            isInTouch = true;
            OnTouch.Invoke();
        }
    }

    public void RestartWalk()
    {
        isInTouch = false;
    }

    public void PlayTouchAnimation(string animName)
    {
        if (touchAnim >= touchAnimationsCount && hasRunAnim)
        {
            isInTouch = false;
            speed = runSpeed;
            anim.Play("Run");
            Invoke(nameof(ResetRun), 5);
        }
        else
        {
            speed = walkSpeed;
            anim.Play(animName + touchAnim);
        }
        if(hasRunAnim)
            touchAnim = ((touchAnim + 1) % (touchAnimationsCount+1));
        else
            touchAnim = ((touchAnim + 1) % touchAnimationsCount);
    }

    private void ResetRun()
    {
        speed = walkSpeed;
        anim.SetTrigger("Walk");
    }

    protected virtual void ReachedWaypoint(Transform waypoint)
    {
        var wp = waypoint.GetComponent<Waypoint>();
        if (wp != null)
        {
            if (!string.IsNullOrEmpty(wp.animationName))
                GetComponent<Animator>().Play(wp.animationName);
            if (wp.speed > 0)
            {
                speed = wp.speed;
            }
            if (wp.delayBeforeNextWaypoint > 0 && routine == null)
            {
                routine = StartCoroutine(WaitForNextWaypoint(wp.delayBeforeNextWaypoint));
                return;
            }
        }
        if(routine == null)
            currentWayPoint = (currentWayPoint + 1) % waypoints.Length;
    }

    IEnumerator WaitForNextWaypoint(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentWayPoint = (currentWayPoint + 1) % waypoints.Length;
        GetComponent<Animator>().SetTrigger("Walk");
        routine = null;
    }
}