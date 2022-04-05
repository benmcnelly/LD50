using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoreLogic : MonoBehaviour
{

    public static GameCoreLogic _game_logic;

    // setup
    public AudioSource audioSource;
    public float volume=0.8f;

    // Audio Links
    public AudioClip jump_audio;
    public AudioClip land_audio;
    public AudioClip launch_audio;
    public AudioClip clanking_audio;


    // Start is called before the first frame update
    void Start()
    {
        if (_game_logic == null)
        {
            DontDestroyOnLoad(gameObject);
            _game_logic = this;
        } else if (_game_logic != this)
        {
            Destroy(_game_logic);
        }	        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioJump()
    {
        audioSource.PlayOneShot(jump_audio, volume);
    }

    public void PlayAudioClanking()
    {
        audioSource.PlayOneShot(clanking_audio, volume);
    }    


}
