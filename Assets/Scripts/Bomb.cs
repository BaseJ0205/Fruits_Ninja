using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public AudioSource bomb_Sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)//��ü�� �浹�� �Ͼ�� ��
    {
        
        if (other.tag.Equals("Player")) // ����� tag�� �÷��̾���
        {
            this.GetComponent<Collider>().enabled = false;
            Time.timeScale = 0.5f;
            Invoke("Explode_Bomb",1.5f);
        }
    }

    public void Explode_Bomb()
    {
       
        Time.timeScale = 1;
        bomb_Sound.Play();
        GameObject.Find("Mouse_Point").GetComponent<Mouse_Pointer>().Flash_on();
        GameObject.Find("GameManager").GetComponent<GameManager>().Game_End();

    }



}
