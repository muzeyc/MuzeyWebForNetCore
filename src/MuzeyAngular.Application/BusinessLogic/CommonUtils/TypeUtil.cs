using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class TypeUtil
    {
        private static Dictionary<string, Type> TypeManagerDic = new Dictionary<string, Type>();
        /// <summary>
        /// 类合并
        /// </summary>
        /// <param name="typeDic"></param>
        /// <returns></returns>
        public static Type TypeMerge(List<Type> typeList)
        {
            IDictionary<string, Type> Properties = new Dictionary<string, Type>();
            string className = "";
            foreach (var t in typeList)
            {
                className += string.IsNullOrEmpty(className) ? t.Name : "__" + t.Name;
                foreach (var p in t.GetProperties())
                {
                    Properties.Add(new KeyValuePair<string, Type>(t.Name + "__" + p.Name, p.PropertyType));
                }
            }

            return TypeManagerDic.ContainsKey(className) ? TypeManagerDic[className] : MuzeyTypeFactory(className, Properties);
        }

        /// <summary>
        /// Dto专用合并
        /// </summary>
        /// <param name="typeDic"></param>
        /// <returns></returns>
        public static Type DtoMerge(List<Type> typeList)
        {
            IDictionary<string, Type> Properties = new Dictionary<string, Type>();
            string className = "";
            foreach (var t in typeList)
            {
                string tName = t.Name.Replace("Dto","");
                className += string.IsNullOrEmpty(className) ? tName : "__" + tName;
                foreach (var p in t.GetProperties())
                {
                    Properties.Add(new KeyValuePair<string, Type>(tName + "__" + p.Name, p.PropertyType));
                }
            }

            return TypeManagerDic.ContainsKey(className) ? TypeManagerDic[className] : MuzeyTypeFactory(className, Properties);
        }

        private static Type MuzeyTypeFactory(string ClassName, IDictionary<string, Type> Properties)
        {
            //应用程序域
            AppDomain currentDomain = System.Threading.Thread.GetDomain(); //AppDomain.CurrentDomain;
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("MuzeyAssembly"), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MuzeyModule");

            TypeBuilder typeBuilder = moduleBuilder.DefineType(ClassName, TypeAttributes.Public);
            foreach (var kv in Properties)
            {
                //动态创建属性
                PropertyBuilder pb = typeBuilder.DefineProperty(kv.Key, PropertyAttributes.HasDefault, kv.Value, null);

                FieldBuilder customerNameBldr = typeBuilder.DefineField("_" + kv.Key,
                                                kv.Value,
                                                FieldAttributes.Private);

                // The property set and property get methods require a special
                // set of attributes.
                MethodAttributes getSetAttr =
                    MethodAttributes.Public | MethodAttributes.SpecialName |
                        MethodAttributes.HideBySig;

                // Define the "get" accessor method for CustomerName.
                MethodBuilder custNameGetPropMthdBldr =
                    typeBuilder.DefineMethod("get_" + kv.Key,
                                               getSetAttr,
                                               kv.Value,
                                               Type.EmptyTypes);

                ILGenerator custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();

                custNameGetIL.Emit(OpCodes.Ldarg_0);
                custNameGetIL.Emit(OpCodes.Ldfld, customerNameBldr);
                custNameGetIL.Emit(OpCodes.Ret);

                // Define the "set" accessor method for CustomerName.
                MethodBuilder custNameSetPropMthdBldr =
                    typeBuilder.DefineMethod("set_" + kv.Key,
                                               getSetAttr,
                                               null,
                                               new Type[] { kv.Value });

                ILGenerator custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

                custNameSetIL.Emit(OpCodes.Ldarg_0);
                custNameSetIL.Emit(OpCodes.Ldarg_1);
                custNameSetIL.Emit(OpCodes.Stfld, customerNameBldr);
                custNameSetIL.Emit(OpCodes.Ret);

                // Last, we must map the two methods created above to our PropertyBuilder to 
                // their corresponding behaviors, "get" and "set" respectively. 
                pb.SetGetMethod(custNameGetPropMthdBldr);
                pb.SetSetMethod(custNameSetPropMthdBldr);
            }

            var t = typeBuilder.CreateType();
            TypeManagerDic.Add(t.Name, t);
            return t;
        }
    }
}
