using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("States")]
    [HideInInspector]
    public static UIManager instance;
    [HideInInspector]
    public PlayerController playerController;
    [SerializeField]
    private InventoryManager inventoryManager;

    public UIState uIState = UIState.Playng;
    
    [SerializeField]
    private Transform PauseMenuScreen;
    [SerializeField]
    private Transform optionsScreen;
    [SerializeField]
    private Transform deathScreen;
    [SerializeField]
    private Transform HUD;
    [SerializeField]
    private Slider healthBarSlider;
    [SerializeField]
    private TextMeshProUGUI healtBarText;
    [SerializeField]
    private TextMeshProUGUI ammoCounter;

    [SerializeField]
    private Transform inventory;
    [SerializeField]
    string sceneName;
    [SerializeField]
    string mainMenuSceneName = "MainMenu";

    public enum UIState
    {
        Playng,
        paused,
        inventory,
        defeat,
        disabled
    }
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerController.instance;
        UpdateUIState((int)uIState);
    }

    // Update is called once per frame
    void Update()
    {
        switch (uIState)
        {
            case UIState.Playng:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UpdateUIState((int)UIState.paused);
                    break;
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    //UpdateUIState((int)UIState.inventory);
                    //break;
                }
                break;
            case UIState.paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UpdateUIState((int)UIState.Playng);
                }
                break;
            case UIState.inventory:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UpdateUIState((int)UIState.Playng);
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    //((int)UIState.Playng);
                    //break;
                }
                break;
            case UIState.defeat:

                break;
            case UIState.disabled:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UpdateUIState((int)UIState.Playng);
                }
                break;
        }
    }
    /// <summary>
    /// changes the state of the UI also changes the character controller state
    /// </summary>
    /// <param name="newstate">0 for playng, 1 for paused, 2 for inventory, 3 for  defeat, 4 for disabled</param>
    public void UpdateUIState(int newstate)
    {
        switch (newstate)
        {
            case 0: // playng
                playerController.SetState(PlayerController.PlayerStatus.Normal);
                uIState = UIState.Playng;
                PauseMenuScreen.gameObject.SetActive(false);
                optionsScreen.gameObject.SetActive(false);
                inventory.gameObject.SetActive(true);
                HUD.gameObject.SetActive(true);
                Time.timeScale = 1f;
                break;
            case 1: // paused
                playerController.SetState(PlayerController.PlayerStatus.Paused);
                uIState = UIState.paused;
                PauseMenuScreen.gameObject.SetActive(true);
                optionsScreen.gameObject.SetActive(false);
                inventory.gameObject.SetActive(false);
                HUD.gameObject.SetActive(false);
                Time.timeScale = 0;
                break;
            case 2: // inventory
                playerController.SetState(PlayerController.PlayerStatus.Inventory);
                uIState = UIState.inventory;
                PauseMenuScreen.gameObject.SetActive(false);
                optionsScreen.gameObject.SetActive(false);
                inventory.gameObject.SetActive(true);
                HUD.gameObject.SetActive(false);
                inventoryManager.UpdateWeaponDisplay();
                Time.timeScale = 0;
                break;
            case 3: // defeat
                playerController.SetState(PlayerController.PlayerStatus.Paused);
                uIState = UIState.defeat;
                PauseMenuScreen.gameObject.SetActive(false);
                optionsScreen.gameObject.SetActive(false);
                inventory.gameObject.SetActive(false);
                HUD.gameObject.SetActive(false);
                deathScreen.gameObject.SetActive(true);
                Time.timeScale = 0.05f;
                break;
            case 4: // disabled
                playerController.SetState(PlayerController.PlayerStatus.Normal);
                uIState = UIState.disabled;
                PauseMenuScreen.gameObject.SetActive(false);
                optionsScreen.gameObject.SetActive(false);
                inventory.gameObject.SetActive(false);
                HUD.gameObject.SetActive(false);
                Time.timeScale = 1;
                break;
            default:
                Debug.Log("newstate is out of range or not set defaulting to playng");
                UpdateUIState(0);
                break;
        }
    }

    public void UpdateHealth(float current, float max)
    {
        healthBarSlider.maxValue = max;
        healthBarSlider.value = current;
        healtBarText.text = "Health " + current + " / " + max;
    }

    public void UpdateAmmo(int magazine, int clipSize ,int reserve)
    {
        if (ammoCounter == null)
        {
            Debug.Log("no ammo counter");
            return;
        }
        ammoCounter.text = "Ammo " + reserve + " [" + magazine + " / " + clipSize + "]";
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
