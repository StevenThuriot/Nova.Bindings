using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nova.Bindings.PropertyWalkers
{
    internal class IndexerPropertyWalker : PropertyWalker
    {
        public IndexerPropertyWalker(PropertyInfo propertyInfo, string index)
            : base(propertyInfo)
        {
            Indexer = index;
            Getter = CompileGetter(propertyInfo, index);
        }

        public string Indexer { get; private set; }

        private static Func<object, object> CompileGetter(PropertyInfo propertyInfo, string indexerKey)
        {
            var getMethod = propertyInfo.GetGetMethod();
            var declaringType = getMethod.DeclaringType;
            var returnType = getMethod.ReturnType;

            var indexerProperties = returnType.GetProperties();

            var indexer =  indexerProperties.FirstOrDefault(x => FindIndexerByType(x, typeof(string))) 
                            ?? indexerProperties.First     (x => FindIndexerByType(x, typeof(object)));

            var obj = Expression.Parameter(typeof(object), "model");

            var getCollection = Expression.Call(Expression.Convert(obj, declaringType), getMethod);
            var getItem = Expression.Property(getCollection, indexer, Expression.Constant(indexerKey));
            var box = Expression.Convert(getItem, typeof(object));

            var expr = Expression.Lambda<Func<object, object>>(box, obj);

            return expr.Compile();
        }

        private static bool FindIndexerByType(PropertyInfo propertyInfo, Type type)
        {
            var indexParameters = propertyInfo.GetIndexParameters();
            if (indexParameters.Length != 1) return false;

            return indexParameters[0].ParameterType == type;
        }
    }
}