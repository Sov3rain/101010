using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour
{
    public enum TYPE
    {
        BACK001,
        BACK002,
        SNAP
    };

    public TYPE type;
    public float scrollSpeed = .1F;
    public float scrollSpeed2 = .2F;
    public float scrollSpeed3 = .5f;
    
    void Update()
    {
        switch(type)
        {
            case TYPE.BACK001:
                {
                    float offset = Time.time * scrollSpeed;
                    GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
                    break;
                }
            case TYPE.BACK002 :
                {
                    float offset = Time.time * scrollSpeed2;
                    GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
                    break;
                }
            case TYPE.SNAP:
                {
                    float offset = Time.time * scrollSpeed3;
                    GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
                    break;
                }
        }   
    }
}
