using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject objectiveTimer;
    [SerializeField] private GameObject progressUI;
    [SerializeField] private Slider progress;
    [SerializeField] private float timeToCompleteObjective;

    [SerializeField] private Image inRangeNotif;
    [SerializeField] private Image correctOrientationNotif;
    
    [SerializeField] private GameObject flipNotif;

    private float currentFill;
    private bool objectiveMet;
    private bool paused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            pauseMenu.SetActive(paused);
        }
        if (objectiveMet)
        {
            currentFill += (1f / timeToCompleteObjective) * Time.deltaTime;
            progress.value = currentFill; // Using a slider as a radial fill. Increment as long as objective conditions are met

            if (currentFill >= 1f)
            {
                EndGame();
            }
        }
    }

    public void ResetProgress()
    {
        currentFill = 0;
        progress.value = 0f;
    }

    public void EndGame()
    {
        endGameScreen.SetActive(true);
        objectiveTimer.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Cancel()
    {
        paused = false;
        pauseMenu.SetActive(paused);
    }

    public void ToggleEndGameScreen(bool isActive)
    {
        endGameScreen.SetActive(isActive);
    }

    public void InsideObjective(bool isInside)
    {
        objectiveTimer.SetActive(isInside);
        objectiveMet = isInside;

        if(!isInside)
        {
            ResetProgress();
        }
    }

    public void UpdateNotifications(bool inRange, bool correctOrientation)
    {
        inRangeNotif.color = inRange ? Color.red : Color.green;
        correctOrientationNotif.color = correctOrientation ? Color.red : Color.green;
    }

    public void UpdateFlipNotification(bool showHint)
    {
        flipNotif.SetActive(showHint);
    }
}