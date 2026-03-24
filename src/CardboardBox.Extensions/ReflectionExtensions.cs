using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace CardboardBox.Extensions;

/// <summary>
/// The extensions for reflection
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="enumVal">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
    public static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }

    /// <summary>
    /// Checks to see if the object can be cast to the type
    /// </summary>
    /// <typeparam name="T">The type to check</typeparam>
    /// <param name="input">The type being cast</param>
    /// <returns>Whether or not the given object can be cast to the given type</returns>
    public static bool OfType<T>(this object input)
    {
        return input.GetType().OfType<T>();
    }

    /// <summary>
    /// Checks to see if the object can be cast to the type
    /// </summary>
    /// <typeparam name="T">The type to check</typeparam>
    /// <param name="input">The type being cast</param>
    /// <param name="type">The casted object</param>
    /// <returns>Whether or not the given object can be cast to the given type</returns>
    public static bool OfType<T>(this object input, [MaybeNullWhen(false)] out T type)
    {
        if (!input.GetType().OfType<T>())
        {
            type = default!;
            return false;
        }

        type = (T)input;
        return true;
    }

    /// <summary>
    /// Checks to see if the type can be cast to the other type
    /// </summary>
    /// <typeparam name="T">The type to check</typeparam>
    /// <param name="type">The type being cast</param>
    /// <returns>Whether or not the given type can be cast to the given type</returns>
    public static bool OfType<T>(this Type type)
    {
        var inf = typeof(T);
        return inf.IsAssignableFrom(type);
    }

    /// <summary>
    /// Gets all of the values for the given enum
    /// </summary>
    /// <typeparam name="T">The type of enum</typeparam>
    /// <param name="value">The enum</param>
    /// <param name="onlyBits">Whether or not to only get enum values that are base 2</param>
    /// <returns>All of the enum values</returns>
    public static IEnumerable<T> Flags<T>(this T value, bool onlyBits = false) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        var ops = values.Where(x => value.HasFlag(x));

        if (onlyBits)
            ops = ops.Where(x => ((int)(object)x & ((int)(object)x - 1)) == 0);

        return ops;
    }

    /// <summary>
    /// Gets all of the values for the given enum that match the predicate
    /// </summary>
    /// <typeparam name="T">The type of enum</typeparam>
    /// <param name="value">The enum</param>
    /// <param name="predicate">The predicate</param>
    /// <returns>All of the values of the enum that match the predicate</returns>
    public static IEnumerable<T> Flags<T>(this T value, Func<T, bool> predicate) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        return values.Where(x => value.HasFlag(x) && predicate(x));
    }

    /// <summary>
    /// Get all of the values for the given enum
    /// </summary>
    /// <typeparam name="T">The type of enum</typeparam>
    /// <param name="_">The enum type</param>
    /// <returns>All of the values of the enum</returns>
    public static IEnumerable<T> AllFlags<T>(this T _) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    /// <summary>
    /// Uses reflection to get the field value from an object.
    /// </summary>
    /// <param name="instance">The instance object.</param>
    /// <param name="fieldName">The field's name which is to be fetched.</param>
    /// <returns>The field value from the object.</returns>
    public static object? GetPrivateFieldValue<T>(this T instance, string fieldName)
    {
        var bindFlags =
            BindingFlags.Instance | BindingFlags.Public
            | BindingFlags.NonPublic | BindingFlags.Static;
        var field = typeof(T).GetField(fieldName, bindFlags);
        return field?.GetValue(instance);
    }

    /// <summary>
    /// Uses reflection to get the field value from an object.
    /// </summary>
    /// <param name="instance">The instance object.</param>
    /// <param name="fieldName">The field's name which is to be fetched.</param>
    /// <returns>The field value from the object.</returns>
    public static TOut? GetPrivateFieldValue<T, TOut>(this T instance, string fieldName)
    {
        var value = instance.GetPrivateFieldValue(fieldName);
        return value is not null ? (TOut)value : default;
    }

    /// <summary>
    /// Sets a _private_ Property Value from a given Object. Uses Reflection.
    /// Throws a ArgumentOutOfRangeException if the Property is not found.
    /// </summary>
    /// <param name="obj">Object from where the Property Value is set</param>
    /// <param name="propName">Property name as string.</param>
    /// <param name="val">Value to set.</param>
    /// <returns>PropertyValue</returns>
    public static void SetPrivatePropertyValue(this object obj, string propName, object val)
    {
        Type t = obj.GetType();
        if (t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) == null)
            throw new ArgumentOutOfRangeException(nameof(propName), string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));
        t.InvokeMember(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, obj, new object[] { val });
    }

    /// <summary>
    /// Set a private Property Value on a given Object. Uses Reflection.
    /// </summary>
    /// <param name="obj">Object from where the Property Value is returned</param>
    /// <param name="propName">Property name as string.</param>
    /// <param name="val">the value to set</param>
    /// <exception cref="ArgumentOutOfRangeException">if the Property is not found</exception>
    public static void SetPrivateFieldValue(this object obj, string propName, object val)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        Type? t = obj.GetType();
        FieldInfo? fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t = t.BaseType;
        }
        if (fi == null) throw new ArgumentOutOfRangeException(nameof(propName), string.Format("Field {0} was not found in Type {1}", propName, obj.GetType().FullName));
        fi.SetValue(obj, val);
    }

	/// <summary>
	/// Converts a method into a delegate for the specified instance
	/// </summary>
	/// <param name="method">The method to convert</param>
	/// <param name="instance">The object the method is attached to</param>
	/// <returns>The delegate created from the method</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if there are more than 16 parameters for the method</exception>
	public static Delegate ToDelegate(this MethodInfo method, object instance)
	{
		static Type MakeFuncParameter(int parameters)
		{
			return parameters switch
			{
				00 => typeof(Func<>),
				01 => typeof(Func<,>),
				02 => typeof(Func<,,>),
				03 => typeof(Func<,,,>),
				04 => typeof(Func<,,,,>),
				05 => typeof(Func<,,,,,>),
				06 => typeof(Func<,,,,,,>),
				07 => typeof(Func<,,,,,,,>),
				08 => typeof(Func<,,,,,,,,>),
				09 => typeof(Func<,,,,,,,,,>),
				10 => typeof(Func<,,,,,,,,,,>),
				11 => typeof(Func<,,,,,,,,,,,>),
				12 => typeof(Func<,,,,,,,,,,,,>),
				13 => typeof(Func<,,,,,,,,,,,,,>),
				14 => typeof(Func<,,,,,,,,,,,,,,>),
				15 => typeof(Func<,,,,,,,,,,,,,,,>),
				16 => typeof(Func<,,,,,,,,,,,,,,,,>),
				_ => throw new ArgumentOutOfRangeException(nameof(parameters), "Too many parameters for Func")
			};
		}

		static Type MakeActionParameter(int parameters)
		{
			return parameters switch
			{
				00 => typeof(Action),
				01 => typeof(Action<>),
				02 => typeof(Action<,>),
				03 => typeof(Action<,,>),
				04 => typeof(Action<,,,>),
				05 => typeof(Action<,,,,>),
				06 => typeof(Action<,,,,,>),
				07 => typeof(Action<,,,,,,>),
				08 => typeof(Action<,,,,,,,>),
				09 => typeof(Action<,,,,,,,,>),
				10 => typeof(Action<,,,,,,,,,>),
				11 => typeof(Action<,,,,,,,,,,>),
				12 => typeof(Action<,,,,,,,,,,,>),
				13 => typeof(Action<,,,,,,,,,,,,>),
				14 => typeof(Action<,,,,,,,,,,,,,>),
				15 => typeof(Action<,,,,,,,,,,,,,,>),
				16 => typeof(Action<,,,,,,,,,,,,,,,>),
				_ => throw new ArgumentOutOfRangeException(nameof(parameters), "Too many parameters for Action")
			};
		}

		var parameters = method.GetParameters();
		var returnType = method.ReturnType;
		Type func;
		if (returnType == typeof(void))
		{
			var types = parameters
				.Select(t => t.ParameterType)
				.ToArray();
			func = MakeActionParameter(types.Length);
			if (types.Length > 0)
				func = func.MakeGenericType(types);
		}
		else
		{
			var types = parameters
				.Select(t => t.ParameterType)
				.Append(returnType)
				.ToArray();
			func = MakeFuncParameter(types.Length - 1)
				.MakeGenericType(types);
		}
		return Delegate.CreateDelegate(func, instance, method);
	}

	/// <summary>
	/// Determines if the type is a collection
	/// </summary>
	/// <param name="type">The type to check</param>
	/// <param name="valueType">The type of the data within the collection</param>
	/// <returns>Whether or not the type is a collection</returns>
	public static bool IsCollection(this Type type, [MaybeNullWhen(false)] out Type valueType)
	{
		var array = type.IsArray;
		if (type.IsArray)
		{
			valueType = type.GetElementType()!;
			return true;
		}

		var interType = type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
		if (interType is null)
		{
			valueType = null;
			return false;
		}

		valueType = interType.GetGenericArguments()[0];
		return true;
	}

	/// <summary>
	/// Determines if the type is a dictionary
	/// </summary>
	/// <param name="type">The type to check</param>
	/// <param name="keyType">The type of the key</param>
	/// <param name="valueType">The type of the value</param>
	/// <returns>Whether or not the type is a dictionary</returns>
	public static bool IsDictionary(this Type type, [MaybeNullWhen(false)] out Type keyType, [MaybeNullWhen(false)] out Type valueType)
	{
		var dict = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		if (!dict)
		{
			keyType = null;
			valueType = null;
			return false;
		}

		var args = type.GetGenericArguments();
		keyType = args[0];
		valueType = args[1];
		return true;
	}

	/// <summary>
	/// Determines if the type is a Task or ValueTask
	/// </summary>
	/// <param name="type">The type to check</param>
	/// <param name="valueType">The generic type of the task (if null - it's a Task or ValueTask without a generic)</param>
	/// <returns>Whether or not the type is a task</returns>
	public static bool IsTask(this Type type, out Type? valueType)
	{
		if (type == typeof(Task) || type == typeof(ValueTask))
		{
			valueType = null;
			return true;
		}

		Type[] types = [typeof(Task<>), typeof(ValueTask<>)];
		if (!type.IsGenericType)
		{
			valueType = null;
			return false;
		}

		var generic = type.GetGenericTypeDefinition();
		if (!types.Any(t => t == generic))
		{
			valueType = null;
			return false;
		}
		valueType = type.GetGenericArguments()[0];
		return true;
	}

	/// <summary>
	/// Gets the property information from the given lambda expression.
	/// </summary>
	/// <typeparam name="TSource">The source class</typeparam>
	/// <typeparam name="TProp">The property on the source class</typeparam>
	/// <param name="propertyLambda">The lambda expression to retrieve the property from the source class</param>
	/// <returns>The property information for the given expression</returns>
	/// <exception cref="ArgumentNullException">Thrown in the case the lambda expression doesn't reference a property</exception>
	/// <exception cref="ArgumentException">Thrown in the case the lambda expression doesn't reference a property</exception>
	public static PropertyInfo GetPropertyInfo<TSource, TProp>(this Expression<Func<TSource, TProp>>? propertyLambda)
    {
        if (propertyLambda == null) throw new ArgumentNullException(nameof(propertyLambda));

        var type = typeof(TSource);

        if (propertyLambda.Body is not MemberExpression member)
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a method, not a property.",
                propertyLambda.ToString()));

        var propInfo = member.Member as PropertyInfo 
            ?? throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a field, not a property.",
                propertyLambda.ToString()));

        if (propInfo.ReflectedType != null &&
            type != propInfo.ReflectedType &&
            !type.IsSubclassOf(propInfo.ReflectedType))
            throw new ArgumentException(string.Format(
                "Expression '{0}' refers to a property that is not from type {1}.",
                propertyLambda.ToString(),
                type));

        return propInfo;
    }
}
