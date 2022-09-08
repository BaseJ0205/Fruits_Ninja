using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject GamePause_Button;
    public GameObject GameReStart_Button;

    

    public GameObject GameLobby;
    public GameObject InGame;
    public GameObject GamePause;
    public GameObject GameEnd;
    public GameObject GameSetting_Panel;

    public GameObject Heart_1;
    public GameObject Heart_2;
    public GameObject Heart_3;

    public TextMeshProUGUI Score_Text;
    public TextMeshProUGUI BestScore_Text;
    public TextMeshProUGUI GameEnd_Score_Text;
    public TextMeshProUGUI GameEnd_BestScore_Text;
    public TextMeshProUGUI GamePause_Score_Text;
    public TextMeshProUGUI GamePause_BestScore_Text;

    public static int Hp = 3;
    public static int Score;
    public static int Best_Score;
    public GameObject[] Prefabs;//������ �����յ��� ������
    public GameObject Bomb;     //��ź
    public Transform Spawn_Point; //������ų ������ ������
    public BoxCollider box_Col; // ������ų������ �ݶ��̴��� ������
    private float delay = 3.0f; // �����Ҷ� ������ �ִ°�
    private float last_Time = 0;// ������ �ٶ� �ʿ��� ��
    private float Bomb_Delay = 3.0f;// ������ �ٶ� �ʿ��� ��
    public bool is_Start = false;
    public bool is_pause = false;
    public bool is_setting = false;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("BGM_Player").GetComponent<BGM>().GameLobby();
        is_Start = false;
        //GamePause_Button.SetActive(false);
        //GameReStart_Button.SetActive(false);

        GameLobby.SetActive(true);
        InGame.SetActive(false);
        GamePause.SetActive(false);
        GameEnd.SetActive(false);
        GameSetting_Panel.SetActive(false);

        box_Col = Spawn_Point.GetComponent<BoxCollider>(); // �ڽ� �ݶ��̴��� �޾ƿ�
        is_pause = false;
        is_setting = false;
    }

    // Update is called once per frame
    void Update()
    {
        Score_Text.text = "Score : " + Score;
        BestScore_Text.text = "BestScore : " + Best_Score;
        if (is_Start == true) {
            if (Time.time - last_Time >= delay)//����ð����� �۾��� �����ð��� ������ �����̺��� ũ�� �����Ŵ
            {
                delay = Random.Range(0.25f, 3.0f); // ó���ѹ����������� delay�� 0.25���� 3�ʻ��̷� �����ϰ� ����
                Spawn_Prefabs(); // �����ϴ� �Լ�
                last_Time = Time.time; // �۾������� �ð�����
            }

        }
        if (Hp <=0)
        {
            Heart_1.SetActive(false);
            Heart_2.SetActive(false);
            Heart_3.SetActive(false);
            Game_End();
        }
        if (Hp == 3)
        {
            Heart_1.SetActive(true);
            Heart_2.SetActive(true);
            Heart_3.SetActive(true);
        }
        else if(Hp == 2){
            Heart_1.SetActive(true);
            Heart_2.SetActive(true);
            Heart_3.SetActive(false);
        }
        else if(Hp == 1)
        {
            Heart_1.SetActive(true);
            Heart_2.SetActive(false);
            Heart_3.SetActive(false);
        }
    }

    void Spawn_Prefabs() // �����յ� ���������ִ� �Լ�
    {
        int a = Random.Range(0,Prefabs.Length); // 0���� ������ �ִ�ġ ������ ���ڹ޾ƿ�
        Vector3 Spawn_Position = new Vector3(Random.Range(box_Col.bounds.min.x+4, box_Col.bounds.max.x-5), Random.Range(box_Col.bounds.min.y, box_Col.bounds.max.y-3),Spawn_Point.position.z);
        //box_col.bounds.min�� max�� �ݶ��̴� �ڽ��� x�ּҰ��� �ִ밪 �� y���� �޾ƿ�������
        //�̵��� ������ ���̰����� �޾ƿͼ� ���������ǿ� ������

        Quaternion Fruit_rot = Quaternion.Euler(0, 0, Random.Range(-15.0f,15.0f));
        //z������ -15���� 15������ ������ ����

        GameObject Fruits = Instantiate(Prefabs[a], Spawn_Position, Fruit_rot);
        //������ �������� ���ӿ�����Ʈ�� ������, spawn_position��ġ�� �����ϰ� �����ϰ� ����� ȸ��ġ�� ������Ʈ�� ��������
        float force = Random.Range(17.0f,20.0f);

        Fruits.GetComponent<Rigidbody>().AddForce(Fruits.transform.up * force, ForceMode.Impulse);
        //Fruits ������Ʈ���� ����������
        //(Fruits������Ʈ�� local��ǥ��� ������ -15~15�� ���� �����ϰ� ���ư����� ����
        //�ش� ������Ʈ�� ���ʹ������� ���������ָ� �����ϰ� �������̵���)


        Fruits.transform.localScale += new Vector3(4.0f, 4.0f, 4.0f);
        //������������ �ʹ� �۾Ƽ� ������Ű���־����ϴ�

        Destroy(Fruits.gameObject, 5.0f);
        //�浹üũ�ϴ°ͺ��� 5�ʵڿ� ������ ������Ʈ�� �����ǵ��� �ϴ°��� ���ҽ��ϴ�.
    }
    void Spawn_Bomb() // �����յ� ���������ִ� �Լ�
    {
        Vector3 Spawn_Position = new Vector3(Random.Range(box_Col.bounds.min.x + 4, box_Col.bounds.max.x - 5), Random.Range(box_Col.bounds.min.y, box_Col.bounds.max.y - 3), Spawn_Point.position.z);
        //box_col.bounds.min�� max�� �ݶ��̴� �ڽ��� x�ּҰ��� �ִ밪 �� y���� �޾ƿ�������
        //�̵��� ������ ���̰����� �޾ƿͼ� ���������ǿ� ������

        Quaternion Bomb_rot = Quaternion.Euler(0, 0, Random.Range(-15.0f, 15.0f));
        //z������ -15���� 15������ ������ ����

        GameObject Bombs = Instantiate(Bomb, Spawn_Position,Bomb_rot);
        //������ �������� ���ӿ�����Ʈ�� ������, spawn_position��ġ�� �����ϰ� �����ϰ� ����� ȸ��ġ�� ������Ʈ�� ��������
        float force = Random.Range(17.0f, 20.0f);

        Bombs.GetComponent<Rigidbody>().AddForce(Bombs.transform.up * force, ForceMode.Impulse);
        //Fruits ������Ʈ���� ����������
        //(Fruits������Ʈ�� local��ǥ��� ������ -15~15�� ���� �����ϰ� ���ư����� ����
        //�ش� ������Ʈ�� ���ʹ������� ���������ָ� �����ϰ� �������̵���)


        Bombs.transform.localScale += new Vector3(2.0f, 2.0f, 2.0f);
        //������������ �ʹ� �۾Ƽ� ������Ű���־����ϴ�

        Destroy(Bombs.gameObject, 4.0f);
        //�浹üũ�ϴ°ͺ��� 4�ʵڿ� ������ ������Ʈ�� �����ǵ��� �ϴ°��� ���ҽ��ϴ�.
    }

    IEnumerator Spawn_Bomb_Courutine()
    {
        yield return new WaitForSeconds(Bomb_Delay);
        Spawn_Bomb();
        Bomb_Delay = Random.Range(4.0f,6.0f);
        yield return new WaitForSeconds(Bomb_Delay);
        StartCoroutine("Spawn_Bomb_Courutine");
    }




    public void  Reset_Score()
    {
        Score = 0;
    }
    public void Add_Score()
    {
        Score += 1;
        if(Score >= Best_Score)
            Best_Score = Score;
    }
    public void Game_Start()
    {
        Time.timeScale = 1;
        Load_Data();
        GameObject.Find("BGM_Player").GetComponent<BGM>().InGame();
        Hp = 3;
        StartCoroutine("Spawn_Bomb_Courutine");

        InGame.SetActive(true);
        GameLobby.SetActive(false);
        GameSetting_Panel.SetActive(false);
        is_Start = true;
    }
    public void Game_reStart()
    {
        Score = 0;
        Hp = 3;
        is_Start = true;
        if(Time.timeScale == 0)
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Game_End()
    {
        StopAllCoroutines();
        GameObject.Find("BGM_Player").GetComponent<BGM>().Game_Stop();
        is_Start = false;
        GameEnd.SetActive(true);
        InGame.SetActive(false);

        GameEnd_Score_Text.text = "Score : " + Score;
        GameEnd_BestScore_Text.text = "BestScore : " + Best_Score;
        Save_Data();
    }
    public void Game_Pause()
    {
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
        if (is_pause == false)
        {
            is_pause = true;
            GameObject.Find("BGM_Player").GetComponent<BGM>().GamePause();
            GamePause.SetActive(true);
            GamePause_Score_Text.text = "Score : " + Score;
            GamePause_BestScore_Text.text = "BestScore : " + Best_Score;
        }
        else
        {
            GameObject.Find("BGM_Player").GetComponent<BGM>().GameUnPause();
            GamePause.SetActive(false);
            is_pause = false;
        }
    }

    public void Game_Over()
    {
        GameObject.Find("BGM_Player").GetComponent<BGM>().Game_Stop();
        GameEnd.SetActive(true);
        Save_Data();
    }


    public void Game_Quit()
    {
        Application.Quit();
        Save_Data();
    }

    public void Decrease_Hp()
    {
        Hp--;
    }
    public void Delay_Start()
    {
        Invoke("Game_Start",1.5f);
    }


    public void Game_Setting()
    {
        if (is_setting == false)
        {
            is_setting = true;
            GameSetting_Panel.SetActive(true);
        }
        else
        {
            GameSetting_Panel.SetActive(false);
            is_setting = false;
        }
    }
    public void Save_Data()
    {
        PlayerPrefs.SetInt("BestScore", Best_Score);
    }
    public void Load_Data()
    {
        Best_Score = PlayerPrefs.GetInt("BestScore");
    }
    public void Reset_Data()
    {
        PlayerPrefs.DeleteKey("BestScore");
    }
}
