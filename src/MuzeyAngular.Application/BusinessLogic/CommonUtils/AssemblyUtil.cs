using CommonUtils;
using MuzeyServer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommonUtils
{
    public static class AssemblyUtil
    {
        public static Dictionary<string, Type> typeDic;
        public static Assembly mockAssembly;

        static AssemblyUtil()
        {
            typeDic = new Dictionary<string, Type>();
            mockAssembly = Assembly.GetExecutingAssembly();
            var ts = mockAssembly.GetTypes();
            foreach(Type t in ts)
            {
                typeDic.Add(t.FullName, t);
            }
        }

        public static object DeSerializerModel(this Type t, string json)
        {
            var MethodType = typeof(JsonUtil);
            var GenericMethod = MethodType.GetMethod("DeserializeJSON", new Type[] { typeof(string) });
            MethodInfo curMethod = GenericMethod.MakeGenericMethod(new Type[] { t });
            return curMethod.Invoke(null, new object[] { json });
        }
    }
}