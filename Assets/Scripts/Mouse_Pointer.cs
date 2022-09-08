using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse_Pointer : MonoBehaviour
{
    public GameObject Flash; // 섬광
    public bool is_Drag = false; // 드래그 가능한지 체크
    public SphereCollider Mouse_Col; // 마우스 충돌
    public Vector3 MousePosition; // 마우스현재 위치
    public Camera mouse_Camera; // 카메라 받아오기
    public TrailRenderer mouse_Trail; // 마우스 트레일
    public Vector3 Direction { get; private set; } // 벡터의 방향 저장
    float Min_Velocity = 0.01f;
    public float Slice_Force = 5.0f;

    private void Awake()
    {
        Mouse_Col = GetComponent<SphereCollider>(); // 스피어 콜라이더 받아오기
        mouse_Camera = Camera.main; // 마우스 카메라에 메인 카메라 넣기
        mouse_Trail = GetComponentInChildren<TrailRenderer>(); // 자식으로부터 트레일 렌더러 받아옴
        
        is_Drag = false; // 드래그 안된다
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))// 마우스 키가 눌렸을 때
        {
            Start_Mouse();//마우스 시작함수 실행
        }
        else if (Input.GetMouseButtonUp(0))// 마우스 키가 때졌을 때
        {
            Stop_Mouse();//마우스 종료함수 실행
        }
        else if (is_Drag)
        {
            Mouse_Drag();//드래그가 true일때  Drag함수 실행
        }
    }


    private void OnEnable()//예외처리
    {
        Stop_Mouse();
    }




    void Mouse_Drag()//마우스 드래그 함수
    {
        MousePosition = mouse_Camera.ScreenToWorldPoint(Input.mousePosition);
        //메인카메라화면에서 마우스의 입력값을 월드값으로 변환하여 마우스 포지션에 저장 
        
        MousePosition.z = 0.0f;
        //z값까지 받아오면 카메라의 z위치에 마우스가 생겨서 마우스가 안보이므로 수정


        Direction = MousePosition - transform.position; // 끝점 - 시작점으로 벡터의 방향을 저장

        float velocity = Direction.magnitude / Time.deltaTime; // 벡터의 크기를 시간으로 나누어 속도를 구함

        if (velocity < Min_Velocity)
        {
            Mouse_Col.enabled = false;
        }
        else
        {
            Mouse_Col.enabled = true;
        }

        transform.position = MousePosition;
        //마우스 움직임에 따라서 현재 스크립트가 들어간 오브젝트를 이동시켜줌




    }
    void Start_Mouse()//마우스 시작 함수
    {
        mouse_Trail.enabled = true; // 마우스 트레일(마우스 선) 활성화 시킴
        is_Drag = true;             //드래그 가능하다고 알림
        Mouse_Col.enabled = true;   //현재 오브잭트의 충돌이 가능하게함


        MousePosition = mouse_Camera.ScreenToWorldPoint(Input.mousePosition);
        //메인카메라화면에서 마우스의 입력값을 월드값으로 변환하여 마우스 포지션에 저장 
        MousePosition.z = 0.0f;
        //z값까지 받아오면 카메라의 z위치에 마우스가 생겨서 마우스가 안보이므로 수정
        transform.position = MousePosition;
        //마우스 움직임에 따라서 현재 스크립트가 들어간 오브젝트를 이동시켜줌
    }
    void Stop_Mouse()//마우스 종료시
    {
        mouse_Trail.enabled = false;//마우스 트레일 종료
        mouse_Trail.Clear();        //이전에 그려져있던 트레일 삭제
        is_Drag = false;            //드래그 그만합니다 전달
        Mouse_Col.enabled = false;  //마우스 콜라이더 사용중지
    }

    public void Flash_on()
    {
        
        if (Flash.activeSelf == false)
        {
            Flash.SetActive(true);
            Invoke("Flash_on", 0.1f);
            
        }
        else
        {
            Color Flash_Color =  Flash.GetComponent<Image>().color;
            Flash_Color.a -= 0.1f;
            Flash.GetComponent<Image>().color = Flash_Color;
            if (Flash_Color.a >= 0) { 
                Invoke("Flash_on", 0.1f);
            }
            else
            {
                Flash.SetActive(false);
                //Stop_Mouse();
            }
        }


    }
}
