using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mouse_Pointer : MonoBehaviour
{
    public GameObject Flash; // ����
    public bool is_Drag = false; // �巡�� �������� üũ
    public SphereCollider Mouse_Col; // ���콺 �浹
    public Vector3 MousePosition; // ���콺���� ��ġ
    public Camera mouse_Camera; // ī�޶� �޾ƿ���
    public TrailRenderer mouse_Trail; // ���콺 Ʈ����
    public Vector3 Direction { get; private set; } // ������ ���� ����
    float Min_Velocity = 0.01f;
    public float Slice_Force = 5.0f;

    private void Awake()
    {
        Mouse_Col = GetComponent<SphereCollider>(); // ���Ǿ� �ݶ��̴� �޾ƿ���
        mouse_Camera = Camera.main; // ���콺 ī�޶� ���� ī�޶� �ֱ�
        mouse_Trail = GetComponentInChildren<TrailRenderer>(); // �ڽ����κ��� Ʈ���� ������ �޾ƿ�
        
        is_Drag = false; // �巡�� �ȵȴ�
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButtonDown(0))// ���콺 Ű�� ������ ��
        {
            Start_Mouse();//���콺 �����Լ� ����
        }
        else if (Input.GetMouseButtonUp(0))// ���콺 Ű�� ������ ��
        {
            Stop_Mouse();//���콺 �����Լ� ����
        }
        else if (is_Drag)
        {
            Mouse_Drag();//�巡�װ� true�϶�  Drag�Լ� ����
        }
    }


    private void OnEnable()//����ó��
    {
        Stop_Mouse();
    }




    void Mouse_Drag()//���콺 �巡�� �Լ�
    {
        MousePosition = mouse_Camera.ScreenToWorldPoint(Input.mousePosition);
        //����ī�޶�ȭ�鿡�� ���콺�� �Է°��� ���尪���� ��ȯ�Ͽ� ���콺 �����ǿ� ���� 
        
        MousePosition.z = 0.0f;
        //z������ �޾ƿ��� ī�޶��� z��ġ�� ���콺�� ���ܼ� ���콺�� �Ⱥ��̹Ƿ� ����


        Direction = MousePosition - transform.position; // ���� - ���������� ������ ������ ����

        float velocity = Direction.magnitude / Time.deltaTime; // ������ ũ�⸦ �ð����� ������ �ӵ��� ����

        if (velocity < Min_Velocity)
        {
            Mouse_Col.enabled = false;
        }
        else
        {
            Mouse_Col.enabled = true;
        }

        transform.position = MousePosition;
        //���콺 �����ӿ� ���� ���� ��ũ��Ʈ�� �� ������Ʈ�� �̵�������




    }
    void Start_Mouse()//���콺 ���� �Լ�
    {
        mouse_Trail.enabled = true; // ���콺 Ʈ����(���콺 ��) Ȱ��ȭ ��Ŵ
        is_Drag = true;             //�巡�� �����ϴٰ� �˸�
        Mouse_Col.enabled = true;   //���� ������Ʈ�� �浹�� �����ϰ���


        MousePosition = mouse_Camera.ScreenToWorldPoint(Input.mousePosition);
        //����ī�޶�ȭ�鿡�� ���콺�� �Է°��� ���尪���� ��ȯ�Ͽ� ���콺 �����ǿ� ���� 
        MousePosition.z = 0.0f;
        //z������ �޾ƿ��� ī�޶��� z��ġ�� ���콺�� ���ܼ� ���콺�� �Ⱥ��̹Ƿ� ����
        transform.position = MousePosition;
        //���콺 �����ӿ� ���� ���� ��ũ��Ʈ�� �� ������Ʈ�� �̵�������
    }
    void Stop_Mouse()//���콺 �����
    {
        mouse_Trail.enabled = false;//���콺 Ʈ���� ����
        mouse_Trail.Clear();        //������ �׷����ִ� Ʈ���� ����
        is_Drag = false;            //�巡�� �׸��մϴ� ����
        Mouse_Col.enabled = false;  //���콺 �ݶ��̴� �������
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
