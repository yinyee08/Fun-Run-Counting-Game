using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseBoard : MonoBehaviour {
    public GameObject Leader_Board;
    public GameObject MainPanel;
	public GameObject MainMenu;

    public void OpenLeaderBoard(){
        MainMenu.SetActive(false);
        MainPanel.SetActive(false);
		Leader_Board.GetComponent<Leaderboard>().OpenLeaderboard();
		Leader_Board.SetActive(true);
    }

    public void CloseLeaderBoard(){
        MainMenu.SetActive(true);
        MainPanel.SetActive(true);
		Leader_Board.SetActive(false);
    }
}