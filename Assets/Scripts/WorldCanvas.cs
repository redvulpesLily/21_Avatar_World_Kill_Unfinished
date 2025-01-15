using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    Camera _mainCam;

    private void Awake()
    {
        // 씬에 존재하는 메인 카메라를 찾아서 _mainCam 변수에 할당해준다.
        // 단, 카메라 게임오브젝트의 Tag가 MainCamera로 되어있어야 한다.
        _mainCam = Camera.main;
    }

    private void Update()
    {
        // WorldCanvas가 매 프레임마다 카메라를 향하도록 한다.
        transform.LookAt(_mainCam.transform);

        // UI가 뒤집히지 않도록 180도 회전(Z축 반전)
        transform.rotation = Quaternion.LookRotation(transform.forward * -1f);
    }
}
