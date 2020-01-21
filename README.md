# PersistentComponent
 Scene-independent Component data storage and retrieval for Unity
 
 **Requires:** Unity Technologies [Guid Based Reference](https://github.com/Unity-Technologies/guid-based-reference)

### Yeah but why?
 Imagine your project has a town where building interiors are different scenes.
 <br>Moving from
 ```
	Town Scene -> House Scene -> Town Scene
 ```
 will place your character back at whatever the default position of the Town Scene is, rather than the doorway of the house.
 Of course you could store the the position of the player before they enter a new scene, then reassign it as they come back, but...
 
 PersistentComponent provides means to do so generically, for pretty much any component!
 
### Yeah but how?
 Using a Globally Unique Identifier (GUID), the PersistentData class maps a GameObject's GUID to a dictionary where the key is a Type, and the value is an array of C# objects.
 Using this nested dictionary, user-defined data that defines the component can be stored and retrieved, even between scenes.
 In order to store a component's data, the user must inherit from PersistentComponent, and implement two functions:
 * Decompose
 * Compose
 
 Decompose turns a Component into an array of objects representing the data that is necessary to store for that component.
 Example:
 ```C#
 protected override object[] Decompose(Transform component)
 {
     // Note the order of the array; Order will be important for reassignment
     return new object[] {component.position, component.rotation, component.localScale};
 }
 ```
 Compose takes the same array of objects and reassigns the data to the Component
 Example:
 ```C#
 protected override void Compose(Transform component, object[] values)
 {
     // Reassign the object array elements to their corresponding component fields
     transform.position = TryResolve<Vector3>(values, 0);
     transform.rotation = TryResolve<Quaternion>(values, 1);
     transform.localScale = TryResolve<Vector3>(values, 2);
 }
 ```
 
### Pitfalls!
 Likely the most obvious concern here is that Persistent component casts up to an object, then back down to the original type.  There's a good chance that if implemented incorrectly, the resulting down-cast will be invalid, or even that the array index being checked is out of range.
 To remediate this concern, the TryResolve method attempts to check that both the index and cast are valid, returning the type's default value otherwise.  Of course this isn't perfect, but realistically, the Compose/Decompose methods are the only way to reassign data, and as long as they are implemented with care, the possibilty of 
 invalid indices/casts shouldn't arise.
 
 Additionally, you cannot store multiples of the same Component type under a single GUID.
 
 Lastly, the user is wholly responsible for maintaining the data in PersistentData.  It must be manually removed whenever it is no longer required.
 
### Some Further Notes:
 Implementation using reflection should also be possible here, but considering Unity components cannot be created with the new keyword, fields must be stored individually in a particularly messy manner.  Considering this, the high additional cost of reflection, and the inability (or rather ease) for the user to define which fields are necessary to store,
 I decided using reflection for creating persistent Components was less useful.
