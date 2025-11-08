using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BipodSlot : MonoBehaviour
{
    public Sprite bipodSprite;
    public GameObject bipodObject;
    public int price;

    public TurretType bipodType;


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
        if (ScoreCounter.Instance == null) return;

        // 🔹 Если уже выбрана эта сошка — отменяем выбор
        if (gms.currentBipod == bipodObject)
        {
            ScoreCounter.Instance.AddToGear(gms.currentBipodPrice);
            gms.currentBipod = null;
            gms.currentBipodSprite = null;
            gms.currentBipodType = TurretType.None;
            gms.currentBipodPrice = 0;
            return;
        }

        // 🔹 Если была выбрана другая сошка — возвращаем её цену
        if (gms.currentBipod && gms.currentBipodPrice > 0)
        {
            ScoreCounter.Instance.AddToGear(gms.currentBipodPrice);
        }

        // 🔹 Проверка бюджета
        if (ScoreCounter.Instance.ScoreGear >= price)
        {
            ScoreCounter.Instance.AddToGear(-price);
            gms.currentBipod = bipodObject;
            gms.currentBipodSprite = bipodSprite;
            gms.currentBipodType = bipodType;
            gms.currentBipodPrice = price;

            // сброс выбора турели
            if (gms.currentTurret)
            {
                ScoreCounter.Instance.AddToEnergy(gms.currentTurretPrice);
                gms.currentTurret = null;
                gms.currentTurretSprite = null;
                gms.currentTurretType = TurretType.None;
                gms.currentTurretPrice = 0;
            }
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
