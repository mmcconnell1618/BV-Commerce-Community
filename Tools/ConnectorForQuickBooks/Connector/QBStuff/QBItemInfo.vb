' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class QBItemInfo

    Private _ListID As String = String.Empty
    Private _ItemType As QBItemType = QBItemType.Unknown

    Public Property ItemType() As QBItemType
        Get
            Return _ItemType
        End Get
        Set(ByVal value As QBItemType)
            _ItemType = value
        End Set
    End Property
    Public Property ListId() As String
        Get
            Return _ListID
        End Get
        Set(ByVal value As String)
            _ListID = value
        End Set
    End Property

End Class
