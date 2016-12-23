using UnityEngine;
using System.Collections;

public class Tete : MonoBehaviour
{
    static public Tete instance;

    float previousStayTime;
    private float speed = 2.85f;
    private int speedUp = 4;

    private Vector3 vectorRight = Vector3.right;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Can't instanciate twice a singleton");
        }

        instance = this;

        previousStayTime = -1;
    }

    void Update()
    {
        if (Manager.instance.state != Manager.STATES.PAUSE)
        {
            transform.position += vectorRight * (speed * Time.deltaTime);
            ClampPosition(); 
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Blocs blocScript = coll.GetComponent<Blocs>();
        if (blocScript != null)
        {
            blocScript.ActionOnEnter();
            previousStayTime = Time.time;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        Blocs blocScript = coll.GetComponent<Blocs>();

        if (Time.time - previousStayTime > 0.1f && blocScript != null)
        {
            blocScript.ActionOnExit();
        }
    }

    void ClampPosition()
    {
        if (Manager.instance.snap == true)
        {
            transform.position = new Vector3(this.transform.position.x, Mathf.Clamp(transform.position.y, Corps.instance.transform.position.y + 1, 0.0F), 0);
        }
        else
        {
            transform.position = new Vector3(this.transform.position.x, Mathf.Clamp(transform.position.y, Corps.instance.transform.position.y + 1.1f, 0.0F), 0);
        }
    }


    public void MoveUp()
    {
        if (Manager.instance.snap == false)
        {
            //transform.position += vectorUp * .1f;
            transform.Translate(Vector3.up * Time.deltaTime * speedUp);
        }
    }

    public void MoveDown()
    {
        if (Manager.instance.snap == false)
        {
            //transform.position += vectorDown * .1f;

            transform.Translate(Vector3.down * Time.deltaTime * speedUp);
        }
    }
}
