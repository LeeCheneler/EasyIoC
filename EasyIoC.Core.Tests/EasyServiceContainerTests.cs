using EasyIoC.Core.Exceptions;
using EasyIoC.Core.ServiceContainer;
using EasyIoC.Tests.TestResources;
using System;
using System.Reflection;
using Xunit;

namespace EasyIoC.Tests
{
    public class EasyServiceContainerTests
    {
        public class Ctor
        {
            [Fact]
            public void Ctor_NullArgument_ThrowsArgumentNullException()
            {
                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // arrange & act
                    new EasyServiceContainer(null);
                });
            }


            [Fact]
            public void Ctor_ServiceRegistrarsExist_ServiceRegistrarsRegistered()
            {
                // arrange & act
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.True(container.IsRegistered<IBar>());
            }
        }

        public class Register
        {
            [Fact]
            public void GenericRegister_GoodArg_EntryIsRegistered()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act
                container.Register<IFoo, Foo>();

                // assert
                Assert.True(container.IsRegistered<IFoo>());
                Assert.True(container.IsRegistered(typeof(IFoo)));
            }


            [Fact]
            public void Register_FirstNullArgument_ThrowsArgumentNullException()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.Register(null, typeof(object));
                });
            }


            [Fact]
            public void Register_SecondNullArgument_ThrowsArgumentNullException()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.Register(typeof(object), null);
                });
            }


            [Fact]
            public void Register_GoodArg_EntryIsRegistered()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act
                container.Register(typeof(IFoo), typeof(Foo));

                // assert
                Assert.True(container.IsRegistered<IFoo>());
                Assert.True(container.IsRegistered(typeof(IFoo)));
            }


            [Fact]
            public void Register_TypeMismatch_ThrowsTypeMismatchException()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<TypeMismatchException>(() =>
                {
                    // act
                    container.Register<IFoo, Bar>();
                });
            }


            [Fact]
            public void Register_AlreadyRegistered_ThrowsAlreadyRegisteredException()
            {
                // arrange 
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.Register(typeof(IFoo), typeof(Foo));

                // assert
                Assert.Throws<AlreadyRegisteredException>(() =>
                {
                    // act
                    container.Register(typeof(IFoo), typeof(Foo));
                });
            }


            [Fact]
            public void GenericRegister_AlreadyRegistered_ThrowsAlreadyRegisteredException()
            {
                // arrange 
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.Register<IFoo, Foo>();

                // assert
                Assert.Throws<AlreadyRegisteredException>(() =>
                {
                    // act
                    container.Register<IFoo, Foo>();
                });
            }


            [Fact]
            public void Register_FuncIsNull_ThrowsArgumentNullException()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.Register<IFoo>(null);
                });
            }


            [Fact]
            public void Register_FuncPassedIn_ReturnsCorrectObject()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act
                container.Register<IFoo>(() =>
                {
                    return new Foo();
                });

                // assert
                var result = container.Activate<IFoo>();
                Assert.IsType<Foo>(result);
            }
        }


        public class IsRegistered
        {
            [Fact]
            public void GenericIsRegistered_NothingRegistered_ReturnsFalse()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act & assert
                Assert.False(container.IsRegistered<IFoo>());
            }


            [Fact]
            public void IsRegistered_NothingRegistered_ReturnsFalse()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act & assert
                Assert.False(container.IsRegistered(typeof(IFoo)));
            }
        }


        public class Activate
        {
            [Fact]
            public void GenericActivate_IsRegistered_ReturnsImpl()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.Register<IFoo, Foo>();

                // act
                var impl = container.Activate<IFoo>();

                // assert
                Assert.IsType<Foo>(impl);
            }


            [Fact]
            public void Activate_NullArgument_ThrowsArgumentNullException()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.Activate(null);
                });
            }


            [Fact]
            public void Activate_IsRegistered_ReturnsImpl()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.Register<IFoo, Foo>();

                // act
                var impl = container.Activate(typeof(IFoo));

                // assert
                Assert.IsType<Foo>(impl);
            }


            [Fact]
            public void Activate_TypeNotRegistered_ThrowsNotRegisteredException()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<NotRegisteredException>(() =>
                {
                    // act
                    container.Activate<IFoo>();
                });
            }


            [Fact]
            public void Activate_GettingSingletonService_ReturnsSameService()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.RegisterSingleton<IFoo, Foo>();

                // act
                var foo1 = container.Activate<IFoo>();
                var foo2 = container.Activate<IFoo>();

                // assert
                Assert.Same(foo1, foo2);
            }
        }


        public class RegisterSingleton
        {
            [Fact]
            public void GenericRegisterSingleton_AlreadyRegistered_ThrowsAlreadyRegiesteredException()
            {
                // arrange 
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.Register<IFoo, Foo>();

                // assert
                Assert.Throws<AlreadyRegisteredException>(() =>
                {
                    // act
                    container.RegisterSingleton<IFoo, Foo>();
                });
            }


            [Fact]
            public void RegisterSingleton_AlreadyRegistered_ThrowsAlreadyRegiesteredException()
            {
                // arrange 
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());
                container.Register(typeof(IFoo), typeof(Foo));

                // assert
                Assert.Throws<AlreadyRegisteredException>(() =>
                {
                    // act
                    container.RegisterSingleton(typeof(IFoo), typeof(Foo));
                });
            }


            [Fact]
            public void RegisterSingleton_NullAbstraction_ThrowsArgumentNullException()
            {
                // arrange 
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.RegisterSingleton(null, typeof(Foo));
                });
            }


            [Fact]
            public void RegisterSingleton_NullConcrete_ThrowsArgumentNullException()
            {
                // arrange 
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.RegisterSingleton(typeof(IFoo), null);
                });
            }


            [Fact]
            public void RegisterSingleton_GoodArgs_IsRegisteredTrue()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act
                container.RegisterSingleton(typeof(IFoo), typeof(Foo));

                // assert
                Assert.True(container.IsRegistered<IFoo>());
            }


            [Fact]
            public void GenericRegisterSingleton_GoodArgs_IsRegisteredTrue()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act
                container.RegisterSingleton<IFoo, Foo>();

                // assert
                Assert.True(container.IsRegistered<IFoo>());
            }
        }
    }
}
