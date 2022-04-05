using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public static PlayerController control;

    Rigidbody2D self_Rigidbody;
    public float test_thrust = 10000f;
    public float speedInUnitPerSecond;
    public float falling_threshold = -5f;
    public bool Falling = false;
    public float flying_threshold = 5f;
    public bool Flying = false;

    // sound
    public AudioSource audioSource;
    public float volume=0.8f;
    // load sounds
    public AudioClip jump_audio;
    public AudioClip land_audio;
    public AudioClip throw_audio;
    public AudioClip clanking_audio;
    
    // Flight Info
    public float current_height = 0f;

    // Time and Space
    public float top_time = 0f;
    public bool can_jump = true;
    public float time_in_air = 0f;

    // Game Score Vars
    public int miles = 0; // 62 miles max
    

    // UI Things
    public Text miles_text;
    public Text time_in_air_text;
    public Text top_time_in_air_text;

    // Thrust Values
    public float thrust_speed= 55f;
    public float thrust_juice = 55f;
    // Upgrades
    public float thrust_level_1= 55f;
    public float thrust_level_2= 55f;
    public float thrust_level_3= 55f;

    // Player Body
    public GameObject player_body;

    // Player States
    public GameObject standing_still;
    public GameObject flying_in_air;

    // Add Ons
    public GameObject umbrella_asset;

    // Possible things to place
    public GameObject place_baloon;
    public GameObject place_windmill;

    // test
    public Vector2 new_vector;



    void Awake () {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        } else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        self_Rigidbody = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(0.0f, -2.0f, 0.0f);
        // umbrella_asset = GameObject.Find("umbrella_asset");
        umbrella_asset.SetActive(false);
        flying_in_air.SetActive(false);
        standing_still.SetActive(true);
        miles_text.text = miles.ToString();
    }

    void Update(){

                if (self_Rigidbody.velocity.y == 0)
        {
            standing_still.SetActive(true);
            umbrella_asset.SetActive(false);
            flying_in_air.SetActive(false);
        }
        
        if (Input.GetKeyDown("q"))
        {
            PlaceBaloon();
        }      

        if (Input.GetKeyDown("e"))
        {
            PlaceWindmill();
        }              

        if (Input.GetKeyDown("space"))
        {
            if (can_jump == true)
            {
                if (top_time > 45)
                {
                    LevelOneLaunch();
                    audioSource.PlayOneShot(jump_audio, volume);

                } else if (top_time > 130)
                {
                    LevelTwoLaunch();
                    audioSource.PlayOneShot(jump_audio, volume);
                } else if (top_time > 220)
                {
                   LevelThreeLaunch(); 
                   audioSource.PlayOneShot(jump_audio, volume);
                } else
                {
                    TestLaunch();
                    audioSource.PlayOneShot(jump_audio, volume);
                }
                
            }
        }

        if (Input.GetKeyDown("a"))
        {
            ThrusterLeft();
            PlayerLeft();
        }

        if (Input.GetKeyUp("a")) {
            PlayerLeftReset();
        }


        if (Input.GetKeyDown("d"))
        {
            ThrusterRight();
            PlayerRight();
        }   

        if (Input.GetKeyUp("d")) {
            PlayerRightReset();
        }

                          
                              

        if (self_Rigidbody.velocity.y < falling_threshold)
        {
            Falling = true;
        }
        else
        {
            Falling = false;
        }

        if (self_Rigidbody.velocity.y > flying_threshold)
        {
            Flying = true;
        }
        else
        {
            Flying = false;
        }        
    
        if (Falling)
        {
            umbrella_asset.SetActive(true);
            flying_in_air.SetActive(true);
            standing_still.SetActive(false);
        }

        if (Flying)
        {
            flying_in_air.SetActive(true);
            umbrella_asset.SetActive(false);
            standing_still.SetActive(false);
        }

        if (Flying == false & Falling == false){
            standing_still.SetActive(true);
            flying_in_air.SetActive(false);
            umbrella_asset.SetActive(false);            
        }

        if (this.transform.position.y > 10f)
        {
            current_height = this.transform.position.y;
            miles_text.text = current_height.ToString("0");
            time_in_air += Time.deltaTime;
            time_in_air_text.text = time_in_air.ToString("0");
            can_jump = false;
        }

        if (this.transform.position.y < 9f)
        {
            if (top_time < time_in_air)
            {
                top_time = time_in_air;
                top_time_in_air_text.text = top_time.ToString("0");
            }

            time_in_air = 0f;
            if (can_jump == false)
            {
                audioSource.PlayOneShot(land_audio, volume);
                can_jump = true;
            }
            
            can_jump = true;
        }

               
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
   
    }

    // TODO: Want to do a "pop up" boost function we can call from other scripts


    public void TestLaunch(){
            self_Rigidbody.AddForce(transform.up * test_thrust, ForceMode2D.Impulse);
    }

    public void LevelOneLaunch(){
            self_Rigidbody.AddForce(transform.up * thrust_level_1, ForceMode2D.Impulse);
    }    

    public void LevelTwoLaunch(){
            self_Rigidbody.AddForce(transform.up * thrust_level_2, ForceMode2D.Impulse);
    }    

    public void LevelThreeLaunch(){
            self_Rigidbody.AddForce(transform.up * thrust_level_3, ForceMode2D.Impulse);
    }            

    void PlayerLeft(){
        player_body.transform.Rotate(0,0,30f);
    }
    void PlayerLeftReset(){
        player_body.transform.Rotate(0,0,-30f);
    }


    void PlayerRight(){
        player_body.transform.Rotate(0,0,-30f);
    }
    void PlayerRightReset(){
        player_body.transform.Rotate(0,0,30f);
    }    


    void ThrusterLeft(){
        if (thrust_juice > 0){
            self_Rigidbody.AddForce(Vector2.left * thrust_juice, ForceMode2D.Impulse);
        }
        
    }

    void ThrusterRight(){
        self_Rigidbody.AddForce(Vector2.right * thrust_juice, ForceMode2D.Impulse);
    }        

    public void UpdateMiles(int new_miles){
        miles = new_miles;
        miles_text.text = miles.ToString();
    }


    public void PlaceBaloon()
    {   
        if (Flying == true)
        {
            var trans_x = transform.position.x;
            var trans_y = transform.position.y;
            var tranz_new_y = trans_y - 3f;
            new_vector = new Vector2(trans_x, tranz_new_y);
            Instantiate(place_baloon, new_vector, Quaternion.identity);
            audioSource.PlayOneShot(throw_audio, volume);            
        }        

    }

    public void PlaceWindmill()
    {   
        if (Flying == true)
        {
            var trans_x = transform.position.x;
            var trans_y = transform.position.y;
            var tranz_new_y = trans_y - 3f;
            new_vector = new Vector2(trans_x, tranz_new_y);
            Instantiate(place_windmill, new_vector, Quaternion.identity);
            audioSource.PlayOneShot(throw_audio, volume);              
        }

    }

    public void PlayClanking()
    {
        audioSource.PlayOneShot(clanking_audio, volume);
    }







    // EOL

}
