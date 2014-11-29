Nova Bindings [![Build status](https://ci.appveyor.com/api/projects/status/h8tgvuk2xju8e2t7)](https://ci.appveyor.com/project/StevenThuriot/nova-bindings)
====

* Model first approach to WPF view designs. 
* Decouple view markup from your data. Instead, define metadata on your (view)models to shape your view through the use of attributes. 
* You can even go a step further and retrieve the metadata at startup from your server's database. This allows you to change field types and labels without having to redeploy.
* Contains:
	* LabelFor markup extension 
	* ValueBinding markup extension
	
These markup extensions will resolve the property path passed to them and configure your labels and controls accordingly.


#Usage

Merge Nova.Bindings's pack URI to your project's ResourceDictionary.

```xml
<ResourceDictionary.MergedDictionaries>
		<ResourceDictionary Source="pack://application:,,,/Nova.Bindings;component/ValueEditor.xaml" />
</ResourceDictionary.MergedDictionaries>
```

##Views

Views become as simple as the following Label and Editor. It will be the only editor you'll ever use again!

A change in the model? No problem! The Bindings will take care of it for you!

```xml

<TextBlock   Grid.Column="0" Text="{LabelFor Model.Property}" />
<ValueEditor Grid.Column="1" Value="{ValueBinding Model.Property}" />

```

The `LabelFor` binding has a property `AppendColon`, default `true`, which will append a colon to the label.

The `ValueBinding` binding has two properties that can be set:
* Mode ( == BindingMode, default BindingMode.Default )
* Converter ( Default null )

##Implementation

Implement IHaveSettingsManager on your ViewModel.
The bindings will try to resolve this Manager (from the control up) to try and find the control's settings.

###ViewModel

```csharp
public class ViewModel : IHaveSettingsManager
{
	public ISettingsManager SettingsManager { get; private set; }
	
	public ViewModel()
	{
		SettingsManager = new NovaSettingsManager();
	}
}
```

###Settings Manager

Next, create a settings manager.

```csharp
public class NovaSettingsManager : ISettingsManager
{
	private readonly Dictionary<string, IDefinition> _definitions;
	private readonly DefinitionFactory _factory;
	
	public NovaSettingsManager()
	{
		_definitions = new Dictionary<string, IDefinition>();
		
		//TODO;
		_factory = new ComboBoxFactory(); //Good starter since it's a special case.
		_factory.SetSuccessor<RadioButtonFactory>()
			    .SetSuccessor<CheckBoxFactory>()
			    .SetSuccessor<DatePickerFactory>()
			    //.......... 
			    .SetSuccessor<TextBoxFactory>(); //Decent Fallback in case nothing matches.
	}
	
	public IDefinition GetDefinition(string id)
	{
		IDefinition definition;
		
		if (_definitions.TryGetValue(id, out definition))
		{
			return definition;
		}
		
		definition = _factory.Create(id);
		_definitions.Add(id, definition);
		return definition;
	}
}


//Sample [Chain Of Responsilibity](https://www.dofactory.com/net/chain-of-responsibility-design-pattern)
abstract class DefinitionFactory
{
	protected DefinitionFactory _successor;
	
	public DefinitionFactory SetSuccessor(DefinitionFactory successor)
	{
		return _successor = successor;
	}

	public DefinitionFactory SetSuccessor<T>()
		where T : DefinitionFactory, new()
	{
		return _successor = new T();
	}
	
	public IDefinition Create(string id)
	{
		var definition = CreateDefinition(id);
		
		if (definition != null)
			return definition;
		
		if (_successor != null)
		{
			definition = _successor.Create(id);
			
			if (definition != null)
				return definition;
		}
		
		throw new NotSupportedException(id);
	}
	
	protected abstract IDefinition CreateDefinition(string id);
}
```

###Meta Data

The id the `GetDefinition` method receives is determined by the metadata you have to place on the model. (Hence **model first**)
This is done by adding attributes.

```csharp
	[Settings("PersonName")]
	public string Name { get; set;}
```

In this specific case, the id will be *PersonName*.
Sometimes, fields can have multiple definitions, depending on the situations. Imagine a class where a second property determins if the decorated property is shown as a combobox or a normal text field. This can't be decorated by a simple attribute.

In this case, we can use a second provided attribute.

```csharp
	[DynamicSettings("SomeDynamicProperty")]
	public string Property { get; set;}
```

When using `Dynamic Settings`, the class that has said property _must_ implement `IHaveDynamicPropertySettings`.

```csharp
public class Model : IHaveDynamicPropertySettings
{
	public string ProvideDynamicSettings(string field)
	{
		if (field == "SomeDynamicProperty")
		{
			if (SecondProperty == "Something")
			{
				return "PersonName";
			}
			else
			{
				return "PersonList";
			}
		}
		
		return field;
	}
}
```

The string returned will be the id used in the settings manager to find the correct `IDefinition`.

###Definitions

####Default

A definition has the following properties:

```csharp
public interface IDefinition
{
	string Id { get; }
	string Label { get; }
	string Editor { get; }
}
```

* `Id` is the id from the `GetDefinition` method.
* `Label` is the text used for the `LabelFor` extension.
* `Editor` is the type of editor that needs to be used.
 
This will do for most settings. Two control types require a specific interface to be implemented;

####ComboBox

```csharp
public interface IComboBoxDefinition : IDefinition
{
    IEnumerable ItemsSource { get; }
}
```

####RadioButton

```csharp
public interface IRadioButtonDefinition : IDefinition
{
    string GroupName { get; }
}
```

To make life easier, constants are available! The `Editor` is defined as a string rather than an enum to make it easier to add your own implementations!

```
Nova.Bindings.ValueEditor.Definitions.ValueTextEditor
Nova.Bindings.ValueEditor.Definitions.ValueCheckBoxEditor
Nova.Bindings.ValueEditor.Definitions.ValueRadioButtonEditor
Nova.Bindings.ValueEditor.Definitions.ValueComboBoxEditor
```

###Templates

A template can easily be added by adding a similar template into the App's Resource Dictionary. Samples can be found [here](/Nova.Bindings/ValueEditor.xaml).

Note that the Template keys are the same as the Editor constants that IDefinition supplies!

```xml
<ControlTemplate x:Key="ValueTextEditor" TargetType="n:ValueEditor">
    <TextBox x:Name="PART_ValueEditor"
             Text="{Binding Value, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}" />
</ControlTemplate>
```
