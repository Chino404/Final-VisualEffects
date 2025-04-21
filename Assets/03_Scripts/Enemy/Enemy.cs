using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    FSM _fsm;

    //public Node initialNode;
    //public Node goalNode;
    //public List<Node> path;

    [Header("Params")]
    [HideInInspector] public Vector3 velocity; //Lo hice publico para que pueda modificarlo en el PatrolEnemy
    public float maxVelocity;
    public float maxForce;

    [Header("Values")]
    public Transform target;
    public Transform[] wayPointsPatrol;
    public float viewRadius; //Area de vision
    public float viewAngle;  //Angulo de vision

    void Start()
    {
        _fsm = new FSM();
        _fsm.CreateState("Patrullar", new PatrolEnemy(_fsm, wayPointsPatrol, target, this));

        _fsm.ChangeState("Patrullar");
    }

    void Update()
    {
        _fsm.Execute();
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable dmg = collision.gameObject.GetComponent<IDamageable>();

        if(dmg != null)
        {
            dmg.Damage();
        }
    }

    #region Visualizar el rango de Vision
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lineA = GetVectorFromAngle(viewAngle * 0.5f + transform.eulerAngles.y);
        Vector3 lineB = GetVectorFromAngle(-viewAngle * 0.5f + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);
    }

    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    #endregion
}
