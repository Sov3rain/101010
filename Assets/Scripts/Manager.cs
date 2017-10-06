using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(AudioSource))]

public class Manager : MonoBehaviour
{

    static public Manager instance;

    public enum STATES
    {
        MENU,
        PLAY,
        FAIL,
        WIN,
        LOADING,
        PAUSE,
        _INVALID
    };

    // rectangle général pour scaler toute l'interface (penser uniquement en 800x480)
    static public Rect Myrect(float x, float y, float size_x, float size_y)
    {
        return new Rect(x * Screen.width / 480, y * Screen.height / (800), size_x * Screen.width / 480, size_y * Screen.height / (800));
    }

    public STATES state = STATES.MENU;
    public bool snap;
    public bool soundOnOff;
    public int maxLevel = 1;
    private int endGame = 5;
    private int tries = 0;

    public GameObject headPrefab;
    public GameObject bodyPrefab;

    public GUIStyle mainStyle;
    public GUIStyle mainOrange;
    public GUIStyle roundedStyleUp;
    public GUIStyle roundedStyleDown;
    public GUIStyle roundedStylePause;
    //public GUITexture blur;

    public Vector2 scrollPos = Vector2.zero;
    private int buttonSize = 225;
    private int buttonHeight = 64;
    private int buttonSpacing = 80;

    public AudioClip[] sounds;

    Levels levels;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Can't instanciate twice a singleton");
        }

        instance = this;
        snap = false;
        levels = GetComponent<Levels>();

        maxLevel = PlayerPrefs.GetInt("maxLevel", 1);
        tries = PlayerPrefs.GetInt("tries", 0);
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mainStyle.fontSize = 48 * Screen.width / 480;
        mainOrange.fontSize = 48 * Screen.width / 480;
        soundOnOff = true;
        PlaySound(0);
        //blur.enabled = true;
    }

    void LateUpdate()
    {
        if (Corps.instance != null)
        {
            Camera.main.transform.position = new Vector3(Corps.instance.transform.position.x + 2, -4, -10);
        }
    }

    void OnGUI()
    {
        switch (state)
        {
            case STATES.MENU:
                {
                    //blur.enabled = true;

                    if (GUI.Button(Myrect(240- 260/2, 700, 260, buttonHeight), "Reset", mainStyle))
                    {
                        Reset();
                    }

                    GUI.Label(Myrect(240 - 260 / 2, 530, 260, buttonHeight), "Tries: " + tries, mainStyle);

                    //scrollPos = GUI.BeginScrollView(new Rect(0, 0, 240, 410), scrollPos, new Rect(0, 0, 240, (maxLevel + 1) * 64.0f));

                    for (int i = 1; i <= maxLevel; i++)
                    {
                        if (GUI.Button(Myrect(240 - buttonSize / 2, i * buttonSpacing, buttonSize, buttonHeight), "Level " + i, mainStyle))
                        {
                            state = STATES.LOADING;
                            levels.LoadLevel(i);
                            PlaySound(1);
                        }            
                    }

                   // GUI.EndScrollView();

                    if (soundOnOff == true)
                    {
                        if (GUI.Button(Myrect(240 - 260 / 2, 615, 260, buttonHeight), "Sound ON", mainOrange))
                        {
                            GetComponent<AudioSource>().Stop();
                            soundOnOff = !soundOnOff;
                        }
                    }

                    if (soundOnOff == false)
                    {
                        if (GUI.Button(Myrect(240 - 260 / 2, 615, 260, buttonHeight), "Sound OFF", mainStyle))
                        {
                            soundOnOff = !soundOnOff;
                            PlaySound(0);
                        }
                    }
                    break;
                }
            case STATES.LOADING:
                {
                    //blur.enabled = true;

                    GUI.Label(Myrect(0, 0, 400, 300), "Loading in progress...");
                    break;
                }
            case STATES.PLAY:
                {
                    //blur.enabled = false;

                    if (GUI.Button(Myrect(10, 10, 60, 60), "", roundedStylePause))
                    {
                        Time.timeScale = .0f;
                        state = STATES.PAUSE;

                    }

                    if (GUI.RepeatButton(Myrect(293, 615, 137, 137), "", roundedStyleUp))
                    {
                        Tete.instance.MoveUp();
                        Corps.instance.MoveDown();
                    }

                    if (GUI.RepeatButton(Myrect(48, 615, 137, 137), "", roundedStyleDown))
                    {
                        Tete.instance.MoveDown();
                        Corps.instance.MoveUp();
                    }
                    break;
                }

            case STATES.FAIL:
                {
                    //blur.enabled = true;

                    if (GUI.Button(Myrect(240 - buttonSize / 2, 280, buttonSize, buttonHeight), "Restart", mainStyle))
                    {
                        Retry();
                    }

                    if (GUI.Button(Myrect(240 - buttonSize / 2, 380, buttonSize, buttonHeight), "Menu", mainStyle))
                    {
                        ReturnToMenu();
                        PlaySound(0);
                    }
                    break;
                }

            case STATES.WIN:
                {
                    //blur.enabled = true;

                    if (levels.currentLevel != endGame)
                    {
                        if (GUI.Button(Myrect(240 - buttonSize / 2, 280, buttonSize, buttonHeight), "Next level", mainStyle))
                        {
                            Clear();
                            state = STATES.LOADING;
                            levels.LoadLevel(levels.currentLevel + 1);
                        }
                    }

                    if (GUI.Button(Myrect(240 - buttonSize / 2, 380, buttonSize, buttonHeight), "Menu", mainStyle))
                    {
                        ReturnToMenu();
                        PlaySound(0);
                    }
                    break;
                }
            case STATES.PAUSE:
                {
                    //blur.enabled = true;

                    if (GUI.Button(Myrect(240 - buttonSize / 2, 280, buttonSize, buttonHeight), "Resume", mainStyle))
                    {
                        state = STATES.PLAY;
                        Time.timeScale = 1.0f;
                    }

                    if (GUI.Button(Myrect(240 - buttonSize / 2, 380, buttonSize, buttonHeight), "Menu", mainStyle))
                    {
                        ReturnToMenu();
                        PlaySound(0);
                    }
                    break;
                }
            case STATES._INVALID: break;
        }
    }

    public void Victory()
    {
        if (levels.currentLevel != endGame)
        {
            maxLevel = Mathf.Max(levels.currentLevel + 1, maxLevel);
            PlayerPrefs.SetInt("maxLevel", maxLevel);
        }
        Clear();
        state = STATES.WIN;
    }

    public void Fail()
    {
        tries++;
        PlayerPrefs.SetInt("tries", tries);
        Clear();
        state = STATES.FAIL;
    }

    void Retry()
    {
        snap = false;
        state = STATES.LOADING;
        levels.LoadLevel(Levels.instance.currentLevel);
    }

    public void Clear()
    {
        for (int childIndex = 0; childIndex < transform.childCount; childIndex++)
        {
            Transform child = transform.GetChild(childIndex);
            Destroy(child.gameObject);
        }
    }

    void ReturnToMenu()
    {
        Clear();
        state = STATES.MENU;
    }

    void PlaySound(int i)
    {
        if (soundOnOff == true)
        {
            this.GetComponent<AudioSource>().clip = sounds[i];
            GetComponent<AudioSource>().Play();
        }
    }

    void Reset()
    {
        maxLevel = 1;
        tries = 0;
        PlayerPrefs.SetInt("maxLevel", 1);
        PlayerPrefs.SetInt("tries", 0);
    }
}
