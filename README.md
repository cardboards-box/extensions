# Cardboard Box Extensions 
A collection of helpful extensions for general C# development.

The library is available as a [NuGet package](https://www.nuget.org/packages/CardboardBox.Extensions/).

The library has various static classes with extensions for common methods. The classes are:
* `DateTimeExtensions` - Extensions for DateTime objects.
* `HashExtensions` - Extensions for hashing byte arrays and strings.
* `HTMLExtensions` - Extensions for working with HTMLAgilityPack
* `LingExtensions` - Extensions for Enumerable and collections
* `MethodExtensions` - Extensions for actions or functions
* `ReflectionExtensions` - Extensions for reflection, type checking, and Enums
* `StreamExtensions` - Extensions for working with streams
* `StringExtensions` - Extensions for working with strings, including Base64 encoding and decoding
* `TaskExtensions` - Extensions for working with Tasks and threading
* `XmlExtensions` - Extensions for working with XML strings and serialization

The library also has some helpful utility classes as well:
* `QueueCacheItem` - A class for working with lazy-loaded cache items and queuing the results.
* Response Boxing - A set of classes for wrapping responses in a common format
	* `Boxed` - The base class a response 
	* `Boxed<T>` - The base class for a response with a payload of a generic
	* `BoxedArray<T>` - The base class for a response with a payload of an array of generics
	* `BoxedError` - A response with an error message
	* `BoxedPage<T>` - A response with an array of generics and pagination information
* `RequestValidator` - A fluent helper class for validating requests 
* `X` - A helper class for creating XML elements and attributes in a compiled-react like manner.
* `CollectionIterator` - A helper class for iterating over collections
