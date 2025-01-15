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
        // Join 버튼을 눌렀을때 OnJoinButtonPressed 함수를 호출
        joinButton.onClick.AddListener(OnJoinButtonPressed);

        // 닉네임을 입력하고 엔터키를 눌렀을때
        inputField.onSubmit.AddListener(OnEnterPressed);
    }

    private void Start()
    {
        // 입장 버튼을 잠시 비활성화
        joinButton.interactable = false;

        // 사용자에게 서버에 접속중이라는 사실을 알려줌
        connectInfoText.text = "서버에 접속중입니다...";

        // 포톤 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 서버 접속에 성공했을때 자동적으로 호출됨(실행)
    public override void OnConnectedToMaster()
    {
        // 비활성화 했던 버튼을 다시 활성화시켜줌.
        joinButton.interactable = true;

        // 서버 접속에 성공했다는 것을 사용자에게 알려줌.
        connectInfoText.text = "서버 접속에 성공했어요.\nJOIN 버튼을 눌러주세요.";
    }

    // 서버 접속에 실패했을 경우 자동적으로 호출됨(실행)
    public override void OnDisconnected(DisconnectCause cause)
    {
        // 버튼 잠시 비활성화
        joinButton.interactable = false;

        // 서버 접속에 실패했다는 것을 사용자에게 알려줌.
        connectInfoText.text = "서버 접속에 실패했어요.\n서버에 재접속 시도를 할게요.";
    }

    // Join 버튼을 눌렀을때 실행되는 함수
    private void OnJoinButtonPressed()
    {
        // 사용자가 닉네임을 입력하지 않았으면
        // 닉네임을 입력해 달라고 요청함
        if (string.IsNullOrEmpty(inputField.text))
        {
            connectInfoText.text = "닉네임을 입력해주세요";
            return;
        }
        else
        {
            string nickName = inputField.text;
            PhotonNetwork.NickName = nickName;
        }

        // 버튼 연타를 방지하기 위해 버튼을 잠시 비활성화
        joinButton.interactable = false;

        JoinRoom();
    }

    // 닉네임을 입력하고 엔터키를 눌렀을때 실행되는 함수
    private void OnEnterPressed(string inputText)
    {
        if (string.IsNullOrEmpty(inputText)) 
        {
            connectInfoText.text = "닉네임을 입력해주세요";
            return;
        }

        // 엔터키 연타를 방지하기 위해 인풋필드 잠시 비활성화
        inputField.interactable = false;

        //print($"인풋 필드에 입력한 텍스트 : {inputText}");

        PhotonNetwork.NickName = inputText;

        JoinRoom();
    }

    // 룸(세션) 입장을 시도함
    private void JoinRoom()
    {
        // 서버에 정말 접속되어있는지
        // 버튼을 누르는 시점에 한번 더 확인을 해본다.
        if (PhotonNetwork.IsConnected)
        {
            // 사용자에게 룸에 입장할것임을 알려줌
            connectInfoText.text = "룸에 입장을 시도할게요.";

            // 입장 가능한 방이 있으면 아무 방이나 입장하고,
            // 만약 입장 가능한 방이 없으면 직접 방을 생성하고 입장하세요.
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: new RoomOptions { MaxPlayers = 4 });
        }

        // 서버와 접속이 끊긴 상황
        else
        {
            // 서버와 접속이 끊긴 상황을 알려줌
            connectInfoText.text = "서버와 접속이 끊겼어요.";

            // 서버 접속을 재시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 참가 가능한 방이 있거나, 아니면 새로 방을 생성 완료해서
    // 방에 입장했을 경우 자동 호출(실행)
    public override void OnJoinedRoom()
    {
        connectInfoText.text = "방에 입장했어요.\n메인 씬으로 전환할게요.";

        // 네트워크 상황이 아닐때는 SceneManager.LoadScene() 함수를 이용해서 씬을 전환할 수 있음
        //SceneManager.LoadScene()

        // 네트워크 상황에서 씬을 전환할때는 PhotonNetwork.LoadLevel() 함수를 이용함
        PhotonNetwork.LoadLevel("Main");
    }
}
