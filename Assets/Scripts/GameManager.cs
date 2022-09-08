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
    public GameObject[] Prefabs;//생성할 프리팹들을 저장함
    public GameObject Bomb;     //폭탄
    public Transform Spawn_Point; //스폰시킬 지점을 저장함
    public BoxCollider box_Col; // 스폰시킬지점의 콜라이더를 가져옴
    private float delay = 3.0f; // 시작할때 딜레이 주는것
    private float last_Time = 0;// 딜레이 줄때 필요한 덤
    private float Bomb_Delay = 3.0f;// 딜레이 줄때 필요한 덤
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

        box_Col = Spawn_Point.GetComponent<BoxCollider>(); // 박스 콜라이더를 받아옴
        is_pause = false;
        is_setting = false;
    }

    // Update is called once per frame
    void Update()
    {
        Score_Text.text = "Score : " + Score;
        BestScore_Text.text = "BestScore : " + Best_Score;
        if (is_Start == true) {
            if (Time.time - last_Time >= delay)//현재시간에서 작업이 끝난시간을 뺀값이 딜레이보다 크면 실행시킴
            {
                delay = Random.Range(0.25f, 3.0f); // 처음한번실행됬을때 delay를 0.25에서 3초사이로 랜덤하게 적용
                Spawn_Prefabs(); // 스폰하는 함수
                last_Time = Time.time; // 작업끝날때 시간저장
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

    void Spawn_Prefabs() // 프리팹들 스폰시켜주는 함수
    {
        int a = Random.Range(0,Prefabs.Length); // 0부터 프리팹 최대치 차이의 숫자받아옴
        Vector3 Spawn_Position = new Vector3(Random.Range(box_Col.bounds.min.x+4, box_Col.bounds.max.x-5), Random.Range(box_Col.bounds.min.y, box_Col.bounds.max.y-3),Spawn_Point.position.z);
        //box_col.bounds.min과 max는 콜라이더 박스의 x최소값과 최대값 및 y값을 받아오기위함
        //이들을 랜덤한 사이값으로 받아와서 스폰포지션에 저장함

        Quaternion Fruit_rot = Quaternion.Euler(0, 0, Random.Range(-15.0f,15.0f));
        //z축으로 -15에서 15도까지 각도를 돌림

        GameObject Fruits = Instantiate(Prefabs[a], Spawn_Position, Fruit_rot);
        //생성할 프리팹을 게임오브젝트로 저장함, spawn_position위치를 저장하고 랜덤하게 저장된 회전치를 오브젝트에 적용해줌
        float force = Random.Range(17.0f,20.0f);

        Fruits.GetComponent<Rigidbody>().AddForce(Fruits.transform.up * force, ForceMode.Impulse);
        //Fruits 오브젝트에서 힘을가해줌
        //(Fruits오브젝트는 local좌표계로 봤을때 -15~15도 까지 랜덤하게 돌아가있음 따라서
        //해당 오브젝트의 위쪽방향으로 힘을가해주면 랜덤하게 포물선이동함)


        Fruits.transform.localScale += new Vector3(4.0f, 4.0f, 4.0f);
        //과일프리팹이 너무 작아서 스케일키워주었습니다

        Destroy(Fruits.gameObject, 5.0f);
        //충돌체크하는것보다 5초뒤에 스스로 오브젝트가 삭제되도록 하는것이 좋았습니다.
    }
    void Spawn_Bomb() // 프리팹들 스폰시켜주는 함수
    {
        Vector3 Spawn_Position = new Vector3(Random.Range(box_Col.bounds.min.x + 4, box_Col.bounds.max.x - 5), Random.Range(box_Col.bounds.min.y, box_Col.bounds.max.y - 3), Spawn_Point.position.z);
        //box_col.bounds.min과 max는 콜라이더 박스의 x최소값과 최대값 및 y값을 받아오기위함
        //이들을 랜덤한 사이값으로 받아와서 스폰포지션에 저장함

        Quaternion Bomb_rot = Quaternion.Euler(0, 0, Random.Range(-15.0f, 15.0f));
        //z축으로 -15에서 15도까지 각도를 돌림

        GameObject Bombs = Instantiate(Bomb, Spawn_Position,Bomb_rot);
        //생성할 프리팹을 게임오브젝트로 저장함, spawn_position위치를 저장하고 랜덤하게 저장된 회전치를 오브젝트에 적용해줌
        float force = Random.Range(17.0f, 20.0f);

        Bombs.GetComponent<Rigidbody>().AddForce(Bombs.transform.up * force, ForceMode.Impulse);
        //Fruits 오브젝트에서 힘을가해줌
        //(Fruits오브젝트는 local좌표계로 봤을때 -15~15도 까지 랜덤하게 돌아가있음 따라서
        //해당 오브젝트의 위쪽방향으로 힘을가해주면 랜덤하게 포물선이동함)


        Bombs.transform.localScale += new Vector3(2.0f, 2.0f, 2.0f);
        //과일프리팹이 너무 작아서 스케일키워주었습니다

        Destroy(Bombs.gameObject, 4.0f);
        //충돌체크하는것보다 4초뒤에 스스로 오브젝트가 삭제되도록 하는것이 좋았습니다.
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
