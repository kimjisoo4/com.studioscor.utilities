using UnityEngine;

namespace StudioScor.Utilities
{
    public static partial class Utility
    {
        public static int LayerMaskToLayer(LayerMask layerMask)
        {
            int layerNumber = 0;
            int layer = layerMask.value;
            
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }

            return layerNumber - 1;
        }
        public static int LayerMaskToLayer(int layer)
        {
            int layerNumber = 0;

            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }

            return layerNumber - 1;
        }
    }
}
