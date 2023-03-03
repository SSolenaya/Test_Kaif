using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalParams", menuName = "ScriptableObjects/GlobalParams", order = 1)]
public class GlobalParamsSO : ScriptableObject
{
    public float ballRadiusOfBlast = 60f;                       //  radius for blast of ball  - cubes are destroying within this zone
    public float coolDownForShoot = 3f;                         //  cooldown before next shoot
    public float coolDownForNewBall = 6f;                       //  cooldown before next ball
    public int startBallsAmount = 3;                            //  start balls
    public int maxBallsAmount = 5;                              //  max available amount of balls

    public float minRecreatingCubeTime = 1f;                    //  min time before new cube will be created 
    public float maxRecreatingCubeTime = 3f;                    //  max time before new cube will be created 
    public int   cubeAmount = 3;                                //  max cubes amount on scene     

    public float cubeDashSpeed = 100f;                          //  cube's moving speed during dash
    public float cubeNormalSpeed = 50f;                         //  cube's normal speed
    public float cubeNormalRotationSpeed = 5f;                  //  cube's normal rotation speed
    public float cubeMaxRotationSpeed = 50f;                    //  max rotation speed
    public float cubeDashDeltaMinTime = 3f;                     //  min time before next dash
    public float cubeDashDeltaMaxTime = 6f;                     //  max time before next dash
    public float cubeDashTime = 0.5f;                           //  dash duration
    public float cubeDistance = 0.5f;                           //  distance to target for successful movement ending
    public float cubeTheorDisToCalculateRotation = 15;          //  additional value for rotation
    public float cubeMinRadiusForNewPoint = 80f;                //  min distance for cube's new destination 


    public float ballSpeed = 180f;
    public float ballAmplitude = 120f;                          //  for parabolic flight  

    public float boardMapOffset = 40f;                          //  map boarder offset
}
