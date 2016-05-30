# JSON Web Token (JWT) Implementation for .NET

### Note that I've made some changes to this fork and have not updated all of the documents yet. It's a work in progress.

This library supports generating and decoding [JSON Web Tokens](http://tools.ietf.org/html/draft-jones-json-web-token-10).

Originally forked from https://github.com/jwt-dotnet/jwt

### Installation
### Usage
### Creating Tokens

```csharp
JsonWebToken jwt = new JsonWebToken(SerializerType.DefaultSerializer);
var payload = new Dictionary<string, object>()
{
    { "claim1", 0 },
    { "claim2", "claim2-value" }
};
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
string token = jwt.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
Console.WriteLine(token);
```

Output will be:
    eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s

### Verifying and Decoding Tokens

```csharp
JsonWebToken jwt = new JsonWebToken(SerializerType.DefaultSerializer);
var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjbGFpbTEiOjAsImNsYWltMiI6ImNsYWltMi12YWx1ZSJ9.8pwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-5kM5s";
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
try
{
    string jsonPayload = jwt.JsonWebToken.Decode(token, secretKey);
    Console.WriteLine(jsonPayload);
}
catch (JWT.SignatureVerificationException)
{
    Console.WriteLine("Invalid token!");
}
```

Output will be:

    {"claim1":0,"claim2":"claim2-value"}

You can also deserialize the JSON payload directly to a .Net object with DecodeToObject:

```csharp
var payload = JWT.JsonWebToken.DecodeToObject(token, secretKey) as IDictionary<string, object>;
Console.WriteLine(payload["claim2"]);
```

which will output:
    
    claim2-value

#### exp claim

As described in the [JWT RFC](https://tools.ietf.org/html/draft-ietf-oauth-json-web-token-32#section-4.1.4) the `exp` "claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing." If an `exp` claim is present and is prior to the current time the token will fail verification. The exp (expiry) value must be specified as the number of seconds since 1/1/1970 UTC.

```csharp
JsonWebToken jwt = new JsonWebToken(SerializerType.DefaultSerializer);
var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
var now = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
var payload = new Dictionary<string, object>()
{
    { "exp", now }
};
var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
string token = jwt.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);

string jsonPayload = jwt.JsonWebToken.Decode(token, secretKey); // JWT.SignatureVerificationException!
```
