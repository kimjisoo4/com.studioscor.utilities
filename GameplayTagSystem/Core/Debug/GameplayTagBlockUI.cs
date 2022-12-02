using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StudioScor.GameplayTagSystem
{
    public class GameplayTagBlockUI : MonoBehaviour
    {
        [SerializeField] private Text Name;
        [SerializeField] private Text Count;

        public void SetText(KeyValuePair<GameplayTag, int> tag)
        {
            Name.text = tag.Key.name;
            Count.text = tag.Value.ToString();
        }
        public void SetText(string name, int count)
        {
            Name.text = name;
            Count.text = count.ToString();
        }
    }
}
