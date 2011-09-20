' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt


' ICreditCardInfo     3.0. Not in QBOE.   
Public Class QBCreditCardInfo

    Private _CreditCardNumber As String = String.Empty       'IQBStringType 25 Chars   Not in QBOE.   
    Private _ExpirationMonth As Integer = 0                  'IQBIntType     Not in QBOE.   
    Private _ExpirationYear As Integer = 0                   'IQBIntType     Not in QBOE.   
    Private _NameOnCard As String = String.Empty             'IQBStringType 41 Chars   Not in QBOE.   
    Private _CreditCardAddress As String = String.Empty      'IQBStringType 41 Chars   Not in QBOE.   
    Private _CreditCardPostalCode As String = String.Empty   'IQBStringType 41 Chars   Not in QBOE.   

    Public Property CreditCardAddress() As String
        Get
            Return _CreditCardAddress
        End Get
        Set(ByVal value As String)
            _CreditCardAddress = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property CreditCardNumber() As String
        Get
            Return _CreditCardNumber
        End Get
        Set(ByVal value As String)
            _CreditCardNumber = QuickBooksUtils.TrimToLength(value, 25)
        End Set
    End Property
    Public Property CreditCardPostalCode() As String
        Get
            Return _CreditCardPostalCode
        End Get
        Set(ByVal value As String)
            _CreditCardPostalCode = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property
    Public Property ExpirationMonth() As Integer
        Get
            Return _ExpirationMonth
        End Get
        Set(ByVal value As Integer)
            _ExpirationMonth = value
        End Set
    End Property
    Public Property ExpirationYear() As Integer
        Get
            Return _ExpirationYear
        End Get
        Set(ByVal value As Integer)
            _ExpirationYear = value
        End Set
    End Property
    Public Property NameOnCard() As String
        Get
            Return _NameOnCard
        End Get
        Set(ByVal value As String)
            _NameOnCard = QuickBooksUtils.TrimToLength(value, 41)
        End Set
    End Property

End Class
