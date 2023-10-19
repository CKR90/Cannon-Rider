using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject LoadingPanel;
    public AutoPilot Vehicle;
    public GPU_Spawner BasketItemSpawner;
    public TextMeshProUGUI fpsText;
    public CannonGun cannonGun;
    public TrajectoryVisualizer trajectoryVisualizer;

    private float timer;
    private int fpsCounter = 0;
    private bool init = false;


    [HideInInspector] public bool FoliageReady = false;
    [HideInInspector] public bool EnvironmentReady = false;
    [HideInInspector] public bool GameStarted = false;
    [HideInInspector] public bool GameFinished = false;

    private float fpsAvarageTimer = 0f;
    private int frameAvarageCounter = 0;

    private void Awake()
    {
        if (QualitySettings.vSyncCount > 0)
            Application.targetFrameRate = 60;
        else
            Application.targetFrameRate = -1;


        Time.timeScale = 1f;
        AudioListener.pause = true;
        LoadingPanel.SetActive(true);
        Instance = this;
        Physics.IgnoreLayerCollision(6, 7);
        Physics.IgnoreLayerCollision(7, 8);
        GameDatabase.Instance.ResetData();

    }
    private void Start()
    {
        DailyMissionDatabase.GetDailyCopy();
        InitializeBasket();
    }
    public void StartGame()
    {
        GameStarted = true;
        Vehicle.StartGame();
    }
    public void ResetGame()
    {
        GameStarted = false;
        GameDatabase.Instance.ResetData();
        DailyMissionDatabase.ReturnDailyCopy();
        GetComponent<BasketController>().ResetBaskets();
        GetComponent<BasketController>().Initialize();
    }
    public void ReturnMainMenu()
    {
        DailyMissionDatabase.ReturnDailyCopy();
        SceneManager.LoadScene("Menu");
    }
    private void Update()
    {
        FPSViewer();

        if(!init && FoliageReady && EnvironmentReady)
        {
            AudioListener.pause = false;
            init = true;
            LoadingPanel.SetActive(false);
        }

        if(LevelDataTransfer.gamePlaySettings.levelIndex < 20 && GameStarted)
        {
            frameAvarageCounter++;
            fpsAvarageTimer += Time.deltaTime;
        }
    }
    private void FPSViewer()
    {
        if (!LevelDataTransfer.levelUI.showFPS)
        {
            if (fpsText.text != "") fpsText.SetText("");
            return;
        }

        timer += Time.deltaTime;
        fpsCounter++;
        if (timer >= 1f)
        {
            fpsText.SetText("FPS: " + fpsCounter.ToString());

            timer = 0f;
            fpsCounter = 0;
        }
    }
    private void InitializeBasket()
    {
        if (!LevelDataTransfer.gamePlaySettings.basketEnable) return;

        BasketItemSpawner.BasketData = GetComponent<BasketController>().basketDatas.Find(x => x.Item == LevelDataTransfer.gamePlaySettings.basketItem);
    }

    public void PlayMusic()
    {
        SoundController.instance.PlayMusic(MusicList.Random);
    }
    public void StopMusic()
    {
        SoundController.instance.StopMusic();
    }
    public void FinishGame()
    {
        GameFinished = true;
        Vehicle.FinishGame();
        cannonGun.FinishGame();
        trajectoryVisualizer.FinishGame();
        ObstacleController.instance.FinishGame();
        FinishLineParticles.Instance.FinishGame();
        FinishGameDarker.instance.FinishGame();

        int rand = Random.Range((int)SFXList.WinSound1, (int)SFXList.WinSound2 + 1);
        SoundController.instance.Play((SFXList)rand);
        SoundController.instance.StopMusic();
        SoundController.instance.PlayEndGameMusic();


        if (LevelDataTransfer.gamePlaySettings.levelIndex < 20)
        {
            int fps = Mathf.FloorToInt((float)frameAvarageCounter / fpsAvarageTimer);

            if(User.info.fps.Count - 1 >= LevelDataTransfer.gamePlaySettings.levelIndex)
            {
                User.info.fps[LevelDataTransfer.gamePlaySettings.levelIndex] = fps;
            }
            else
            {
                User.info.fps.Add(fps);
            }
        }



        GameDatabase.Instance.SetFinishTime(PlayTimeCounter.Instance.TimeToFinish);
        GameDatabase.Instance.ApplyDataToLocalDatabase();
        DBManager.Instance.UpdateUserInfo(User.info);
        DailyMissionDatabase.UpdateBasketData();
        DailyMissionDatabase.ApplyDaily();
    }
}