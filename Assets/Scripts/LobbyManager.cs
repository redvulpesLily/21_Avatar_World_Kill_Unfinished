using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text       connectInfoText;
    [SerializeField] private Button     joinButton;
    [SerializeField] private InputField inputField;

    private void Awake()
    {
        // Join ��ư�� �������� OnJoinButtonPressed �Լ��� ȣ��
        joinButton.onClick.AddListener(OnJoinButtonPressed);

        // �г����� �Է��ϰ� ����Ű�� ��������
        inputField.onSubmit.AddListener(OnEnterPressed);
    }

    private void Start()
    {
        // ���� ��ư�� ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ����ڿ��� ������ �������̶�� ����� �˷���
        connectInfoText.text = "������ �������Դϴ�...";

        // ���� ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // ���� ���ӿ� ���������� �ڵ������� ȣ���(����)
    public override void OnConnectedToMaster()
    {
        // ��Ȱ��ȭ �ߴ� ��ư�� �ٽ� Ȱ��ȭ������.
        joinButton.interactable = true;

        // ���� ���ӿ� �����ߴٴ� ���� ����ڿ��� �˷���.
        connectInfoText.text = "���� ���ӿ� �����߾��.\nJOIN ��ư�� �����ּ���.";
    }

    // ���� ���ӿ� �������� ��� �ڵ������� ȣ���(����)
    public override void OnDisconnected(DisconnectCause cause)
    {
        // ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        // ���� ���ӿ� �����ߴٴ� ���� ����ڿ��� �˷���.
        connectInfoText.text = "���� ���ӿ� �����߾��.\n������ ������ �õ��� �ҰԿ�.";
    }

    // Join ��ư�� �������� ����Ǵ� �Լ�
    private void OnJoinButtonPressed()
    {
        // ����ڰ� �г����� �Է����� �ʾ�����
        // �г����� �Է��� �޶�� ��û��
        if (string.IsNullOrEmpty(inputField.text))
        {
            connectInfoText.text = "�г����� �Է����ּ���";
            return;
        }
        else
        {
            string nickName = inputField.text;
            PhotonNetwork.NickName = nickName;
        }

        // ��ư ��Ÿ�� �����ϱ� ���� ��ư�� ��� ��Ȱ��ȭ
        joinButton.interactable = false;

        JoinRoom();
    }

    // �г����� �Է��ϰ� ����Ű�� �������� ����Ǵ� �Լ�
    private void OnEnterPressed(string inputText)
    {
        if (string.IsNullOrEmpty(inputText)) 
        {
            connectInfoText.text = "�г����� �Է����ּ���";
            return;
        }

        // ����Ű ��Ÿ�� �����ϱ� ���� ��ǲ�ʵ� ��� ��Ȱ��ȭ
        inputField.interactable = false;

        //print($"��ǲ �ʵ忡 �Է��� �ؽ�Ʈ : {inputText}");

        PhotonNetwork.NickName = inputText;

        JoinRoom();
    }

    // ��(����) ������ �õ���
    private void JoinRoom()
    {
        // ������ ���� ���ӵǾ��ִ���
        // ��ư�� ������ ������ �ѹ� �� Ȯ���� �غ���.
        if (PhotonNetwork.IsConnected)
        {
            // ����ڿ��� �뿡 �����Ұ����� �˷���
            connectInfoText.text = "�뿡 ������ �õ��ҰԿ�.";

            // ���� ������ ���� ������ �ƹ� ���̳� �����ϰ�,
            // ���� ���� ������ ���� ������ ���� ���� �����ϰ� �����ϼ���.
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions { MaxPlayers = 4 });
        }

        // ������ ������ ���� ��Ȳ
        else
        {
            // ������ ������ ���� ��Ȳ�� �˷���
            connectInfoText.text = "������ ������ ������.";

            // ���� ������ ��õ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // ���� ������ ���� �ְų�, �ƴϸ� ���� ���� ���� �Ϸ��ؼ�
    // �濡 �������� ��� �ڵ� ȣ��(����)
    public override void OnJoinedRoom()
    {
        connectInfoText.text = "�濡 �����߾��.\n���� ������ ��ȯ�ҰԿ�.";

        // ��Ʈ��ũ ��Ȳ�� �ƴҶ��� SceneManager.LoadScene() �Լ��� �̿��ؼ� ���� ��ȯ�� �� ����
        //SceneManager.LoadScene()

        // ��Ʈ��ũ ��Ȳ���� ���� ��ȯ�Ҷ��� PhotonNetwork.LoadLevel() �Լ��� �̿���
        PhotonNetwork.LoadLevel("Main");
    }
}
