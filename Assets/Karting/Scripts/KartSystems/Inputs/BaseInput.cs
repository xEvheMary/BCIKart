using UnityEngine;

namespace KartGame.KartSystems
{
    public struct InputData
    {
        public bool Accelerate;
        public bool Brake;
        public float TurnInput;
    }

    // Newer Version
    public interface IInput
    {
        InputData GenerateInput();

        bool IsActive();
    }

    public abstract class BaseInput : MonoBehaviour, IInput
    {
        /// <summary>
        /// Override this function to generate an XY input that can be used to steer and control the car.
        /// </summary>
        public abstract InputData GenerateInput();
        public abstract bool IsActive();
    }

    // Older version
        public interface IInputAlt
    {
        Vector2 GenerateInput();
    }

    public abstract class BaseInputAlt : MonoBehaviour, IInputAlt
    {
        /// <summary>
        /// Override this function to generate an XY input that can be used to steer and control the car.
        /// </summary>
        public abstract Vector2 GenerateInput();
    }
}
