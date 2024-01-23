using UnityEngine;

namespace KartGame.KartSystems {

    public class LSLInput : BaseInput
    {
        [SerializeField] private bool accelerateFlag = true;
        [SerializeField] private bool brakeFlag = false;
        [SerializeField] private float steerScale = 1f;
        Controller controller;
        
        void Start(){
            controller = FindObjectOfType<Controller>();
        }
        
        public override InputData GenerateInput() {
            float lr = controller.GetSteerVal();
            accelerateFlag = controller.GetAccelBool();
            return new InputData
            {
                Accelerate = accelerateFlag,
                Brake = brakeFlag,
                TurnInput = lr * steerScale
            };
        }

        public override bool IsActive(){
           return controller.GetConnectedFlag();
        }
    }
}
