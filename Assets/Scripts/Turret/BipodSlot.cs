using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BipodSlot : MonoBehaviour
{
    public Sprite bipodSprite;
    public GameObject bipodObject;
    public int price;

    public Image icon;
    public TextMeshProUGUI priceText;

    private Gamemanager gms;
    private Button button;

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(BuyBipod);
    }

    private void Update()
    {
        bool canAfford = ScoreCounter.Instance != null && ScoreCounter.Instance.ScoreGear >= price;
        priceText.color = canAfford ? Color.green : Color.red;
        button.interactable = canAfford;
    }

    private void BuyBipod()
    {
        if (ScoreCounter.Instance == null)
            return;

        if (ScoreCounter.Instance.ScoreGear >= price)
        {
            ScoreCounter.Instance.AddToGear(-price);
            gms.currentBipod = bipodObject;
            gms.currentBipodSprite = bipodSprite;
            gms.currentTurret = null;
        }
        else
        {
            transform.DOShakeScale(0.3f, 0.2f);
        }
    }

    private void OnValidate()
    {
        if (bipodSprite)
        {
            icon.enabled = true;
            icon.sprite = bipodSprite;
            priceText.text = price.ToString();
        }
        else
        {
            icon.enabled = false;
        }
    }
}
