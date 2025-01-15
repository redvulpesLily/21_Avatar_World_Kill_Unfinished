using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AvatarController : MonoBehaviourPun
{
    #region 변수
    public bool  canMove = true;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private PhotonView _photonView;
    private Camera _mainCamera;
    private Animator _avatarAnim;

    private bool _isWalking;
    #endregion

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        _mainCamera = Camera.main; // 메인 카메라 할당
        _avatarAnim = GetComponent<Animator>(); // 아바타 애니메이터 할당
    }

    public void Update()
    {
        if (_photonView.IsMine == false)
            return;

        if (canMove == false)
        {
            _avatarAnim.SetBool("IsWalking", false);
            return;
        }

        // 입력값 가져오기
        float horizontal = Input.GetAxis("Horizontal"); // A, D 키
        float vertical = Input.GetAxis("Vertical");     // W, S 키

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        _isWalking = inputDirection.magnitude >= 0.1f;

        if (_isWalking)
        {
            // 카메라 기준 이동 방향 계산
            Vector3 cameraForward = _mainCamera.transform.forward;
            Vector3 cameraRight = _mainCamera.transform.right;

            // Y축만 유지 (평면 이동)
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // 입력 방향을 카메라 방향에 맞게 변환
            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            // 캐릭터 회전
            //Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 캐릭터 이동
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        MousePos();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hitInfo, 3f);

            Debug.DrawRay(transform.position, transform.forward * 3f, Color.red, 1.5f);

            var target = hitInfo.collider.GetComponent<PlayerAvatar>();

            if (target != null)
            {
                photonView.RPC(nameof(Kill), RpcTarget.All, target);
            }
        }

        // 아바타 걷기 애니메이션 재생
        _avatarAnim.SetBool("IsWalking", _isWalking);
    }

    #region 마우스 이동에 따른 캐릭터의 회전
    private void MousePos()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(GroupPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }
    #endregion

    [PunRPC]
    private void Kill(int targetActorNumber)
    {
        var allAvatars = FindObjectsByType<PlayerAvatar>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        PlayerAvatar targetAvatar = null;

        foreach (var avatar in allAvatars)
        {
            if (avatar.IsTargetAvatar(targetActorNumber))
            {
                targetAvatar = avatar;
                break;
            }
        }

        if (targetAvatar == null)
            return;

        if (targetActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if(_stopMove != null) StopCoroutine(_stopMove);

            _stopMove = StopMove(targetActorNumber);
            StartCoroutine(_stopMove);
        }

        targetAvatar.PlayerDie(targetActorNumber);
    }

    private IEnumerator _stopMove;
    private IEnumerator StopMove(int targetActorNumber)
    {
        canMove = false;

        yield return new WaitForSeconds(5f);

        canMove = true;
    }
}