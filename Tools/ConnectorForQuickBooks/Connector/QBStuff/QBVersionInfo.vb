' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class QBVersionInfo

    Private _CountryCode As String = String.Empty
    Private _MajorVersion As String = String.Empty
    Private _MinorVersion As String = String.Empty

    Public Property CountryCode() As String
        Get
            Return _CountryCode
        End Get
        Set(ByVal value As String)
            _CountryCode = value
        End Set
    End Property
    Public Property MajorVersion() As String
        Get
            Return _MajorVersion
        End Get
        Set(ByVal value As String)
            _MajorVersion = value
        End Set
    End Property
    Public Property MinorVersion() As String
        Get
            Return _MinorVersion
        End Get
        Set(ByVal value As String)
            _MinorVersion = value
        End Set
    End Property

End Class
