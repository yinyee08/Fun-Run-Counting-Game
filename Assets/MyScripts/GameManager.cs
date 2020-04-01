using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{  
    // #region singleton
    // public static GameManager instance;

    // private void Awake()
    // {
    //     if(instance != null)
    //     {
    //         Debug.LogError("More than one GameManager in the scene");
    //     }
    //     else
    //     {
    //         instance = this;
    //     }
    // }
    // #endregion

    public GameObject ControllerKinect;
    public Text count_text;
	public Text hit_count_text;
    public int count_object;
    public static int hit_count_object;
    public GameObject[] fruitsType;
    public GameObject[] obstacles;
    int objectToSpawn;
    int countObjectToSpawn;
    public bool newRound = true;
    float[] positionX = {-2.2f, -1.1f, 1.1f, 2.2f};
    static float positionY = -10f;
    List<GameObject> fruits;
    //public GameObject WinMenu;
    public AudioClip[] numberingSound;
    public AudioClip hitSound;
    private static int counting = 0;
    public float spawnRate = 2f;
    float nextSpawn = 1f;
    public GameObject[] lives;
    public static int livesCount;
    public Leaderboard leaderboardUI;
    public bool gameOver = false;
    public static bool gameStart = false;
    public Text game_level_text;
    public static int gameLevel;
    public GameObject game_end_menu;
    public GameObject main_canvas;
    public Text game_end_score;
    public static bool playHit = false;
    public RectTransform stars_list;
    

    void Start(){
        //count_object = Random.Range(1,11);
        count_object = 0;
        // count_text.text = count_object.ToString();
        livesCount = 3;

        hit_count_object = 0;
        hit_count_text.text = hit_count_object.ToString();
        countObjectToSpawn = 0;
        fruits = new List<GameObject>();

        gameLevel = 1;
        game_level_text.text = gameLevel.ToString();
    }

    void Update(){
        if(!gameOver){

            switch(livesCount)
            {
                case 0: lives[0].GetComponent<Image>().color = Color.black;
                        break;
                case 1: lives[1].GetComponent<Image>().color = Color.black;
                        break;
                case 2: lives[2].GetComponent<Image>().color = Color.black;
                        break;
                default: break;        

            }

            if(playHit)
            {
                AudioSource audio;
                audio = GetComponent<AudioSource>();
                audio.clip = hitSound;
                audio.Play();
                playHit = false;
            }
            /*if(livesCount == 0)
            {
                //Time.timeScale = 0f;
                game_end_score.text = hit_count_object.ToString();
                EndGame();
                gameOver = true;
            }*/

            if(newRound){
                count_object = Random.Range(5,11);
                count_text.text = count_object.ToString();
                Debug.Log("Count Object: " + count_object);
                newRound = false;
            }

            if(gameStart){
                if(Time.time > nextSpawn)
                {
                    Instantiate(obstacles[0], new Vector3(0.6f,8.7f,-10f), Quaternion.Euler(18, 181, -11));
                    nextSpawn = Time.time + spawnRate;
                }


                if(hit_count_object <= count_object){
                    
                    if(hit_count_object == counting+1) {
                        AudioSource audio;
                        audio = GetComponent<AudioSource>();
                        audio.clip = numberingSound[counting];
                        audio.Play();
                        counting++;
                    
                    }

                    hit_count_text.text = hit_count_object.ToString();

                    if(hit_count_object == count_object || livesCount == 0){
                        //Time.timeScale = 0f;
                        game_end_score.text = hit_count_object.ToString();
                        if(hit_count_object != count_object){
                            stars_list.GetChild(2).GetComponent<Image>().color = new Color(0.509f,0.509f,0.509f);

                            if(hit_count_object < count_object -2)
                                stars_list.GetChild(1).GetComponent<Image>().color = new Color(0.509f,0.509f,0.509f);

                        }
                        EndGame();
                        gameOver = true;
                        gameLevel ++;

                    }
                }

                
                if(livesCount == 0 || hit_count_object != count_object) { //Time.time > 
                    int randomPositionX = Random.Range(0, 4);
                    positionY += Random.Range(10f,15f);
                    objectToSpawn =  Random.Range(0,fruitsType.Length);
                    fruits.Add(Instantiate(fruitsType[objectToSpawn], new Vector3(positionX[randomPositionX], 5.4f, positionY), Quaternion.identity));
                    countObjectToSpawn ++;
                }

                foreach(GameObject fruit in fruits){
                    fruit.transform.Translate(0,0,-5.5f*Time.deltaTime);
                }
            }
        }
        
    }

    void EndGame(){
    
        leaderboardUI.SaveGame(hit_count_object);
        //WinMenu.SetActive(true);
        main_canvas.SetActive(false);
        game_end_menu.SetActive(true);
        //ControllerKinect.GetComponent<SimpleGestureListener>().enabled = false;
        ControllerKinect.GetComponent<InteractionManager>().enabled = true;
    }
}