using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Fruit : MonoBehaviour
{
    public GameObject Fruits;//과일 오브젝트
    public GameObject Sliced; // 잘리는 단면
    Rigidbody Fruit_Rigid; // 과일전체의 rigidbody
    Collider Fruit_Col; // 과일전체의 Collider
    public ParticleSystem particle;
    public float speed = 100;
    //float radian = 0.0f;
    public AudioSource Slice_Sound;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Fruit_Rigid = GetComponent<Rigidbody>();//초기화
        Fruit_Col = GetComponent<Collider>(); // 초기화
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,1,0)*Time.deltaTime*speed);
        //transform.Translate(new Vector3(0, Mathf.Sin(Time.deltaTime), 0));
        
        //radian += Time.deltaTime * Mathf.Deg2Rad;
        //transform.position += new Vector3(0, Mathf.Sin(radian), 0);
    }

    private void OnTriggerEnter(Collider other)//물체와 충돌이 일어났을 때
    {
        if (other.tag.Equals("Player")) // 대상의 tag가 플레이어라면
        {
            Mouse_Pointer Mouse_Pointer = other.GetComponent<Mouse_Pointer>();

            Fruit_Slice(Mouse_Pointer.Direction, Mouse_Pointer.transform.position, Mouse_Pointer.Slice_Force);
            if (Time.timeScale == 0)
                Time.timeScale = 1;
            GameObject.Find("GameManager").GetComponent<GameManager>().Delay_Start();

        }
    }

    void Fruit_Slice(Vector3 Direction, Vector3 position, float force)
    {
        //GameObject.Find("GameManager").GetComponent<GameManager>().Add_Score();
        Fruits.SetActive(false);
        Sliced.SetActive(true);

        Slice_Sound.Play();
        particle.Play();
        Fruit_Col.enabled = false;

        //float Fruit_x = position.x - Sliced.transform.position.x;// 마우스 좌표 - 과일 좌표
        //float Fruit_y = position.y - Sliced.transform.position.y;

        //float Degree = Mathf.Atan2(Fruit_y, Fruit_x) * Mathf.Rad2Deg;

        Sliced.transform.rotation = Quaternion.Euler(Random.Range(-15.0f, 15.0f), Random.Range(-15.0f, 15.0f), Random.Range(-90.0f, 90.0f));

        Rigidbody[] Slices = Sliced.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody Slice in Slices)
        {
            Slice.velocity = Fruit_Rigid.velocity;
            Slice.AddForceAtPosition(Direction * force, position, ForceMode.Impulse);
        }
    }
}
