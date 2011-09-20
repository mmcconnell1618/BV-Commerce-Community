
' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt


Public Class QBAccountInfo

    Private _ListId As String = String.Empty
    Private _Name As String = String.Empty
    Private _AccountType As QBAccountType = QBAccountType.Unknown

    Public Property ListId() As String
        Get
            Return _ListId
        End Get
        Set(ByVal value As String)
            _ListId = value
        End Set
    End Property
    Public Property AccountType() As QBAccountType
        Get
            Return _AccountType
        End Get
        Set(ByVal value As QBAccountType)
            _AccountType = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
        End Set
    End Property

End Class
