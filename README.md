# EasyIoC
Dependency injection made easy for MVC 5 and Web Api 2. EasyIoC is designed to be quick and easy to setup.

[![Build status](https://ci.appveyor.com/api/projects/status/nbcvn4d6la5rl6cd/branch/master?svg=true)](https://ci.appveyor.com/project/LeeCheneler/easyioc/branch/master)

### EasyIoC Core
##### https://www.nuget.org/packages/EasyIoC.Core/

EasyIoC core contains the core components that power EasyIoC's dependency injection for Mvc 5 and WebApi 2. The main component users of this library will utilize is the interface `IEasyServiceRegistrar`. This is used to implement Easy Service Registrars. `EasyServiceContainers` will search a given assembly (usually your web apps assembly if you use the default initialisers explained below) for all classes that implement `IEasyServiceRegistrar` and call its `RegisterServices(IEasyServiceContainer container)` method to register the services in the passed in container.

``` c#
public class MyEasyServiceRegistrar : IEasyServiceRegistrar
{
    public void RegisterServices(IEasyServiceContainer container)
    {
        container.Register<IFoo, Foo>();
        container.Register(typeof(IBar), typeof(Bar));
        container.RegisterSingleton<ISingletonFoo, SingletonFoo>();
        container.RegisterSingleton(typeof(ISingletonBar), typeof(SingletonBar));
        
        // Being able to register a delegate to create a service removes the need for single line factory classes
        // that only exist to allow injection of a dependency that requires constructor args...
        // Less code, less files, less clutter, less surface space for bugs
        container.Register<IFooBar>(() => 
            { 
                return new FooBar(new Foo(), new Bar()); 
            });
    }
}
```

EasyIoC supports injecting as many constructor arguments into your controllers arguments as you'd like. It is not a limit to a single argument.


### EasyIoC Mvc Guide (Mvc 5)
##### https://www.nuget.org/packages/EasyIoC.Mvc/

The quickest and simplest way to setup EasyIoC for use in your Mvc 5 application is to use its initialiser to do the work for you. This will obtain the calling assembly and find and register all Mvc 5 Controllers within it aswell as find all classes that implement IEasyServiceRegistrar and register all your services so they can be injected into any controllers that require them.

``` c#
public class Global : HttpApplication
{
    public void Application_Start()
    {
        var initialiser = new EasyMvcIoCInitialiser();
        initialiser.Initialise(ControllerBuilder);
    }
}

public class MyController : Controller
{
    private readonly IFoo _foo;
    public MyController(IFoo foo)
    {
        _foo = foo; // foo of the contrete type Foo
    }
}
```


### EasyIoC WebApi Guide (WebApi 2)
##### https://www.nuget.org/packages/EasyIoC.WebApi/

The quickest and simplest way to setup EasyIoC for use in your WebApi 2 application is to use its initialiser to do the work for you. This will obtain the calling assembly and find and register all WebApi 2 ApiControllers within it aswell as find all classes that implement IEasyServiceRegistrar and register all your services so they can be injected into any controllers that require them.

``` c#
public class Global : HttpApplication
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
            
            var initialiser = new EasyWebApiIoCInitialiser();
            initialiser.Initialise(config);
        }
    }
}

public class MyApiController : ApiController
{
    private readonly IBar _bar;
    public MyController(IBar bar)
    {
        _bar = bar; // bar of the contrete type Bar
    }
}
```
