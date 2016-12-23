using UnityEngine;
using System.Collections;

public class Corps : MonoBehaviour
{

    static public Corps instance;
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
    }

    void Update()
    {
        if (Manager.instance.state != Manager.STATES.PAUSE)
        {
            transform.position += vectorRight * (speed*Time.deltaTime);
            ClampPosition();
            SetPosition();
        }
    }

    public void SetPosition()
    {
        if (Manager.instance.snap == false && this.transform.position.y != 0)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(this.transform.position.x, -5.0f, 0.0f), 10 * Time.deltaTime);
        }
    }

    void ClampPosition()
    {
        if (Manager.instance.snap == true)
        {
            transform.position = new Vector3(this.transform.position.x, Mathf.Clamp(transform.position.y, -5.0F, Tete.instance.transform.position.y - 1.15f), 0);
        }
        else
        {
            transform.position = new Vector3(this.transform.position.x, Mathf.Clamp(transform.position.y, -5.0F, Tete.instance.transform.position.y - 0.9f), 0);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        Blocs blocScript = coll.GetComponent<Blocs>();
        if (blocScript != null)
        {
            blocScript.ActionOnEnter();
        }
    }

    public void MoveUp()
    {
        if (Manager.instance.snap == true)
        {
            //transform.position += vectorUp * .1f;

            transform.Translate(Vector3.up * Time.deltaTime * speedUp);
        }
    }

    public void MoveDown()
    {
        if (Manager.instance.snap == true)
        {
            //transform.position += vectorDown * .1f;

            transform.Translate(Vector3.down * Time.deltaTime * speedUp);
        }
    }
}
