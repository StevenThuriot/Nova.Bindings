using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Nova.Bindings.PropertyWalkers
{
    internal class DefaultPropertyWalker : PropertyWalker
    {
        public DefaultPropertyWalker(PropertyInfo propertyInfo)
            : base (propertyInfo)
        {
            Getter = CompileGetter(propertyInfo);
        }

        private static Func<object, object> CompileGetter(PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();
            var declaringType = getMethod.DeclaringType;

            var obj = Expression.Parameter(typeof(object), "model");
            var expr = Expression.Lambda<Func<object, object>>
                (
                    Expression.Convert
                        (
                            Expression.Call
                                (
                                    Expression.Convert(obj, declaringType),
                                    getMethod
                                ), typeof(object)
                        ),
                    obj
                );

            return expr.Compile();
        }
    }
}