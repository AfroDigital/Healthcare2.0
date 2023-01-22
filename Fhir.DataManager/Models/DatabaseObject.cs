﻿using System.Dynamic;

namespace Fhir.DataManager.Models;

public class DatabaseObject : DynamicObject
{
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    public int Count   { get { return dictionary.Count; } }

    public override bool TryGetMember(
    GetMemberBinder binder, out object result)
    {
        // Converting the property name to lowercase
        // so that property names become case-insensitive.
        string name = binder.Name.ToLower();

        // If the property name is found in a dictionary,
        // set the result parameter to the property value and return true.
        // Otherwise, return false.
        return dictionary.TryGetValue(name, out result);
    }

    // If you try to set a value of a property that is
    // not defined in the class, this method is called.
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        // Converting the property name to lowercase
        // so that property names become case-insensitive.
        dictionary[binder.Name.ToLower()] = value;

        // You can always add a value to a dictionary,
        // so this method always returns true.
        return true;
    }
}
