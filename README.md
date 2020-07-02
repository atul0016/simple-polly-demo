# Fault Tolerant in ASP.NET Core using Polly
This is a simple proof of concept application developed to showcase how we can implement fault resiliency in .NET Core applications. To demonstrate this, we have implemented a simple application in an ASP.NET Core project using [Polly](https://github.com/App-vNext/Polly). 


## Prerequisites
- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/community/)
- [.NET Core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [Polly](https://github.com/App-vNext/Polly) 
- [Postman](https://www.postman.com/)

## Solution Layout
In Visual Studio, we have scaffolded the solution containing two layers:
- **Infrastructure**: It contains the Polly registry items, and implementation.
- **PollyApi**: A simple Web API project that references the infrastructure layer to access methods from Polly registry class.

![sln](https://user-images.githubusercontent.com/23207774/86381821-9e9a7a00-bc9e-11ea-8cfc-e41ec2cd4ce5.jpg)

## Resiliency Approaches Implemented
- **Retry Pattern**: If something goes wrong in the first situation, attempt repeating the same operation again x number of times to overcome the transient fault before giving up.
- **Circuit-Breaker Pattern**: If something goes wrong, hit the «may-day» button that prevents any further attempts to repeat the operation. This approach is used where we have an unreliable dependency and calling such dependency will take significant time and might worsen the situation.

A standard repository pattern has been followed for simplicity to simulate a fault dependency. The `MessageRepository` class throws an exception randomly - say 50% of the time. 

```
private void ThrowRandomException()
{
       var random = new Random().Next(0, 10);

       if (random > 5)
       {
            Console.WriteLine("Error! Throwing Exception");
             throw new Exception("Exception in MessageRepository");
       }
}

```

### Polly Retry Registry
A retry policy is created by the following signature:

```
Policy
  .Handle<Exception>()
  .WaitAndRetryAsync(int retryCount, Func<int, Timespan> sleepDurationProvider)
```
- `Handle<Exception>`: It states the type of exceptions the policy can handle, where it is application for all `Exception` types. 
- `WaitAndRetry(int retryCount, Func<int, Timespan>`: The `retryCount` is the number of times the policy should try again. `Func<int, Timespan>` is a delegate which determines the waiting period before trying again. 

### Circuit-Break Registry
A circuit-breaker policy is generally created using the following signature:

```
Policy
  .Handle<Exception>()
  .CircuitBreakerAsync(
    int exceptionsAllowedBeforeBreaking,
    TimeSpan durationOfBreak,
    Action<Exception, TimeSpan> onBreak,
    Action onReset);
```
- `Handle<Exception>`: Same concept as Retry policies. 
- `CircuitBreakerAsync(..)`: `int exceptionsAllowedBeforeBreaking` specifies the number of exceptions allowed before triggering a circuit break. `TimeSpan durationOfBreak` specifies how long the circuit will remain broken. `Action<Exception, TimeSpan> onBreak` a delegate which allows you to perform some action (typically this is used for logging) when the circuit is broken. `Action onReset` a delegate which allows you to perform some action (again, typically for logging) when the circuit is reset.

## References 
- [Polly](https://github.com/App-vNext/Polly)
- [Building Resilient Applications with Circut Breaker Pattern](https://medium.com/@maiconcpereira/building-resilient-applications-with-circuit-breaker-pattern-184b98f4482c)
- [Polly 5.0 - a wider resilience framework](http://www.thepollyproject.org/2016/10/25/polly-5-0-a-wider-resilience-framework/)
