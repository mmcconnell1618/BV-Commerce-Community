' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class DuplicateForm

    Public Property Message() As String
        Get
            Return Me.lblMessage.Text
        End Get
        Set(ByVal Value As String)
            Me.lblMessage.Text = Value
        End Set
    End Property

End Class