using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Speed 
    [SerializeField]       
    private float m_fWalkSpeed = 5f;        // 걷기
    [SerializeField]
    private float m_fRunSpeed = 10f;              // 달리기

    private float m_fMoveSpeed;

    // Jump
    [SerializeField]
    private float m_fJumpForce = 5f;

    // 상태 변수
    private bool isRun = false;     // 뛰는 건지 안뛰는건지
    private bool isGround = true;       // 땅인지 아닌지



    // 1인칭 Player Camera
    // 카메라 민감도 
    [SerializeField]
    private float m_fLookSensitivity = 2f;

    // 카메라 범위 한계 위아래
    [SerializeField]
    private float m_fCameraRotationLimit = 45f;

    private float m_fCurrentCameraRotationX = 0f;


    // 컴포넌트
    [SerializeField]
    private Camera m_PlayerCamera;
    private Rigidbody m_PlayerRigidbody;
    private CapsuleCollider m_CapsuleCollider;
    // Start is called before the first frame update
    void Start()
    {
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_PlayerRigidbody = GetComponent<Rigidbody>();


        m_fMoveSpeed = m_fWalkSpeed;
    }




    // Update is called once per frame
    void Update()
    {
        JudgeGround();
        TryToJump();
        TryToRun();
        Move();
        RotateCharacter();
        RotateCamera();
    }






    // 땅인지 아닌지 판단하는 곳.
    private void JudgeGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, m_CapsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryToJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            ProgressJump();
        }
    }

    private void ProgressJump()
    {
        m_PlayerRigidbody.velocity = transform.up * m_fJumpForce;
    }

    private void TryToRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        isRun = true;
        m_fMoveSpeed = m_fRunSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        m_fMoveSpeed = m_fWalkSpeed;
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");        // 오른쪽 1 왼쪽 -1
        float moveDirZ = Input.GetAxisRaw("Vertical");          // 위 1 아래 -1

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * m_fMoveSpeed;       // 방향 * 속도
        m_PlayerRigidbody.MovePosition(transform.position + velocity * Time.deltaTime);     //  deltaTime 값은 약0.016
    }

    // 좌우 캐릭터 회전
    private void RotateCharacter()
    {
        
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * m_fLookSensitivity;

        m_PlayerRigidbody.MoveRotation(m_PlayerRigidbody.rotation * Quaternion.Euler(characterRotationY));
    }

    // 상하 카메라 회전
    private void RotateCamera()
    {
        
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * m_fLookSensitivity;

        m_fCurrentCameraRotationX -= cameraRotationX;          
        m_fCurrentCameraRotationX = Mathf.Clamp(m_fCurrentCameraRotationX, -m_fCameraRotationLimit, m_fCameraRotationLimit);

        m_PlayerCamera.transform.localEulerAngles = new Vector3(m_fCurrentCameraRotationX, 0f, 0f);
    }
}
