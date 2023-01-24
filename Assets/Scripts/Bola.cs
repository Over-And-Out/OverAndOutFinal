using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bola : MonoBehaviour
{
    public enum AIStates {idleState, movingState, diedState, maxAIStates};

    public AIStates state = AIStates.idleState;

    public const int idleState = 0,
                     movingState = 1,
                     diedState = 2;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
