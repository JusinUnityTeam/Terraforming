using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MonsterView : MonoBehaviour
{
    // SerializeField : 에디터에선 수정할 수 있는 private 변수
    [SerializeField] float m_fViewAngle = 0f;
    [SerializeField] float m_fDistance = 0f;
    [SerializeField] LayerMask m_layerMask = 0;

    void Sight()
    {
        // 주변에 있는 플레이어 컬라이더 검출
        Collider[] cCols = Physics.OverlapSphere(transform.position, m_fDistance, m_layerMask);

        // 플레이어는 한 명이라 가정할 때, 인덱스 0은 플레이어를 의미
        if(cCols.Length > 0)
        {
            // 플레이어 위치를 구한다.
            Transform tfPlayer = cCols[0].transform;

            // 몬스터에서 플레이어로의 방향과 각도를 구한다.
            Vector3 vDirection = (tfPlayer.position - transform.position).normalized;
            float fAngle = Vector3.Angle(vDirection, transform.forward);

            // AI의 방향&플레이어 방향의 각도차이가 시야각보다 작은지 확인!
            if(fAngle < m_fViewAngle * 0.5f)
            {
                // 시야각 안에 있다면 Ray를 쏴서 방해물이 있는지를 체크한다.
                if (Physics.Raycast(transform.position, vDirection, out RaycastHit rhHit, m_fDistance))
                {
                    // Ray에 닿은 객체가 Player라면 점점 다가가도록 한다.
                    if (rhHit.transform.name == "Player")
                    {
                        transform.position = Vector3.Lerp(transform.position, rhHit.transform.position, 0.02f);
                    }
                    // 다른 경우라면 정지해있는다.
                }
            }
        }
    }

    private void Update()
    {
        Sight();
    }
}
