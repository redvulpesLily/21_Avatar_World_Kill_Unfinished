using Photon.Pun;
using UnityEngine;

public class ChatManager : MonoBehaviourPun
{
    #region Singleton
    private static ChatManager _instance;

    public static ChatManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ChatManager>();
            return _instance;
        }
    }
    #endregion
    
    [SerializeField] private UIChat uiChat;

    public void Chat(string chat)
    {
        var actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        var nickName = PhotonNetwork.NickName;
        photonView.RPC(nameof(SendChat), RpcTarget.All, actorNumber, nickName, chat);
    }

    [PunRPC]
    private void SendChat(int actorNumber, string nickName, string chat)
    {
        var allAvatars = FindObjectsByType<PlayerAvatar>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        PlayerAvatar targetAvatar = null;
        
        foreach (var avatar in allAvatars)
        {
            if (avatar.IsTargetAvatar(actorNumber))
            {
                targetAvatar = avatar;
                break;
            }
        }

        if (targetAvatar == null)
            return;
        
        targetAvatar.ShowChat(chat);
    }

    private void Update()
    {
        // 엔터키 입력을 감지해서
        // uiChat의 ToggleInputChat() 함수 호출
        if (Input.GetKeyDown(KeyCode.Return))
        {
            uiChat.ToggleInputChat();
        }
    }
}