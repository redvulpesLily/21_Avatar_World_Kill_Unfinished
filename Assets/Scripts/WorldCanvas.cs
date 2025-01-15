using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    Camera _mainCam;

    private void Awake()
    {
        // ���� �����ϴ� ���� ī�޶� ã�Ƽ� _mainCam ������ �Ҵ����ش�.
        // ��, ī�޶� ���ӿ�����Ʈ�� Tag�� MainCamera�� �Ǿ��־�� �Ѵ�.
        _mainCam = Camera.main;
    }

    private void Update()
    {
        // WorldCanvas�� �� �����Ӹ��� ī�޶� ���ϵ��� �Ѵ�.
        transform.LookAt(_mainCam.transform);

        // UI�� �������� �ʵ��� 180�� ȸ��(Z�� ����)
        transform.rotation = Quaternion.LookRotation(transform.forward * -1f);
    }
}
