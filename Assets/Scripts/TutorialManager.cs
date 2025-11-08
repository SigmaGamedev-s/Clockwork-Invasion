using UnityEngine;
using UnityEngine.UI;

public class SimpleTutorial : MonoBehaviour
{
    [Header("Pages")]
    public GameObject page1;
    public GameObject page2;

    [Header("Button")]
    public Button nextButton;

    private bool onFirstPage = true;

    private void Start()
    {
        page1.SetActive(true);
        page2.SetActive(false);

        if (nextButton != null)
            nextButton.onClick.AddListener(SwitchPage);
    }

    private void SwitchPage()
    {
        onFirstPage = !onFirstPage;

        page1.SetActive(onFirstPage);
        page2.SetActive(!onFirstPage);
    }
}
