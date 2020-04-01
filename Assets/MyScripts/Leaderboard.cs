using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public struct Highscore{
    public string name;
    public int score;
}

public class Leaderboard : MonoBehaviour{
    
    public List<Highscore> highscores;
    public RectTransform entriesRoot;

    public Leaderboard()
    {
        highscores = new List<Highscore>();

        string saveFile = "Assets/Resources/leaderboard_scores.txt"; 

        if(File.Exists(saveFile))
            Read();
    }

    public void SaveGame(int score)
    {
        Highscore entry = new Highscore();
        if(PlayerPrefs.GetString("PlayerName") == "")
            PlayerPrefs.SetString("PlayerName", "James");
        
        entry.name = PlayerPrefs.GetString("PlayerName", "James");
        entry.score = score;

        // if(highscores.Count == 0){
        //     highscores.Add(entry);
        //     Save();
        // }else{
            CallSort(entry.name, score);
            Save();
        //}
    }

    void CallSort(string name, int score)
    {
        bool enter = false;
        Highscore newEntry = new Highscore();
        newEntry.name = name;
        newEntry.score = score;
        
        for(int i=0; i<highscores.Count; i++){
            if(score >= highscores[i].score){        
                highscores.Insert(i, newEntry);
                enter = true;
                break;
            }
        }

        if(!enter){
            if(highscores.Count < 9)
                highscores.Insert(highscores.Count, newEntry);
        }
            
        if(highscores.Count > 10)
            highscores.RemoveAt(highscores.Count -1);
        
    }

    public void Read(){
        string path = "Assets/Resources/leaderboard_scores.txt";
        
        if(File.Exists(path)){
            highscores.Clear();
            StreamReader reader = new StreamReader(path);

            string line = "";
            char[] separators = { ' ', '\t' };
            string[] row_of_details;

            for(int i=0; i<10; i++){
                Highscore readEntry = new Highscore();

                line = reader.ReadLine();
                if(line == null)
                    break;
                row_of_details = line.Split(separators,2);

                readEntry.name = row_of_details[0];
                readEntry.score = int.Parse(row_of_details[1]);

                highscores.Add(readEntry);            
            }

            reader.Close();
        }

    }

    public void Save(){
        string path = "Assets/Resources/leaderboard_scores.txt";
        StreamWriter writer = new StreamWriter(path);

        string line = "";

        Debug.Log("hscore: "+ highscores.Count);
        for(int i=0; i<highscores.Count; i++){
            Debug.Log("hs: " + highscores[i].name + " " + highscores[i].score.ToString());
            line += highscores[i].name + " " + highscores[i].score.ToString();
            writer.WriteLine(line);
            line = "";
        }
        Debug.Log("hscore: "+ highscores.Count);


        writer.Close();
    }

    public void OpenLeaderboard()
    {
        Read();
        if(highscores != null){
            for(int i=0; i<highscores.Count; i++){
                entriesRoot.GetChild(i).GetComponent<HighscoreUI>().playerName.text = highscores[i].name;
                entriesRoot.GetChild(i).GetComponent<HighscoreUI>().score.text = highscores[i].score.ToString();
                entriesRoot.GetChild(i).gameObject.SetActive(true);
            }
        }
        gameObject.SetActive(true);
    }
}