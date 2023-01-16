using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //single instance of game manager that can be accessed anyware
    //publicly gettable and privately setter
    // only waant this class to set it
    public static GameManager Instance { get; private set; }



    public int world { get; private set; }
    public int stage { get; private set; }

    public int lives { get; private set; }
    public int coins { get; private set; }


    private void Awake()
    {
        //check to see  if there is an instance already available
        //if there is another one we need to destroy this one
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        } else
        //if there isnt we will make this one 'this'
        // if we make one we need to make sure it is not destroyed on load
        {
            Instance = this;
            //this will maintain the game manager across all levels
            DontDestroyOnLoad(gameObject);
        }
 
    }
    //we need to destroy this instance 
    //if our game manager is being destroyed then we null it out
    //so next time the game manager is created it will reassign itself 
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // logic for game state


    //start is a unity funcition, ran first frame game manager is enabled
    private void Start()
    {
        //we can set the game framerate here
        Application.targetFrameRate = 60;
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        //reset coins
        coins = 0;
        LoadLevel(1, 1);
    }


    private void LoadLevel(int world,int stage)
    {
        this.world = world;
        this.stage = stage;


        //using string interpelation along with UnityEngine.SceneManagment Library
        //we will name all scenes with respect to thier level and world name
        //make sure the build setttings has alll scenes loaded into

        SceneManager.LoadScene($"{world}-{stage}");
    }

    public void NextLevel(int world,int stage)
    {
        //here is where we would also determin if reached the end of a world and incrament it
        LoadLevel(world, stage + 1);
    }


    //overloading to slow down the reset level
    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay); 
    }

    public void ResetLevel()
    {
        lives--;

        if (lives > 0)
        {
            LoadLevel(world, stage);
        }
        else
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        NewGame();
        // if we wanted to wait for 3 seconds to start new game "Invoke(nameof(NewGame), 3f);"
    }
       
    //logic for coins being gathered, needs to be public so blockcoin canc all
    public void AddCoin()
    {
        coins++;


        //need life goin on 100 coins
        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
    }

    //logic for adding another life ie. 100 coins and 1up mushroom
    public void AddLife()
    {
        lives++;

    }



}
