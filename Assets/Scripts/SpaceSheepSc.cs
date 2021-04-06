using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class SpaceSheepSc : MonoBehaviour
{
    int sw, sh;
    public float SheepSpeed=2;
    Vector3 maxP, minP;

    //объект стрельбы
    public GameObject bullet;
    GameObject Bullet;

    //звук выстрела
    AudioSource source;
    public AudioClip[] clips;
    // переменная для отслеживания количества жизней
    public static int LifeCount;
    GameManagerSc gameManager;

    Animator anim;
    bool activity;


    private void Awake()
    {
        LifeCount = 4;
    }

    void Start()
    {
        //при старте игры помещаем корабль в центр экрана
        sw = Screen.width;
        sh = Screen.height;        
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(sw / 2, sh / 2, 0));
        transform.position = new Vector3(pos.x, pos.y, 0);

        //определяем рамки за которые корабль не вылетает
        maxP = Camera.main.ScreenToWorldPoint(new Vector3(sw, sh, 0));
        minP = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        
        // определяем источник звука
        source = GetComponent<AudioSource>();

        LifeCount = 4;
        gameManager = GameObject.Find("Canvas").GetComponent<GameManagerSc>();

        anim = GetComponent<Animator>();
        activity = true;
    }

    IEnumerator EndGameDelay()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.EndGame();
        Destroy(this);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
        if (LifeCount == 0)
        {
            anim.SetTrigger("destroy");
            StartCoroutine(EndGameDelay());
            activity = false;
           
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("столкновение");
        source.clip = clips[1];
        if (!source.isPlaying) source.Play();
        LifeCount--;
    }

    private void KeyboardInput()
    {
        if (activity)
        {
            // движение по нажатию кнопок
            if (transform.position.x < maxP.x && Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * SheepSpeed * Time.deltaTime, Space.World);
            }
            if (transform.position.x > minP.x && Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.right * -SheepSpeed * Time.deltaTime, Space.World);
            }
            if (transform.position.y < maxP.y && Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.up * SheepSpeed * Time.deltaTime, Space.World);
            }
            if (transform.position.y > minP.y && Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.up * -SheepSpeed * Time.deltaTime, Space.World);
            }
            // поворот по нажатию кнопок - стрелок, зависит от той же переменной скорости, но с коэфициентом
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, SheepSpeed * 60 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -SheepSpeed * 60 * Time.deltaTime);
            }

            // стрельба добавляет на сцену снаряд, дальше он управляется собственным скриптом
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (bullet != null)
                {
                    Bullet = Instantiate(bullet, transform.position + transform.up * 0.5f, transform.rotation);
                    source.clip = clips[0];
                    if (!source.isPlaying) source.Play();
                }
            }
        }
        
    }
}
