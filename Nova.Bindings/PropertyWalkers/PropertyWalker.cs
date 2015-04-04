using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Nova.Bindings.Metadata;

namespace Nova.Bindings.PropertyWalkers
{
    abstract class PropertyWalker
    {
        private static readonly Dictionary<Type, List<PropertyWalker>> PropertyWalkers = new Dictionary<Type, List<PropertyWalker>>();

        protected PropertyWalker(PropertyInfo propertyInfo)
        {
            PropertyName = propertyInfo.Name;
            PropertyType = propertyInfo.PropertyType;

            SettingsAttribute = propertyInfo.GetAttribute<SettingsAttribute>();
            DynamicSettingsAttribute = propertyInfo.GetAttribute<DynamicSettingsAttribute>();
        }

        public string PropertyName { get; private set; }
        public SettingsAttribute SettingsAttribute { get; private set; }
        public DynamicSettingsAttribute DynamicSettingsAttribute { get; private set; }
        public Func<object, object> Getter { get; protected set; }
        public Type PropertyType { get; private set; }

        public string GetDefinitionId(object model)
        {
            var attribute = SettingsAttribute;
            if (attribute != null)
            {
                return attribute.Id;
            }

            if (DynamicSettingsAttribute != null)
            {
                if (model == null)
                    throw new NotSupportedException("The model must be completely initialized on start when using a dynamic model ({0}).".FormatWith(PropertyName));

                var dynamicRubriekModel = model as IHaveDynamicPropertySettings;

                if (dynamicRubriekModel == null)
                    throw new NotSupportedException("Dynamic models must implement IHaveDynamicPropertySettings ({0}).".FormatWith(PropertyName));

                var settings = dynamicRubriekModel.ProvideDynamicSettings(PropertyName);

                if (string.IsNullOrWhiteSpace(settings))
                    throw new NotSupportedException("Settings cannot be empty ({0}).".FormatWith(PropertyName));

                return settings;
            }

            throw new NotSupportedException("Property '{0}' does not have a settings attribute.".FormatWith(PropertyName));
        }






        public static string GetDefinitionId(object dataContext, PropertyPath path)
        {
#if INFRAGISTICS
            var datarecord = dataContext as Infragistics.Windows.DataPresenter.DataRecord;

            //Infragistics Template item, return null as the binding path won't be resolved anyway and an exception will be thrown.
            if (datarecord != null && datarecord.DataItemIndex == -1) return null;
#endif
            PropertyWalker walker;
            object model;

            if (!RetrievePropertyInfoAndModel(dataContext, path.Path.Split('.'), out walker, out model))
                throw new NotSupportedException("Binding path can't be resolved.");

            return walker.GetDefinitionId(model);
        }

        private static bool RetrievePropertyInfoAndModel(object dataContext, IList<string> splitPath, out PropertyWalker walker, out object model)
        {
            walker = null;
            model = dataContext;

            for (var i = 0; i < splitPath.Count; i++)
            {
                if (model == null && walker == null)
                    return false; //Bad path, both model as walker are null

                var part = splitPath[i];
                string indexer = null;

                var indexerArray = part.Split(new[] { '[', ']' }, 3, StringSplitOptions.RemoveEmptyEntries);
                var hasIndexer = indexerArray.Length > 1;

                if (hasIndexer)
                {
                    part = indexerArray[0];
                    indexer = indexerArray[1];
                }


                var type = model == null
                    ? walker.PropertyType//The model is not completely initialized, so we'll trust the defined property type knows enough.
                    : model.GetType();

                PropertyWalker cachedWalker = null;
                List<PropertyWalker> cachedWalkers;
                if (PropertyWalkers.TryGetValue(type, out cachedWalkers))
                {
                    IEnumerable<PropertyWalker> walkers = cachedWalkers;
                    if (hasIndexer) walkers = walkers.OfType<IndexerPropertyWalker>().Where(x => indexer.Equals(x.Indexer, StringComparison.Ordinal));

                    cachedWalker = walkers.FirstOrDefault(x => x.PropertyName.Equals(part, StringComparison.OrdinalIgnoreCase));
                }

                if (cachedWalker == null)
                {
                    var property = type.GetProperty(part);

                    if (property == null) return false;

                    if (hasIndexer)
                    {
                        cachedWalker = new IndexerPropertyWalker(property, indexer);
                    }
                    else
                    {
                        cachedWalker = new DefaultPropertyWalker(property);
                    }

                    if (cachedWalkers == null)
                    {
                        cachedWalkers = new List<PropertyWalker> { cachedWalker };
                        PropertyWalkers.Add(type, cachedWalkers);
                    }
                    else
                    {
                        cachedWalkers.Add(cachedWalker);
                    }
                }

                walker = cachedWalker;

                if (i + 1 == splitPath.Count) break;
                if (model == null) continue;

                model = walker.Getter(model);
            }

            return true;
        }
    }
}
