﻿using System;
using CommonSnappableTypes;
namespace CSharpSnapIn
{
    [CompanyInfo(CompanyName = "AloGeno Studio", CompanyUrl = "www.AloGeno.com")]
    public class CSharpModule : IAppFunctionality
    {
        void IAppFunctionality.DoIt()
        {
            Console.WriteLine("You have just used the C# snap-in!");
        }
    }
}