using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject Fruits;//���� ������Ʈ
    public GameObject Sliced; // �߸��� �ܸ�
    Rigidbody Fruit_Rigid; // ������ü�� rigidbody
    Collider Fruit_Col; // ������ü�� Collider
    public ParticleSystem particle;

    public AudioSource Slice_Sound;

    private void Awake()
    {
        //particle = GetComponent<ParticleSystem>();
        particle = GetComponentInChildren<ParticleSystem>();
    }
    void Start()
    {
        Fruit_Rigid = GetComponent<Rigidbody>();//�ʱ�ȭ
        Fruit_Col = GetComponent<Collider>(); // �ʱ�ȭ
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)//��ü�� �浹�� �Ͼ�� ��
    {
        if (other.tag.Equals("Player")) // ����� tag�� �÷��̾���
        {
            Mouse_Pointer Mouse_Pointer = other.GetComponent<Mouse_Pointer>();

            Fruit_Slice(Mouse_Pointer.Direction, Mouse_Pointer.transform.position, Mouse_Pointer.Slice_Force);

        }
        else if (other.tag.Equals("Enemy"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Decrease_Hp();
            Destroy(gameObject);
        }
    }

    void Fruit_Slice(Vector3 Direction, Vector3 position, float force)
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Add_Score();
        Fruits.SetActive(false);
        Sliced.SetActive(true);
        Slice_Sound.Play();
        particle.Play();
        Fruit_Col.enabled = false;

        //float Fruit_x = position.x - Sliced.transform.position.x;// ���콺 ��ǥ - ���� ��ǥ
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
