using UnityEngine;
using System.Collections;

public class ButtonJoinGame : MonoBehaviour {

    public int gameId;
    public string gameName;

    public UILabel nameLabel;

	public void SetupButton(int id, string name)
    {
        gameId = id;
        gameName = name;
        nameLabel.text = name;
    }

    public void JoinGame()
    {
        PlayerPrefs.SetInt("gameToJoin", gameId);
        PlayerPrefs.SetInt("player", FindObjectOfType<MenuController>().player);
        Application.LoadLevel("main");
    }
}
