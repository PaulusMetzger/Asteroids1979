using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed=10f;

    int sw, sh;
    // определение координат за которые снаряд не вылетает
    Vector2 min, max;

    Asteroid my_Asteroid;
    AlienShipSc my_AlienShip;



    // Start is called before the first frame update
    void Start()
    {
        sw = Screen.width;
        sh = Screen.height;
        Vector3 maxP = Camera.main.ScreenToWorldPoint(new Vector3(sw, sh, 0));
        Vector3 minP = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));

        max = new Vector2(maxP.x, maxP.y);
        min = new Vector2(minP.x, minP.y);
        Debug.Log("min" + min+ "max" + max);
    }

    // Update is called once per frame
    void Update()
    {
        //движение вперед по прямой или уничтожение при вылете за экран
        if (transform.position.x < max.x && transform.position.y < max.y && transform.position.x > min.x && transform.position.y > min.y)
            transform.position += transform.up * Time.deltaTime * bulletSpeed;
        else
        {
            Destroy(gameObject);
            Debug.Log("пуля улетела");
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("столкновение");
        if (col.gameObject.tag == "Asteroid")
        {
            my_Asteroid = col.gameObject.GetComponent<Asteroid>();
            my_Asteroid.Trouble.Invoke();
        }
        else if(col.gameObject.tag == "Alien")
        {
            my_AlienShip = col.gameObject.GetComponent<AlienShipSc>();
            my_AlienShip.AlienDestroy();
        }
        Destroy(gameObject);
    }
}
