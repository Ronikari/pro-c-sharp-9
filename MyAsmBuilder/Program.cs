using System;
using System.Reflection;
using System.Reflection.Emit;
namespace MyAsmBuilder
{
    internal class Program
    {
        static AssemblyBuilder CreateMyAsm()
        {
            // Установить общие характеристики сборки
            AssemblyName assemblyName = new AssemblyName
            {
                Name = "MyAssembly",
                Version = new Version("1.0.0.0")
            };

            // Создать новую сборку
            var builder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            // Определить имя модуля
            ModuleBuilder module = builder.DefineDynamicModule("MyAssembly");

            // Определить открытый класс по имени HelloWorld
            TypeBuilder helloWorldClass = module.DefineType("MyAssembly.HelloWorld", TypeAttributes.Public);

            // Определить закрытую переменную-член типа String по имени theMessage
            FieldBuilder msgField = helloWorldClass.DefineField
                ("theMessage",
                Type.GetType("System.String"),
                attributes: FieldAttributes.Private);

            // Создать специальный конструктор
            Type[] constructorArgs = new Type[1];
            constructorArgs[0] = typeof(string);
            ConstructorBuilder constructor = helloWorldClass.DefineConstructor
                (MethodAttributes.Public,
                CallingConventions.Standard,
                constructorArgs);
            ILGenerator constructorIl = constructor.GetILGenerator();
            constructorIl.Emit(OpCodes.Ldarg_0);

            Type objectClass = typeof(object);
            ConstructorInfo? superConstructor = objectClass.GetConstructor(new Type[0]);

            constructorIl.Emit(OpCodes.Call, superConstructor);
            constructorIl.Emit(OpCodes.Ldarg_0);
            constructorIl.Emit(OpCodes.Ldarg_1);
            constructorIl.Emit(OpCodes.Stfld, msgField);
            constructorIl.Emit(OpCodes.Ret);

            // Создать стандартный конструктор
            helloWorldClass.DefineDefaultConstructor(MethodAttributes.Public);

            // Создать метод GetMsg()
            MethodBuilder getMsgMethod = helloWorldClass.DefineMethod
                ("GetMsg",
                MethodAttributes.Public,
                typeof(string),
                null);
            ILGenerator methodIl = getMsgMethod.GetILGenerator();
            methodIl.Emit(OpCodes.Ldarg_0);
            methodIl.Emit(OpCodes.Ldfld, msgField);
            methodIl.Emit(OpCodes.Ret);

            // Создать метод SayHello()
            MethodBuilder sayHiMethod = helloWorldClass.DefineMethod("SayHello", MethodAttributes.Public, null, null);
            methodIl = sayHiMethod.GetILGenerator();
            methodIl.EmitWriteLine("Hello from the HelloWorld class!");
            methodIl.Emit(OpCodes.Ret);

            // Выпустить класс HelloWorld
            helloWorldClass.CreateType();
            return builder;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("***** The Amazing Dynamic Assembly Builder App *****");
            // Создать объект AssemblyBuilder с использованием вспомогательной функции
            AssemblyBuilder builder = CreateMyAsm();

            // Получить тип HelloWorld
            Type? hello = builder.GetType("MyAssembly.HelloWorld");

            // Создать экземпляр HelloWorld и вызвать конкретный конструктор
            Console.Write("-> Enter Message to pass HelloWorld class: ");
            string? msg = Console.ReadLine();
            object[] ctorArgs = new object[1];
            ctorArgs[0] = msg;
            object? obj = Activator.CreateInstance(hello, ctorArgs);

            // Вызвать метод SayHello() и отобразить возвращенную строку
            Console.WriteLine("-> Calling SayHello() via late binding.");
            MethodInfo? mi = hello.GetMethod("SayHello");
            mi?.Invoke(obj, null);

            // Вызвать метод GetMsg()
            mi = hello.GetMethod("GetMsg");
            Console.WriteLine(mi?.Invoke(obj, null));
        }
    }
}