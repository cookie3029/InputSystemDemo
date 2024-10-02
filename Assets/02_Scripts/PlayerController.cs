using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionProperty moveAction; // Vectro2 (x, y)
    [SerializeField] private InputActionProperty attackAction; // 버튼 bool

    private CharacterController controller;
    private CinemachineCamera cinemachineCamera;

    private Vector2 moveInput;

    void OnEnable()
    {
        moveAction.action.performed += (ctx) =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        moveAction.action.canceled += (ctx) =>
        {
            moveInput = Vector3.zero;
        };

        attackAction.action.performed += _ =>
        {
            Debug.Log("공격");
        };
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cinemachineCamera = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        // 씨네머신 카메라의 벡터를 계산
        Vector3 camForward = cinemachineCamera.transform.forward;
        Vector3 camRight = cinemachineCamera.transform.right;

        // 평면 벡터로 변환
        camForward.y = camRight.y = 0.0f;

        // 벡터의 정규화
        camForward.Normalize(); // camForward = camForward.nomalized
        camRight.Normalize();   // camRight = camRight.nomalized

        // 이동 벡터 계산
        // Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        Vector3 moveDir = (camForward * moveInput.y) + (camRight * moveInput.x);

        moveDir.Normalize();

        // 회전 처리
        transform.rotation = Quaternion.LookRotation(moveDir);

        // 이동 처리
        controller.Move(moveDir * Time.deltaTime * 8.0f);
    }
}
