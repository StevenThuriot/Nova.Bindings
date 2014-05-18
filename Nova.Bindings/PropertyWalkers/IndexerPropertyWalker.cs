#region License
//  
// Copyright 2014 Steven Thuriot
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 
#endregion

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