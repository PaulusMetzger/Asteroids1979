
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class Asteroid : MonoBehaviour
{    
    public UnityEvent Trouble;
    public float AsteroidSpeed=1;
    AudioSource source;

    // определение координат за которые астероид не вылетает
    int sw, sh;
    Vector2 min, max;
    Vector3 maxP, minP, minC, maxC, destinationPoint;

    //код астероида, который будет передаваться в фунцию

    [Header("код астероида")]    
   public int asteroidCode;

    AsteroidManager manager;

  // для обезвреживания астероида после попадания
    CircleCollider2D coll;
    bool activity;
    // для включения анимации
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        sw = Screen.width;
        sh = Screen.height;
        //определяем прямоугольник координат на 1/10 больше экрана если астероид вылетает за него то переносится в другое место
        maxP = Camera.main.ScreenToWorldPoint(new Vector3(sw+sw/10f, sh+sh/10, 0));
        minP = Camera.main.ScreenToWorldPoint(new Vector3(0-sw/10, 0 - sh/10f, 0));
        max = new Vector2(maxP.x, maxP.y);
        min = new Vector2(minP.x, minP.y);
        //определяем прямоугольник координат 1/3  экрана по центру. В эту область будут направляться астероиды.
        maxC = Camera.main.ScreenToWorldPoint(new Vector3(sw - sw*0.3f, sh - sh*0.3f, 0));
        minC = Camera.main.ScreenToWorldPoint(new Vector3(0 + sw*0.3f, 0 + sh*0.3f, 0));      

        //Debug.Log(maxC + "максимальные координаты"+ minC + "минимальные координаты");

        destinationPoint = new Vector3(Random.Range(minC.x, maxC.x), Random.Range(minC.y, maxC.y), 0);
        
        //Debug.Log("destinationPoint "+destinationPoint);
        // поворот астероида в центральную область
        Vector3 direction = destinationPoint - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        manager = GameObject.Find("AsteroidManager").GetComponent<AsteroidManager>();

        coll = GetComponent<CircleCollider2D>();
        activity = true;

       anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (activity)
        {
            //астероид все время в движении по прямой

            if (transform.position.x < max.x && transform.position.y < max.y && transform.position.x > min.x && transform.position.y > min.y)
                transform.position += transform.up * AsteroidSpeed * Time.deltaTime;
            else
            {
                /*здесь нужно определить точку где снова появляется астероид, который вылетел за пределы экрана
                можно вынести в отдельную функцию потому что получается довольно громоздко */
                transform.position = LocationNew();
            }
        }
            
    }

    public void TrobleShoot()
    {
        if (!source.isPlaying) source.Play();
        if (manager != null) manager.CreateNewWreck(asteroidCode, transform.position);
        Destroy(coll);
        activity = false;
        if(anim!=null)anim.SetTrigger("destroy");
        Destroy(gameObject,1);
    }

    private Vector3 LocationNew()
    {
        Vector3 originPoint = new Vector3(0,0,0);

        int a = Random.Range(0, 4);
        //Debug.Log("a " + a);
        switch (a)
        {
            case 0:
                originPoint = new Vector3(Random.Range(max.x-0.3f, max.x), Random.Range(min.y, max.y), 0);
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
        destinationPoint = new Vector3(Random.Range(minC.x, maxC.x), Random.Range(minC.y, maxC.y), 0);
        //Debug.Log("destinationPoint " + destinationPoint);
        Vector3 direction = destinationPoint - transform.position;

        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        AsteroidSpeed = Random.Range(1, 4);
        return originPoint;
    }

}
