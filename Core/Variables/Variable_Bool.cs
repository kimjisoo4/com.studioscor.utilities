using UnityEngine;

namespace StudioScor.Utilities
{
    [CreateAssetMenu(menuName = "StudioScor/Utilities/Variable/new Bool Variable", fileName = "Variable_Bool_")]
    public class Variable_Bool : VariableObject<bool>
    {
        public override void SetValue(bool value)
        {
            if (value == _runtimeValue)
                return;

            _runtimeValue = value;

            Callback_OnChangeValue(!_runtimeValue);
        }

        protected override void OnLoadData()
        {
            if(PlayerPrefs.HasKey(_id))
            {
                int value = PlayerPrefs.GetInt(_id);

                _runtimeValue = value != 0;
            }
        }

        protected override void OnSaveData()
        {
            PlayerPrefs.SetInt(_id, Value ? 1 : 0);
            PlayerPrefs.Save();
        }

        protected override void OnDeleteData()
        {
            if(PlayerPrefs.HasKey(_id))
            {
                PlayerPrefs.DeleteKey(_id);
                PlayerPrefs.Save();
            }
        }
    }
}
