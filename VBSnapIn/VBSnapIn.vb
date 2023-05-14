Imports CommonSnappableTypes
<CompanyInfo(CompanyName:="DartPower Team", CompanyUrl:="www.DPT.net")>
Public Class VBSnapIn
    Implements IAppFunctionality
    Public Sub DoIt() Implements CommonSnappableTypes.IAppFunctionality.DoIt
        Console.WriteLine("You have just used the VB snap-in!")
    End Sub
End Class
