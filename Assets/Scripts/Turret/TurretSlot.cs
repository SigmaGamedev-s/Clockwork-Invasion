using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TurretSlot : MonoBehaviour
{
    public Sprite turretSprite;
    public GameObject turretObject;
    public int price;
    public TurretType turretType;

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
        if (ScoreCounter.Instance == null) return;

        // 🔹 Если уже выбрана эта турель — отменяем выбор
        if (gms.currentTurret == turretObject)
        {
            ScoreCounter.Instance.AddToEnergy(gms.currentTurretPrice);
            gms.currentTurret = null;
            gms.currentTurretSprite = null;
            gms.currentTurretType = TurretType.None;
            gms.currentTurretPrice = 0;
            return;
        }

        // 🔹 Если была выбрана другая турель — возвращаем её цену
        if (gms.currentTurret && gms.currentTurretPrice > 0)
        {
            ScoreCounter.Instance.AddToEnergy(gms.currentTurretPrice);
        }

        // 🔹 Проверка бюджета
        if (ScoreCounter.Instance.ScoreEnergy >= price)
        {
            ScoreCounter.Instance.AddToEnergy(-price);
            gms.currentTurret = turretObject;
            gms.currentTurretSprite = turretSprite;
            gms.currentTurretType = turretType;
            gms.currentTurretPrice = price;

            // сброс выбора сошки
            if (gms.currentBipod)
            {
                ScoreCounter.Instance.AddToGear(gms.currentBipodPrice);
                gms.currentBipod = null;
                gms.currentBipodSprite = null;
                gms.currentBipodType = TurretType.None;
                gms.currentBipodPrice = 0;
            }
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
