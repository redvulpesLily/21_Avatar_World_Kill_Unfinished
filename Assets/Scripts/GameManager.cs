using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    #region SingleTon
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();

            return _instance;
        }
    }
    #endregion
    
    public PlayerAvatar MyAvatar { get; private set; }
    
    CinemachineVirtualCamera followCamera;

    private void Awake()
    {
        followCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Main������ ��ȯ�Ǿ�����
    // ���� ���� ����� �Լ�
    public void Start()
    {
        SpawnAvatar();
    }

    private void SpawnAvatar()
    {
        // -1���� 1���� ������ ���ڸ� �����ؼ�
        // ������ ��ġ�� �ƹ�Ÿ �����ϵ��� ��ġ�� ����
        float x = Random.Range(-1f, 1f);
        float y = 0f;
        float z = Random.Range(-1f, 1f);

        Vector3 randomSpwanPosition = new Vector3(x, y, z);

        // ���� ��Ʈ��ũ Ŭ������ �̿��ؼ� ��ü Ŭ���̾�Ʈ�� PlayerAvatar��� ���� ������Ʈ�� ����
        GameObject avatarObject = PhotonNetwork.Instantiate("PlayerAvatar", randomSpwanPosition, Quaternion.identity);

        // ����ī�޶� �ƹ�Ÿ ������Ʈ�� �����ϵ��� ����
        followCamera.Follow = avatarObject.transform;
        followCamera.LookAt = avatarObject.transform;

        MyAvatar = avatarObject.GetComponent<PlayerAvatar>();
        MyAvatar.photonView.RPC("SetNickname", RpcTarget.AllBuffered, PhotonNetwork.NickName, PhotonNetwork.LocalPlayer.ActorNumber);
    }
}
