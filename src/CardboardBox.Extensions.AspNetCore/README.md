# Cardboard Box Extensions - ASP.Net Core
A collection of extensions for ASP.net core web-api projects.

The library is available as a [NuGet package](https://www.nuget.org/packages/CardboardBox.Extensions.AspNetCore/).

The library is built on top of the [CardboardBox.Extensions](https://www.nuget.org/packages/CardboardBox.Extensions/) and provides additional functionality for ASP.Net Core projects.
* `BaseController` - A base controller class that provides response boxing using the `Boxed` classes from the `CardboardBox.Extensions` library.
	* `ProducesBoxAttribute` - Short hand for `ProducesResponseTypeAttribute(typeof(Boxed), 200)`
	* `ProducesBoxAttribute<T>` - Short hand for `ProducesResponseTypeAttribute(typeof(Boxed<T>), 200)`
	* `ProducesArrayAttribute<T>` - Short hand for `ProducesResponseTypeAttribute(typeof(BoxedArray<T>), 200)`
	* `ProducesPagedAttribute<T>` - Short hand for `ProducesResponseTypeAttribute(typeof(BoxedPaged<T>), 200)`
	* `ProducesErrorAttribute` - Short hand for `ProducesResponseTypeAttribute(typeof(BoxedError), 500)`
* JSON Web Tokens - The library provides a set of classes to help with JWT token generation and validation.
	* `JwtToken` - A class that represents a JWT token that can be used to generate and parse tokens.
	* `JwtExtensions` - A helper class of extensions for working with JWT tokens, claims, and other related functionality.
	* `JwtTokenResult` - A class that represents the result of the token parsing process.
