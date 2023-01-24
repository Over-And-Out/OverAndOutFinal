using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource = default;
    public AudioSource PlayerAudioSource { get => playerAudioSource; }
     [SerializeField] private AudioClip sonido = default;
    public AudioClip sonidoIni { get => sonido; }

    public deathCanvas canvasMuerte;
    public GameObject playerRef;
    public NavMeshAgent agent;
    public FieldOfView visionLejana;
    public FieldOfView visionCercana;
    public FieldOfView visionPeriferica;
    public FieldOfView visionTrasera;
    public Transform[] waypoints;
    public int runSpeed;
    public int walkSpeed;
    MonsterStates State;
    Vector3 target;
    int waypointIndex;
    enum MonsterStates
    {
        Buscando,
        Persiguiendo,
        Atacando
    }
    
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        State = MonsterStates.Buscando;
        UpdateDestination();
    }

    // Update is called once per frame
    void Update()
    {
        switch(State){
            case MonsterStates.Buscando: 
                 if(visionLejana.playerDetected     || 
                    visionCercana.playerDetected    ||
                    visionPeriferica.playerDetected ||
                    visionTrasera.playerDetected){
                    State = MonsterStates.Persiguiendo;
                 }else{
                    if(Vector3.Distance(transform.position, target) < 2)
                    {
                        if(UnityEngine.Random.Range(0,2) == 0) agent.speed = walkSpeed;
                        else agent.speed = runSpeed;
                        UpdateDestination();
                    }
                 }
            break;
            case MonsterStates.Persiguiendo:
                  if(!visionLejana.playerDetected     && 
                     !visionCercana.playerDetected    &&
                     !visionPeriferica.playerDetected &&
                     !visionTrasera.playerDetected){

                    agent.speed = runSpeed;
                    State = MonsterStates.Buscando;
                    UpdateDestination();
                  }else if(Vector3.Distance(transform.position, playerRef.transform.position) <= 1.5){
                    State = MonsterStates.Atacando;
                  }else{
                    agent.speed = runSpeed;
                    agent.SetDestination(playerRef.transform.position);
                  }
                break;
            case MonsterStates.Atacando: 
                FindObjectOfType<GameManager>().EndGame();
                PlayerAudioSource.PlayOneShot(sonido);
                canvasMuerte.gameObject.SetActive(true);
                break;
            default:   
                break;
        }
    }

    void UpdateDestination()
    {
        waypointIndex = UnityEngine.Random.Range(0, waypoints.Length);
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }
}
