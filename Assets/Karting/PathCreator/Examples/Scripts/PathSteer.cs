using UnityEngine;
using KartGame.KartSystems;
using PathCreation;
using System.Collections.Generic;
using System;


// Moves along a path at constant speed.
// Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.


public class PathSteer : BaseInput
    {
        [Header("Path Creator")]
        [SerializeField] private List<PathCreator> pathCreator;
        PathCreator activePath;
        //public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        
        [Header("Control")]
        public ArcadeKart Kart;
        Controller controller;
        [SerializeField] private bool accelerateFlag = true;
        [SerializeField] private bool brakeFlag = false;
        [SerializeField] private float maxAngle = 48f;
        [SerializeField] private bool isActive = true;
        private float currentDistance;
        [SerializeField] private float waypointDistance = 2f;
        private Vector3 targetPos;

        void Start() {
            if (pathCreator.Count > 0)
            {   
                int i = 0;
                if (pathCreator.Count > 1){i = 1;}
                FindActivePath(pathCreator, i);
                activePath.pathUpdated += OnPathChanged;
            }
            else{
                Debug.LogError("Need that path creator");
            }
            controller = FindObjectOfType<Controller>();
            //BaselineTrackToggle.OnTrackChange += TrackToggle_OnTrackChange;
        }

        private void TrackToggle_OnTrackChange(object sender, Controller.OnCueArgs e)
        {
            FindActivePath(pathCreator, e.cueClass);
            OnPathChanged();
        }

        void Update()
        {

        }

        public void FindActivePath(List<PathCreator> x, int idx = 0){
            activePath = x[idx];
        }

        void OnPathChanged() {
            currentDistance = activePath.path.GetClosestDistanceAlongPath(transform.position);
        }

        public override InputData GenerateInput()
        {
            currentDistance = activePath.path.GetClosestDistanceAlongPath(Kart.gameObject.transform.position);
            float targetDistance = currentDistance + waypointDistance;
            targetPos = activePath.path.GetPointAtDistance(targetDistance);

            Vector3 dirToMove = (targetPos - Kart.gameObject.transform.position).normalized;    // Find out direction
            float res = Vector3.Dot(Kart.transform.forward, dirToMove);                         // 1 -> forward | -1 -> behind

            float angleToTarget = Vector3.SignedAngle(Kart.transform.forward, dirToMove, Vector3.up);
            float normalizedAngle = angleToTarget / maxAngle;
            normalizedAngle = Mathf.Round(normalizedAngle * 100) / (float)100.0;
            float normalizedSteer = Mathf.Clamp(normalizedAngle, -1f, 1f);

            accelerateFlag = controller.GetAccelBool();
            return new InputData
            {
                Accelerate = accelerateFlag,
                Brake = brakeFlag,
                TurnInput = normalizedSteer
            };
        }

        public override bool IsActive(){
           return isActive;
        }
    }