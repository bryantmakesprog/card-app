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

    public GameObject purchaseButton;

    private ApiController api;

    public void SetCardId(int newId)
    {
        cardId = newId;
    }

    IEnumerator GetCardInfo()
    {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetCardInfo(cardId);
        json = api.result;
    }

    public IEnumerator GenerateCard(bool isOwned, bool isPurchasable, int pointsOwned)
    {
        yield return GetCardInfo();
        var cardInfo = JSON.Parse(json);
        name = cardInfo["name"];
        transform.name = name;
        description = cardInfo["description"];
        image = cardInfo["image"];
        background = cardInfo["background"];
        cost = int.Parse(cardInfo["cost"]);
        rollMin = int.Parse(cardInfo["rollMin"]);
        rollMax = int.Parse(cardInfo["rollMax"]);
        yield return GenerateTextures();
        GenerateText();
        GenerateButton(isOwned, isPurchasable, pointsOwned);
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

    void GenerateButton(bool isOwned, bool isPurchasable, int pointsOwned)
    {
        if (!isOwned)
            return;
        else
        {
            if (!isPurchasable)
                return;
            else
            {
                purchaseButton.SetActive(true);
                if (cost > pointsOwned)
                    purchaseButton.GetComponent<UIButton>().isEnabled = false;
            }
        }
    }

}
