' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class SendItemsPacket
    Public Url As String = String.Empty
    Public UserName As String = String.Empty
    Public Password As String = String.Empty
    Public service5 As BVC5WebServices.WebServices3
    Public Token5 As BVC5WebServices.AuthenticationToken
    Public Items As Collection(Of QBInventoryItem)
End Class

