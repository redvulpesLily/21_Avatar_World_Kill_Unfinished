using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    [SerializeField] private InputField inputChat;

    private void Awake()
    {
        // ���� ���� ���۵ɶ� ��ǲ�ʵ带 ��Ȱ��ȭ�Ͽ�
        // ȭ�鿡 ��µ��� �ʰ� ��
        inputChat.gameObject.SetActive(false);
    }

    public void ToggleInputChat()
    {
        // ��ǲ�ʵ� ���ӿ�����Ʈ�� Ȱ��ȭ�Ǿ�������,
        if (inputChat.gameObject.activeSelf)
        {
            // ��ǲ�ʵ忡 �ؽ�Ʈ�� ����������(ä���� �ԷµǾ� ������)
            if (string.IsNullOrEmpty(inputChat.text) == false)
                ChatManager.Instance.Chat(inputChat.text);
            
            // �ƹ�Ÿ�� ������ �� �ְ�
            GameManager.Instance.MyAvatar.SetMovable(true);

            inputChat.text = null;
            inputChat.gameObject.SetActive(false);
        }
        // ��ǲ�ʵ� ���ӿ�����Ʈ�� ��Ȱ��ȭ �Ǿ�������,
        else
        {
            inputChat.gameObject.SetActive(true);
            
            // �ƹ�Ÿ �������� ���ϰ�
            GameManager.Instance.MyAvatar.SetMovable(false);
            
            // ��ǲ�ʵ忡 ��Ŀ���� �� (�ٷ� �ؽ�Ʈ�� �Է��� �� �ְ�)
            inputChat.Select();
            inputChat.ActivateInputField();
        }
    }
}
