' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class QBAddress

    Private _Addr1 As String = String.Empty 'IQBStringType 41 Chars 500 Chars
    Private _Addr2 As String = String.Empty  'IQBStringType 41 Chars 500 Chars    
    Private _Addr3 As String = String.Empty 'IQBStringType 41 Chars 500 Chars    
    Private _Addr4 As String = String.Empty    'IQBStringType 41 Chars 500 Chars
    Private _Addr5 As String = String.Empty    'IQBStringType 41 Chars 500 Chars
    Private _City As String = String.Empty     'IQBStringType 31 Chars 255 Chars    
    Private _State As String = String.Empty     'IQBStringType 21 Chars 255 Chars Not in QBCA.   
    Private _PostalCode As String = String.Empty 'IQBStringType 13 Chars 30 Chars    
    Private _Country As String = String.Empty ' 31 chars
    Private _County As String = String.Empty ' only for BV
    Private _Company As String = String.Empty ' only for BV
    Private _Note As String = String.Empty 'IQBStringType 41 chars

    Public Property Addr1() As String
        Get
            Return _Addr1
        End Get
        Set(ByVal value As String)
            _Addr1 = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property Addr2() As String
        Get
            Return _Addr2
        End Get
        Set(ByVal value As String)
            _Addr2 = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property Addr3() As String
        Get
            Return _Addr3
        End Get
        Set(ByVal value As String)
            _Addr3 = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property Addr4() As String
        Get
            Return _Addr4
        End Get
        Set(ByVal value As String)
            _Addr4 = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property Addr5() As String
        Get
            Return _Addr5
        End Get
        Set(ByVal value As String)
            _Addr5 = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property City() As String
        Get
            Return _City
        End Get
        Set(ByVal value As String)
            _City = QuickBooksUtils.TrimToLength(value, 31)
        End Set
    End Property
    Public Property Country() As String
        Get
            Return _Country
        End Get
        Set(ByVal value As String)
            _Country = QuickBooksUtils.TrimToLength(value, 31)
        End Set
    End Property
    Public Property County() As String ' Only for BV
        Get
            Return _County
        End Get
        Set(ByVal value As String)
            _County = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property PostalCode() As String
        Get
            Return _PostalCode
        End Get
        Set(ByVal value As String)
            _PostalCode = QuickBooksUtils.TrimToLength(value, 13)
        End Set
    End Property
    Public Property State() As String
        Get
            Return _State
        End Get
        Set(ByVal value As String)
            _State = QuickBooksUtils.TrimToLength(value, 21)
        End Set
    End Property
    Public Property Company() As String
        Get
            Return _Company
        End Get
        Set(ByVal value As String)
            _Company = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property Note() As String
        Get
            Return _Note
        End Get
        Set(ByVal value As String)
            _Note = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByRef a As BVC5WebServices.Address, ByVal qbversion As Integer)
        LoadBvc5Address(a, qbversion)
    End Sub

    Public Sub LoadBvc5Address(ByRef a As BVC5WebServices.Address, ByVal qbversion As Integer)
        If a IsNot Nothing Then
            Dim fullname As String = a.FirstName & " "
            If a.MiddleInitial <> String.Empty Then
                fullname += a.MiddleInitial & " "
            End If
            fullname += a.LastName

            ' Line 1 - Name
            Me.Addr1 = fullname

            ' Line 2 - Address
            Me.Addr2 = a.Line1
            If a.Line2.Trim.Length > 0 Then
                Me.Addr2 += " " & a.Line2
            End If

            '' Line 3 - City,State,Etc.
            'If a.CountryBvin <> QuickBooksUtils.UnitedStatesBVC5Guid Then
            '    Me.Addr3 = a.RegionName & " " & a.City & " " & a.PostalCode
            '    Me.Addr4 = a.CountryName
            'Else                
            '    Me.Addr3 = a.City & ", " & a.RegionName & " " & a.PostalCode
            'End If

            Me.City = a.City
            Me.State = a.RegionName
            Me.PostalCode = a.PostalCode
            Me.Country = a.CountryName
            Me.County = a.CountyName
        End If
    End Sub

    Public Sub LoadQBFCAddress(ByRef input As QBFC7Lib.IAddress, ByRef ver As QBVersionInfo)

        If Not input Is Nothing Then
            If Not input.Addr1 Is Nothing Then
                Me.Addr1 = input.Addr1.GetValue()
            End If
            If Not input.Addr2 Is Nothing Then
                Me.Addr2 = input.Addr2.GetValue()
            End If
            If Not input.Addr3 Is Nothing Then
                Me.Addr3 = input.Addr3.GetValue()
            End If
            If Not input.Addr4 Is Nothing Then
                Me.Addr4 = input.Addr4.GetValue()
            End If
            If Not input.Addr5 Is Nothing Then
                Me.Addr5 = input.Addr5.GetValue()
            End If
            If Not input.City Is Nothing Then
                Me.City = input.City.GetValue()
            End If
            If Not input.State Is Nothing Then
                Me.State = input.State.GetValue()
            End If
            If Not input.PostalCode Is Nothing Then
                Me.PostalCode = input.PostalCode.GetValue()
            End If
            If Not input.Country Is Nothing Then
                Me.Country = input.Country.GetValue()
            End If
            If Not input.Note Is Nothing Then
                Me.Note = input.Note.GetValue()
            End If
        End If
    End Sub

End Class
