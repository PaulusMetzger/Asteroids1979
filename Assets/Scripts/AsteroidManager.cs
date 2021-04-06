using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AsteroidManager : MonoBehaviour
{
    
    // определение координат где образуются астероиды
    int sw, sh;
    Vector2 min, max;
    Vector3 maxP, minP, minC, maxC, destinationPoint;
   
    //массивы для префабов разных астероидов
    [Header("набор больших астероидов")]
    public GameObject[] bigAsteroids;
    [Header("набор средних астероидов")]
    public GameObject[] mediumAsteroids;
    [Header("набор маленьких астероидов")]
    public GameObject[] smallAsteroids;
    [Header("летающая тарелка")]
    public GameObject alienShip;

    GameObject asteroid;
    Asteroid asteroidSc;

    GameObject alien;
    AlienShipSc alienSc;

    int asteroidsCount;    //число астероидов на сцене
    int asteroidsNumber=4;  //число астероидов при создании (с каждой итерацией увеличивается)

    //набранные игроком баллы
    public static int Scores;

    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        sw = Screen.width;
        sh = Screen.height;
        //определяем прямоугольник координат на 1/10 больше экрана для создания астероидов за экраном
        maxP = Camera.main.ScreenToWorldPoint(new Vector3(sw + sw / 10f, sh + sh / 10, 0));
        minP = Camera.main.ScreenToWorldPoint(new Vector3(0 - sw / 10, 0 - sh / 10f, 0));
        max = new Vector2(maxP.x, maxP.y);
        min = new Vector2(minP.x, minP.y);
        StartCoroutine(timeForNewSet());
        StartCoroutine(TimeForNewAlien());

        CreateNewSet();
        source = GetComponent<AudioSource>();
        if (!source.isPlaying) source.Play();
        asteroidsCount = 28;          
    }

    IEnumerator timeForNewSet()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("число астероидоа"+ asteroidsCount);
        if (asteroidsCount < 5)
        {
            CreateNewSet();
            if (!source.isPlaying) source.Play();
        }
        StartCoroutine(timeForNewSet());
    }

    IEnumerator TimeForNewAlien()
    {
        yield return new WaitForSeconds(15);
        if (asteroidsCount < 10)
        {
            //if(alien!=null) CreateAlien();
            CreateAlien();
        }
        StartCoroutine(TimeForNewAlien());
    }

    private void CreateAlien()
    {
        alien = Instantiate(alienShip,Vector3.zero,Quaternion.identity);
        alienSc = alien.GetComponent<AlienShipSc>();
        alienSc.alienSpeed = Random.Range(3, 5);

        Vector3 originPoint = new Vector3(0, 0, 0);
        int a = Random.Range(0, 4);
        switch (a)
        {
            case 0:
                originPoint = new Vector3(Random.Range(max.x - 0.3f, max.x), Random.Range(min.y, max.y), 0);
                break;
            case 1:
                originPoint = new Vector3(Random.Range(min.x, min.x + 0.3f), Random.Range(min.y, max.y), 0);
                break;
            case 2:
                originPoint = new Vector3(Random.Range(min.x, max.x), Random.Range(max.y - 0.3f, max.y), 0);
                break;
            case 3:
                originPoint = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, min.y + 0.3f), 0);
                break;
            default:
                originPoint = Vector3.zero;
                break;
        }
        alien.transform.position = new Vector3(originPoint.x, originPoint.y, 0);
        alienSc = alien.GetComponent<AlienShipSc>();
        alienSc.alienSpeed = Random.Range(3, 5);
        StartCoroutine(DestroyAlien()); //тарелка появляется только на определенное время
    }

    IEnumerator DestroyAlien()
    {
        yield return new WaitForSeconds(15);
        if (alien != null) Destroy(alien);
    }
    //функция создания новых астероидов при уничтожении крупных
    public void CreateNewWreck(int AsteroidCode, Vector3 pos)
    {
        asteroidsCount--;
        if (AsteroidCode == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                asteroid = Instantiate(mediumAsteroids[Random.Range(0, mediumAsteroids.Length)]);
                asteroid.transform.position = pos;
                Scores += 25;
            }
        }
        else if (AsteroidCode == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                asteroid = Instantiate(smallAsteroids[Random.Range(0, smallAsteroids.Length)]);
                asteroid.transform.position = pos;
                Scores += 25;
            }
        }
        else if (AsteroidCode == 3)
        {
            Scores += 50;
        }
        else Debug.Log("Code mistake");
    }

    public void CreateNewSet()
    {
        
        for (int i=0; i< asteroidsNumber; i++) // с каждым разом создается на один большой астероид больше
        {
            asteroid = Instantiate(bigAsteroids[Random.Range(0, bigAsteroids.Length)]);
            asteroidsCount += 7;

            Vector3 originPoint = new Vector3(0, 0, 0);
            int a = Random.Range(0, 4);            
            switch (a)
            {
                case 0:
                    originPoint = new Vector3(Random.Range(max.x - 0.3f, max.x), Random.Range(min.y, max.y), 0);
                    break;
                case 1:
                    originPoint = new Vector3(Random.Range(min.x, min.x + 0.3f), Random.Range(min.y, max.y), 0);
                    break;
                case 2:
                    originPoint = new Vector3(Random.Range(min.x, max.x), Random.Range(max.y - 0.3f, max.y), 0);
                    break;
                case 3:
                    originPoint = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, min.y + 0.3f), 0);
                    break;
                default:
                    originPoint = Vector3.zero;
                    break;
            }
            asteroid.transform.position = new Vector3(originPoint.x, originPoint.y, 0);
            asteroidSc = asteroid.GetComponent<Asteroid>();
            asteroidSc.AsteroidSpeed = Random.Range(1, 4);   
        }
        asteroidsNumber++;
    }
}
