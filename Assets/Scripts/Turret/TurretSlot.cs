using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TurretSlot : MonoBehaviour
{
    public Sprite turretSprite;
    public GameObject turretObject;
    public int price;

    public Image icon;
    public TextMeshProUGUI priceText;

    private Gamemanager gms;
    private Button button;

    private void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<Gamemanager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(BuyTurret);
    }

    private void Update()
    {
        bool canAfford = ScoreCounter.Instance != null && ScoreCounter.Instance.ScoreEnergy >= price;
        priceText.color = canAfford ? Color.green : Color.red;
        button.interactable = canAfford;
    }

    private void BuyTurret()
    {
        if (ScoreCounter.Instance == null)
            return;

        if (ScoreCounter.Instance.ScoreEnergy >= price)
        {
            ScoreCounter.Instance.AddToEnergy(-price);
            gms.currentTurret = turretObject;
            gms.currentTurretSprite = turretSprite;
            gms.currentBipod = null;
        }
        else
        {
            transform.DOShakeScale(0.3f, 0.2f);
        }
    }

    private void OnValidate()
    {
        if (turretSprite)
        {
            icon.enabled = true;
            icon.sprite = turretSprite;
            priceText.text = price.ToString();
        }
        else
        {
            icon.enabled = false;
        }
    }
}
