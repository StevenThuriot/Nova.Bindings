Nova Bindings [![Build status](https://ci.appveyor.com/api/projects/status/h8tgvuk2xju8e2t7)](https://ci.appveyor.com/project/StevenThuriot/nova-bindings)
====

* Model first approach to WPF view designs.
* Ability to define metadata on your models to shape your view through the use of attributes.

* Contains:
	* LabelFor markup extension 
	* ValueBinding markup extension
	
These markup extensions will resolve the property path passed to them and configure your labels and controls accordingly.


#Sample

```xml

<TextBlock   Grid.Column="0" Text="{LabelFor Model.Property}" />
<ValueEditor Grid.Column="1" Text="{ValueBinding Model.Property}" />

```
