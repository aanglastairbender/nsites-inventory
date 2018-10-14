using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NSites.Global
{
    class GlobalClassHandler
    {
        public Type createObjectFromClass(string pObject)
        {
            Type _Type = null;
            Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "NSites.ApplicationObjects.Classes");
            for (int i = 0; i < typelist.Length; i++)
            {
                if (pObject == typelist[i].Name)
                {
                    _Type = typelist[i];
                }
            }
            return _Type;
        }

        public Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }
    }
}
