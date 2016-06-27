using EasyIoC.Core.Exceptions;
using EasyIoC.Core.ServiceContainer;
using EasyIoC.WebApi.Tests.TestResources;
using System;
using System.Reflection;
using Xunit;

namespace EasyIoC.WebApi.Tests
{
    public class EasyWebApiControllerContainerTests
    {
        public class RegisterControllers
        {
            [Fact]
            public void RegisterControllers_CalledWithAssembly_RegistersControllers()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // act
                container.RegisterControllers(assembly);

                // assert
                Assert.True(container.IsRegistered<FooController>());
                Assert.True(container.IsRegistered<BarController>());
                Assert.True(container.IsRegistered<FooBarController>());
                Assert.True(container.IsRegistered<TestController>());
            }
        }
        

        public class Register
        {
            [Fact]
            public void Register_NullArgument_ThrowsNullArgumentException()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.Register(null);
                });
            }

            [Fact]
            public void GenericRegister_GoodArg_EntryIsRegistered()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // act
                container.Register<FooController>();

                // assert
                Assert.True(container.IsRegistered<FooController>());
                Assert.True(container.IsRegistered(typeof(FooController)));
            }


            [Fact]
            public void Register_GoodArg_EntryIsRegistered()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // act
                container.Register(typeof(FooController));

                // assert
                Assert.True(container.IsRegistered<FooController>());
                Assert.True(container.IsRegistered(typeof(FooController)));
            }


            [Fact]
            public void Register_NotIController_ThrowsTypeMismatchException()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // assert
                Assert.Throws<TypeMismatchException>(() =>
                {
                    // act
                    container.Register(typeof(Foo));
                });
            }
        }
        

        public class IsRegistered
        {
            [Fact]
            public void GenericIsRegistered_NothingRegistered_ReturnsFalse()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // act & assert
                Assert.False(container.IsRegistered<FooController>());
            }


            [Fact]
            public void IsRegistered_NothingRegistered_ReturnsFalse()
            {
                // arrange
                var container = new EasyServiceContainer(Assembly.GetExecutingAssembly());

                // act & assert
                Assert.False(container.IsRegistered(typeof(FooController)));
            }
        }


        public class Activate
        {
            [Fact]
            public void Activate_NullArgument_ThrowsNullArgumentException()
            {
                // arrange
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));

                // assert
                Assert.Throws<ArgumentNullException>(() =>
                {
                    // act
                    container.Activate(null);
                });
            }


            [Fact]
            public void Activate_ControllerWithNoParameters_Returns()
            {
                // arrange 
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));
                container.Register<TestController>();

                // act
                var controller = container.Activate<TestController>();

                // assert
                Assert.IsType<TestController>(controller);
            }


            [Fact]
            public void Activate_ControllerWithOneParameter_Returns()
            {
                // arrange 
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));
                container.Register<FooController>();

                // act
                var controller = container.Activate<FooController>();

                // assert
                Assert.IsType<FooController>(controller);
            }


            [Fact]
            public void Activate_ControllerWithMultipleParameters_Returns()
            {
                // arrange 
                var assembly = Assembly.GetExecutingAssembly();
                var container = new EasyWebApiControllerContainer(new EasyServiceContainer(assembly));
                container.Register<FooBarController>();

                // act
                var controller = container.Activate<FooBarController>();

                // assert
                Assert.IsType<FooBarController>(controller);
            }
        }
    }
}
