using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShipSc : MonoBehaviour
{

    Animator anim;
    AudioSource source;
    public AudioClip[] clips=new AudioClip[2];
    public float alienSpeed;

    // определение координат за которые тарелка не вылетает
    int sw, sh;
    Vector2 min, max;
    Vector3 maxP, minP, minC, maxC, destinationPoint;
    // для обезвреживания тарелки после попадания
    CircleCollider2D coll;
    bool activity;

    GameObject target; //игрок, которого тарелка будет пытаться таранить


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        coll= GetComponent<CircleCollider2D>();
        activity = true;

        sw = Screen.width;
        sh = Screen.height;

        //определяем прямоугольник координат 1/3  экрана по центру. В эту область будут направляться тарелка.
        maxC = Camera.main.ScreenToWorldPoint(new Vector3(sw - sw * 0.3f, sh - sh * 0.3f, 0));
        minC = Camera.main.ScreenToWorldPoint(new Vector3(0 + sw * 0.3f, 0 + sh * 0.3f, 0));

        //находим игрока
        target = GameObject.FindGameObjectWithTag("Player");
        destinationPoint = target.transform.position;
        StartCoroutine(NewDestination());
    }
    IEnumerator NewDestination()
    {
        yield return new WaitForSeconds(5);
        int a = Random.Range(0, 4);
        if(a%2==0) destinationPoint = target.transform.position;
        else
        {
            destinationPoint = new Vector3(Random.Range(minC.x, maxC.x), Random.Range(minC.y, maxC.y), 0);
        }
    }
    // Update is called once per frame
    void Update()
    {       
        if (activity)
        {
            source.clip = clips[0];
            if (!source.isPlaying) source.Play();
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, alienSpeed * Time.deltaTime);
        }
    }

    public void AlienDestroy()
    {
        source.clip = clips[1]; //переключение на взрыв
        if (!source.isPlaying) source.Play();
        AsteroidManager.Scores += 150;
        SpaceSheepSc.LifeCount++;
        Destroy(coll);
        activity = false;
        if (anim != null) anim.SetTrigger("destroy");
        Destroy(gameObject, 1);
    }
}
