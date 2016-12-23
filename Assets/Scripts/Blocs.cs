using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blocs : MonoBehaviour
{
    public enum BLOC_TYPE
    {
        GROUND,
        OBSTACLE,
        OBUS,
        SNAPER,
        INFO,
        END
    };

    public BLOC_TYPE type;
    public string blocChar = "";

    Vector3 deplacement = Vector3.left;

    void FixedUpdate()
    {
        switch (type)
        {
            case BLOC_TYPE.OBUS:
                {
                    transform.position += deplacement * .1f;

                    if (this.transform.position.x < 0)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }

            case BLOC_TYPE.SNAPER:
                {
                    break;
                }
        }

    }

    public void ActionOnEnter()
    {
        switch (type)
        {
            case BLOC_TYPE.GROUND:
                {
                    break;
                }
            case BLOC_TYPE.SNAPER:
                {
                    Manager.instance.snap = true;
                    break;
                }
            case BLOC_TYPE.OBSTACLE:
                {
                    Manager.instance.Fail();
                    break;
                }
            case BLOC_TYPE.END:
                {
                    Manager.instance.Victory();
                    break;
                }
            case BLOC_TYPE.OBUS :
                {
                    Manager.instance.Fail();
                    //Destroy(this.gameObject, .1f);
                    break;
                }
        }
    }


    public void ActionOnExit()
    {
        switch (type)
        {
            case BLOC_TYPE.GROUND:
                {
                    break;
                }
            case BLOC_TYPE.SNAPER:
                {
                    Manager.instance.snap = false;
                    break;
                }
            case BLOC_TYPE.OBSTACLE:
                {
                    break;
                }
        }
    }
}
