using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashBehaviourScript : MonoBehaviour
{
    const float TimeOut = 5.0f;
    enum SplashStates { Moving, Finish }
    SplashStates State;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        State = SplashStates.Moving;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case SplashStates.Moving:
                if (Time.time - startTime > TimeOut)
                    State = SplashStates.Finish;
                if (Input.GetKey(KeyCode.Escape) ||
                    Input.GetKey(KeyCode.Return) ||
                    Input.GetKey(KeyCode.Space))
                    State = SplashStates.Finish;
                break;
            case SplashStates.Finish:
                SceneManager.LoadScene("MenuPrincipal");
                break;
            default: break;
        }
    }
}