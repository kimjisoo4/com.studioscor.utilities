using UnityEngine;


namespace StudioScor.Utilities
{
    public static class DilationSystemUtility
    {
        public static IDilationSystem GetDilationSystem(this GameObject gameObject)
        {
            return gameObject.GetComponent<IDilationSystem>();
        }
        public static IDilationSystem GetDilationSystem(this Component component)
        {
            return component.gameObject.GetDilationSystem();
        }
        public static bool TryGetDilationSystem(this GameObject gameObject, out IDilationSystem dilationSystem)
        {
            dilationSystem = gameObject.GetDilationSystem();

            return dilationSystem is not null;
        }
        public static bool TryGetDilationSystem(this Component component, out IDilationSystem dilationSystem)
        {
            return component.gameObject.TryGetDilationSystem(out dilationSystem);
        }

        public static bool TrySetDilation(this GameObject gameObject, float newDilation)
        {
            if(gameObject.TryGetDilationSystem(out IDilationSystem dilationSystem))
            {
                dilationSystem.SetDilation(newDilation);
                
                return true;
            }

            return false;
        }
        public static bool TrySetDilation(this Component component, float newDilation)
        {
            return TrySetDilation(component.gameObject, newDilation);
        }

        public static bool TryResetDilation(this GameObject gameObject)
        {
            if(gameObject.TryGetDilationSystem(out IDilationSystem dilationSystem))
            {
                dilationSystem.ResetDilation();
                return true;
            }

            return false;
        }
        public static bool TryResetDilation(this Component component)
        {
            return component.gameObject.TryResetDilation();
        }
    }
}