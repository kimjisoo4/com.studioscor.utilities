using UnityEngine;

namespace StudioScor.Utilities
{
    public class RolloverNumber
    {
        public int PrevNumber { get; private set; } = -1;
        private readonly int _maxNumber;

        public RolloverNumber(int maxNumber)
        {
            _maxNumber = maxNumber;
        }

        public bool CheckNewerSequence(int number)
        {
            if (PrevNumber < 0)
            {
                PrevNumber = number;
                return true;
            }
            else
            {
                int diff = (number - PrevNumber + _maxNumber) % _maxNumber;

                if (diff > 0 && diff < (_maxNumber / 2))
                {
                    PrevNumber = number;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
