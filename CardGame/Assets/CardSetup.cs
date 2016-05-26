using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class CardSetup : MonoBehaviour {

    public int cardId;
    public string json;

    public string name, description, image, background;

    public int cost, rollMin, rollMax;

    public CardTexture main, cardBackground;
    public UILabel cardName, cardDescription, cardCost, cardTrigger;

    private ApiController api;

	// Use this for initialization
	IEnumerator Start () {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetCardInfo(cardId);
        json = api.result;
        yield return GenerateCard();
	}

    IEnumerator GenerateCard()
    {
        Debug.Log("generating card");
        var cardInfo = JSON.Parse(json);
        name = cardInfo["name"];
        description = cardInfo["description"];
        image = cardInfo["image"];
        background = cardInfo["background"];
        cost = int.Parse(cardInfo["cost"]);
        rollMin = int.Parse(cardInfo["rollMin"]);
        rollMax = int.Parse(cardInfo["rollMax"]);
        yield return GenerateTextures();
        GenerateText();
    }

    IEnumerator GenerateTextures()
    {
        yield return main.DownloadTexture(image);
        yield return cardBackground.DownloadTexture(background);
    }

    void GenerateText()
    {
        cardName.text = name;
        cardDescription.text = description;
        cardCost.text = cost.ToString();
        string triggerText = rollMin.ToString();
        if (rollMin != rollMax)
            triggerText += " - " + rollMax.ToString();
        cardTrigger.text = triggerText;
    }

}
