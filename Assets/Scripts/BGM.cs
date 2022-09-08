using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioSource Game_Audio;
    public AudioClip Lobby_BGM_Clip;
    public AudioClip InGame_Bgm_Clip;


    // Start is called before the first frame update
    void Start()
    {
        Game_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GamePause()
    {
        Game_Audio.Pause();
    }
    public void GameUnPause()
    {
        Game_Audio.UnPause();
    }
    public void GameLobby()
    {
        Game_Audio.clip = Lobby_BGM_Clip;
        Game_Audio.Play();
        Game_Audio.loop = true;
    }
    public void InGame()
    {
        Game_Audio.clip = InGame_Bgm_Clip;
        Game_Audio.Play();
        Game_Audio.loop = true;
    }
    public void Game_Stop()
    {
        Game_Audio.Stop();
    }


}
