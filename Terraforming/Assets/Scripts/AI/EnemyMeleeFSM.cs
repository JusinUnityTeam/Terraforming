using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeFSM : MonoBehaviour
{
    // SerializeField : 에디터에선 수정할 수 있는 private 변수
    [SerializeField] protected float m_fViewAngle = 0f;
    [SerializeField] protected float m_fDistance = 0f;
    [SerializeField] protected LayerMask m_layerMask = 0;
    [SerializeField] protected GameObject m_TargetObject = null;

    [SerializeField] protected float m_fMaxHp = 1000f;
    [SerializeField] protected float m_fCurrentHp = 1000f;

    [SerializeField] protected float m_fDamage = 100f;

    [SerializeField] protected float m_fPlayerRealizeRange = 10f;
    [SerializeField] protected float m_fAttackRange = 5f;

    [SerializeField] protected float m_fMoveSpeed = 2f;

    protected NavMeshAgent m_nvAgent;

    public enum E_STATE
    {
        Idle, Move, Attack // 열거형이자 함수명이기도 함.
    };

    public E_STATE m_eCurrentState = E_STATE.Idle;

    // 캐슁하는 것이 더 빠르다.
    //WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    //WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    //IEnumerator Start()
    //{
    //    while(true)
    //    {
    //        yield return StartCoroutine(m_eCurrentState.ToString());
    //    }
    //}

    void Start()
    {
        m_nvAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FSM());
    }


    protected virtual IEnumerator FSM()
    {
        //yield return null; // 한 프레임 쉬기
        //yield return Delay500;

        while(true)
        {
            // StartCoroutine은 인자로 전달받은 함수를 다돌아야 호출이 끝난다.
            yield return StartCoroutine(m_eCurrentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        // 잠깐 기다렸다가 다시 움직인다.
        yield return null;
        m_eCurrentState = E_STATE.Move;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        m_nvAgent.SetDestination(m_TargetObject.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.name == "Target") { 
            m_nvAgent.isStopped = true;
            m_eCurrentState = E_STATE.Idle;
        }
    }
}
