HttpServer
==========

Middleware based httpserver inspired by Sinatra, Expressjs and the likes.


## Installation

```bash
git clone https://github.com/kildevaeld/HttpServer.git
cd HttpServer &&  git submodule update --init

```

## Usage

```csharp

var server = new HttpServer.HttpServer();

server.Listen(80);

```

## Middleware
Middleware is run in the order, which they are "Used". If a middleware ends the the request (eg. Sends a response to the client), the remaining middleware in the stack will not be run.

```csharp

var docRoot = ....

server.Use(Favicon(docRoot)); // Support for favicon
server.Use(Static(docRoot)); // Support for static files.
server.Use(Html(docRoot)); // Support for html index files.

server.Use((HTTPRequest request, HTTPResponse response) => {
  request.Headers["Content-Type"] = "text/html; charset=utf-8";
});

```


## Routing
Support for basic routing, using HTTP verbs. Routes are just middleware

```csharp

server.Get("/some-route", (HTTPRequest request, HTTPResponse response) => {
  response.Send("Hello, World"); 
});

server.Get("/another-route", (HTTPRequest request, HTTPResponse response) => {
  response.SendFile(someFilePath);
});

// Or you can use multiple middleware pr route eg:

server.Get("/some-private-route", SomeMiddleWareAuthenticationThingy, (HTTPRequest request, HTTPResponse response) => {
  response.SendFile(someFilePath);
});


server.Get("/api/:id", (HTTPRequest request, HTTPResponse response) => {
  response.SendFormat("Got id : {0}", request.Param("id"));
});

server.Post(....);

server.Delete(...);

server.Put(...);


```

### Matching

```csharp

class SomeClass {

  [Route("/some-route")] // Defaults to a get request
  public void Index(HTTPRequest request, HTTPResponse response) {}
  
  [Route("/some-other/route", Method = Methods.Get)]
  public void Show(HTTPRequest request, HTTPResponse response) {}
  
}

server.Match<SomeClass>();

```


## Custom error handling.

```csharp

server.Use(HTTPRequest request, HTTPResponse response, HTTPException exception) => {
  request.Send(exception.StatusCode, exception.Message);
});

```

## The HTTPRequest object

### Query parameters

```csharp

// /route?some=query

var query = request.Query();

if (query != null)
  Console.WriteLine("Got some : {0}", query["some"]);

// OR
var some = request.GetQuery("some");
if (some != null) {
  Console.WriteLine("Got some : {0}", some); 
}



```

### application/json / x-form-urlencoded


```csharp

var json = request.getJSON();

// OR

var json = request.getJSON<SomeClass>();

// OR

var urlencoded = request.GetFormUrlEncoded();

// Or any of the above

var body = request.GetBody();

```
