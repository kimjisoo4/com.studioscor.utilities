#if SCOR_ENABLE_VISUALSCRIPTING
using System;
using System.Collections.Generic;
using System.Reflection;
namespace StudioScor.Utilities.VisualScripting.Editor.Community
{
    public static partial class HUMType_Children
    {
        public static Type[] Derived(this HUMType.Data.Get derived, bool includeSelf = false)
        {
            List<Type> result = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int assembly = 0; assembly < assemblies.Length; assembly++)
            {
                Type[] types = assemblies[assembly].GetTypes();

                for (int type = 0; type < types.Length; type++)
                {
                    if ((!types[type].IsAbstract && !types[type].IsInterface) && derived.type.IsAssignableFrom(types[type]))
                    {
                        result.Add(types[type]);
                    }
                }
            }
            if (includeSelf) result.Add(derived.type);
            return result.ToArray();
        }
    }
}
#endif