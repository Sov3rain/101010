using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;

    public float scale;

    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        line.SetWidth(scale, scale);
    }

    void Update()
    {
        if(Manager.instance.state != Manager.STATES.PLAY)
        {
            line.enabled = false;
        }
        else
        {
            line.enabled = true;
        }

        if (Corps.instance != null && Tete.instance != null)
        {
            line.SetPosition(0, Tete.instance.transform.position);
            line.SetPosition(1, Corps.instance.transform.position);
        }
    }
}
