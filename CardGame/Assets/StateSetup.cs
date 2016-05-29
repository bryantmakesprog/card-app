using UnityEngine;
using System.Collections;
using SimpleJSON;

public class StateSetup : MonoBehaviour {

    public int game, player, points;
    public GameObject cardPrefab;

    public Transform purchasablePosition, landmarkPosition, inventoryPosition;
    public float cardSpacing, cardVerticalSpacing;
    public int cardsPerRow;

    private ApiController api;
    public string json;

    IEnumerator GetHandInfo()
    {
        api = GameObject.FindObjectOfType<ApiController>();
        yield return api.GetHandInfo(game, player);
        json = api.result;
    }
	
	public IEnumerator GenerateHand(bool isMyTurn)
    {
        yield return GetHandInfo();
        Debug.Log("generating hand for player " + player);
        var handInfo = JSON.Parse(json);
        gameObject.transform.name = "Player " + handInfo["player"];
        points = int.Parse(handInfo["points"]);
        bool isOwned = player == GameObject.FindObjectOfType<InputController>().player;
        yield return GenerateInventory(handInfo["inventory"], isOwned, isMyTurn);
        yield return GeneratePurchasable(handInfo["purchasable"], isOwned, isMyTurn);
        yield return GenerateLandmarks(handInfo["landmarks"], isOwned, isMyTurn);
    }

    IEnumerator GenerateInventory(JSONNode inventory, bool isOwned, bool isMyTurn)
    {
        int i = 0;
        bool isPurchasable = false && isMyTurn;
        while(i < inventory.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            entity.GetComponent<CardSetup>().SetCardId(int.Parse(inventory[i]));
            yield return entity.GetComponent<CardSetup>().GenerateCard(isOwned, isPurchasable, points);
            entity.transform.localPosition = inventoryPosition.localPosition + (Vector3.right * (i % cardsPerRow) * cardSpacing) + (Vector3.down * (i / cardsPerRow) * cardVerticalSpacing);
            entity.transform.parent = inventoryPosition.transform;
            i++;
        }
    }

    IEnumerator GeneratePurchasable(JSONNode purchasable, bool isOwned, bool isMyTurn)
    {
        int i = 0;
        bool isPurchasable = true && isMyTurn;
        while (i < purchasable.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            entity.GetComponent<CardSetup>().SetCardId(int.Parse(purchasable[i]));
            yield return entity.GetComponent<CardSetup>().GenerateCard(isOwned, isPurchasable, points);
            entity.transform.localPosition = purchasablePosition.localPosition + (Vector3.right * (i%cardsPerRow) * cardSpacing) + (Vector3.up * (i/cardsPerRow) * cardVerticalSpacing);
            entity.transform.parent = purchasablePosition.transform;
            i++;
        }
    }

    IEnumerator GenerateLandmarks(JSONNode landmarks, bool isOwned, bool isMyTurn)
    {
        int i = 0;
        bool isPurchasable = true;
        while (i < landmarks.Count)
        {
            GameObject entity = GameObject.Instantiate(cardPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            isPurchasable = (int.Parse(landmarks[i]) < 0) && isMyTurn;
            entity.GetComponent<CardSetup>().SetCardId(Mathf.Abs(int.Parse(landmarks[i])));
            yield return entity.GetComponent<CardSetup>().GenerateCard(isOwned, isPurchasable, points);
            entity.transform.localPosition = landmarkPosition.localPosition + (Vector3.right * i * cardSpacing);
            entity.transform.parent = landmarkPosition.transform;
            i++;
        }
    }
}
