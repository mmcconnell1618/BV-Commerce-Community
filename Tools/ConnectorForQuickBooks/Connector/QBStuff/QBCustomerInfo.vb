' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt


Public Class QBCustomerInfo

    Private _BillingAddress As New QBAddress
    Private _CreditCardInfo As New QBCreditCardInfo
    Private _EditSequence As String = String.Empty
    Private _Email As String = String.Empty
    Private _FirstName As String = String.Empty
    Private _Lastname As String = String.Empty
    Private _ListID As String = String.Empty
    Private _MiddleInitial As String = String.Empty
    Private _PhoneNumber As String = String.Empty
    Private _ShippingAddress As New QBAddress
    Private _UserName As String = String.Empty
    Private _TaxExempt As Boolean = False

    Public Property BillingAddress() As QBAddress
        Get
            Return _BillingAddress
        End Get
        Set(ByVal value As QBAddress)
            _BillingAddress = value
        End Set
    End Property
    Public Property CreditCardInfo() As QBCreditCardInfo
        Get
            Return _CreditCardInfo
        End Get
        Set(ByVal value As QBCreditCardInfo)
            _CreditCardInfo = value
        End Set
    End Property
    Public Property EditSequence() As String
        Get
            Return _EditSequence
        End Get
        Set(ByVal value As String)
            _EditSequence = value
        End Set
    End Property
    Public Property Email() As String
        Get
            Return _Email
        End Get
        Set(ByVal value As String)
            _Email = value
        End Set
    End Property
    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property
    Public Property Lastname() As String
        Get
            Return _Lastname
        End Get
        Set(ByVal value As String)
            _Lastname = value
        End Set
    End Property
    Public Property ListID() As String
        Get
            Return _ListID
        End Get
        Set(ByVal value As String)
            _ListID = value
        End Set
    End Property
    Public Property MiddleInitial() As String
        Get
            Return _MiddleInitial
        End Get
        Set(ByVal value As String)
            _MiddleInitial = value
        End Set
    End Property
    Public Property PhoneNumber() As String
        Get
            Return _PhoneNumber
        End Get
        Set(ByVal value As String)
            _PhoneNumber = value
        End Set
    End Property
    Public Property ShippingAddress() As QBAddress
        Get
            Return _ShippingAddress
        End Get
        Set(ByVal value As QBAddress)
            _ShippingAddress = value
        End Set
    End Property
    Public Property Username() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property
    Public Property TaxExempt() As Boolean
        Get
            Return _TaxExempt
        End Get
        Set(ByVal value As Boolean)
            _TaxExempt = value
        End Set
    End Property


End Class
