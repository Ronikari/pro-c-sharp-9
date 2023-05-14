.assembly extern System.Runtime
{
  .publickeytoken = ( B0 3F 5F 7F 11 D5 0A 3A )              
  .ver 7:0:0:0
}

// Наша сборка
.assembly CILTypes
{
	.ver 1:0:0:0
}

// Модуль нашей однофайловой сборки
.module CILTypes.dll

// Наша сборка имеет единственное пространство имен
.namespace MyNamespace
{
	// Определение интерфейса
	.class public interface IMyInterface {}
	
	// Расширение интерфейсов в CIL
	.class public interface IMyOtherInterface implements MyNamespace.IMyInterface {}
	
	// Предполагается базовый класс System.Object
	.class public MyBaseClass
	{
		// Поля класса
		.field private string stringField
		.field private int32 intField
		
		// Свойство класса
		.method public hidebysig specialname instance string get_TheString() cil managed
		{
			// Добавить код реализации...
		}
		.method public hidebysig specialname instance void set_TheString(string 'value') cil managed
		{
			// Добавить код реализации...
		}
		.property instance string TheString()
		{
			.get instance string MyNamespace.MyBaseClass::get_TheString()
			.set instance void MyNamespace.MyBaseClass::set_TheString(string)
		}
		
		// Конструктор класса
		.method public hidebysig specialname rtspecialname instance void .ctor(string s, int32 i) cil managed
		{
			// Добавить код реализации...
		}
		
		// Метод класса, принимающий входные параметры
		.method public hidebysig static void MyMethod(int32 inputInt, int32& refInt, class [System.Runtime.Extensions]System.Collections.ArrayList ar, [out] int32& outputInt) cil managed {}
		.method public hidebysig instance void Speak() cil managed
		{
			// Внутри области действия этого метода в стеке находится в точности одно значение (строковый литерал)
			.maxstack 1
			ldstr "Hello there..."
			call void [System.Runtime]System.Console::WriteLine(string)
			ret
		}
		.method public hidebysig static void MyLocalVariables() cil managed
		{
			.maxstack 8
			// Определить три локальные переменные
			.locals init (string myStr, int32 myInt, object myObj)
			
			// Загрузить строку в виртуальный стек выполнения
			ldstr "CIL code is fun!"
			
			// Извлечь текущее значение и сохранить его в локальной переменной [0]
			stloc.0
			
			// Загрузить константу типа i4 (сокращение для int32) со значением 33
			ldc.i4.s 33
			
			// Извлечь текущее значение и сохранить его в локальной переменной [1]
			stloc.1
			
			// Создать новый объект и поместить его в стек
			newobj instance void [System.Runtime]System.Object::.ctor()
			
			// Извлечь текущее значение и сохранить его в локальной переменной [2]
			stloc.2
			ret
		}
		.method public hidebysig static int32 Add(int32 a, int32 b) cil managed
		{
			.maxstack 2
			ldarg.0 	// загрузить а в стек
			ldarg.1		// загрузить b в стек
			add			// сложить оба значения
			ret
		}
	}
	
	// MyDerivedClass реализует IMyInterface и расширяет MyBaseClass
	.class public MyDerivedClass extends MyNamespace.MyBaseClass implements MyNamespace.IMyInterface {}
	
	// Определение структуры
	.class public sealed MyStruct extends [System.Runtime]System.ValueType{}
	// .class public sealed value MyStruct{}
	
	// Определение перечисления
	.class public sealed MyEnum extends [System.Runtime]System.Enum
	// .class public sealed enum MyEnum
	{
		.field public static literal valuetype MyNamespace.MyEnum A = int32(0)
		.field public static literal valuetype MyNamespace.MyEnum B = int32(1)
		.field public static literal valuetype MyNamespace.MyEnum C = int32(2)
	}
	
	// Определение обобщений
	//newobj instance void class [System.Collections]System.Collections.Generic.List`1<int32>::.ctor()
	//newobj instance void class [System.Collections]System.Collections.Generic.Dictionary`2<string,int32>::.ctor()
	//newobj instance void class [System.Runtime]System.Collections.Generic.List`1<class [System.Collections]System.Collections.Generic.List`1<int32>>::.ctor()
	
	.method static public void Main() cil managed
	{
		.entrypoint
	}
}