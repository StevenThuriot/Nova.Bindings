Nova Bindings [![Build status](https://ci.appveyor.com/api/projects/status/h8tgvuk2xju8e2t7)](https://ci.appveyor.com/project/StevenThuriot/nova-bindings)
====

* Model first approach to WPF view designs.
* Ability to define metadata on your models to shape your view through the use of attributes.

* Contains:
	* LabelFor markup extension 
	* ValueBinding markup extension
	
These markup extensions will resolve the property path passed to them and configure your labels and controls accordingly.


#Usage

##Views

```xml

<TextBlock   Grid.Column="0" Text="{LabelFor Model.Property}" />
<ValueEditor Grid.Column="1" Text="{ValueBinding Model.Property}" />

```

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
	public IDefinition GetDefinition(string id)
	{
		var definition = //TODO!
		return definition;
	}
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
Nova.Bindings.ValueEditor.ValueTextEditor
Nova.Bindings.ValueEditor.ValueCheckBoxEditor
Nova.Bindings.ValueEditor.ValueRadioButtonEditor
Nova.Bindings.ValueEditor.ValueComboBoxEditor
```

###Templates

A template can easily be added by adding a similar template into the App's Resource Dictionary. Samples can be found [here](/Nova.Bindings/ValueEditor.xaml).

```xml
<ControlTemplate x:Key="ValueTextEditor" TargetType="n:ValueEditor">
    <TextBox x:Name="PART_ValueEditor"
             Text="{Binding Value, RelativeSource={RelativeSource TemplatedParent}, Mode=TwoWay}" />
</ControlTemplate>
```
