using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class GameManagerSc : MonoBehaviour
{
    public GameObject StartMenu;
    public GameObject EndMenu;
    

    bool pauseReady;
    AudioSource source;
    [Header("Звуки паузы и возврата к игре")]
   public AudioClip[] clips;

    bool single;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        if (StartMenu != null) StartMenu.SetActive(true);
        if (StartMenu != null) EndMenu.SetActive(false);
        pauseReady = false;
        single = true;
        source = GetComponent<AudioSource>();
        source.clip = clips[1];
    }
    // функция для начала игры с начала (по кнопке)
    public void StartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    // функция для выхода из игры (по кнопке)
    public void ExitGame()
    {
        Application.Quit();
    }
    // функция открывающая итоговое меню
    public void EndGame()
    {
        EndMenu.SetActive(true);
        source.clip = clips[2];
        if(!source.isPlaying)source.Play();
        Time.timeScale = 0;
    }
    // Update is called once per frame
    void Update()
    {//блок работает один раз, игра снимается с паузы, убирается информационная панель
        if (single)
        {
            if (Input.anyKey)
            {
                Time.timeScale = 1;
                StartMenu.SetActive(false);
                pauseReady = true;
                source.Play();
                single = false;
            }

        }
        //блок работает по ходу  игры, включает выключает паузу
        if (!single)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (pauseReady)
                {
                    pauseReady = false;
                    source.clip = clips[0];
                    if (!source.isPlaying) source.Play();
                    Time.timeScale = 0;
                }
                else
                {
                    pauseReady = true;
                    source.clip = clips[1];
                    if (!source.isPlaying) source.Play();
                    Time.timeScale = 1;
                }
            }
        }
        
    }
}