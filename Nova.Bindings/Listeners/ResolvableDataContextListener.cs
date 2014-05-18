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

using System.Windows;
using Nova.Bindings.Base;

namespace Nova.Bindings.Listeners
{
    abstract class ResolvableDataContextListener<T> : DataContextListener<T> 
        where T : FrameworkElement
    {
        private readonly ResolveMode _resolveMode;

        protected ResolvableDataContextListener(PropertyPath path, ResolveMode resolveMode) 
            : base(path)
        {
            _resolveMode = resolveMode;
        }
        protected override void OnDataContextChanged(T element, object dataContext)
        {
            if (OnResolvableDataContextChanged(element, dataContext))
            {
                if (_resolveMode == ResolveMode.Once) DetachFrom(element);
            }
        }

        protected abstract bool OnResolvableDataContextChanged(T element, object dataContext);
    }
}