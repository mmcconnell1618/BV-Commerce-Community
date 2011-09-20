' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt


Public Class QBInventoryItem
    Inherits QBItemInfo

    Private _Attribute As String = String.Empty
    Private _SKU As String = String.Empty ' ALU 20 chars
    Private _Desc1 As String = String.Empty ' 30 chars
    Private _Desc2 As String = String.Empty ' 30 chars
    Private _Cost As Decimal = 0D
    Private _MSRP As Decimal = 0D
    Private _Price1 As Decimal = 0D
    Private _OnHandQuantity As Decimal = 0D
    Private _UPC As String = String.Empty ' 13 chars max/min

    Public Property Attribute() As String
        Get
            Return _Attribute
        End Get
        Set(ByVal value As String)
            _Attribute = QuickBooksUtils.TrimToLength(value, 12)
        End Set
    End Property
    Public Property SKU() As String
        Get
            Return _SKU
        End Get
        Set(ByVal value As String)
            _SKU = QuickBooksUtils.TrimToLength(value, 20)
        End Set
    End Property
    Public Property Desc1() As String
        Get
            Return _Desc1
        End Get
        Set(ByVal value As String)
            _Desc1 = QuickBooksUtils.TrimToLength(value, 30)
        End Set
    End Property
    Public Property Desc2() As String
        Get
            Return _Desc2
        End Get
        Set(ByVal value As String)
            _Desc2 = QuickBooksUtils.TrimToLength(value, 30)
        End Set
    End Property
    Public Property Cost() As Decimal
        Get
            Return _Cost
        End Get
        Set(ByVal value As Decimal)
            _Cost = value
        End Set
    End Property
    Public Property MSRP() As Decimal
        Get
            Return _MSRP
        End Get
        Set(ByVal value As Decimal)
            _MSRP = value
        End Set
    End Property
    Public Property Price1() As Decimal
        Get
            Return _Price1
        End Get
        Set(ByVal value As Decimal)
            _Price1 = value
        End Set
    End Property
    Public Property OnHandQuantity() As Decimal
        Get
            Return _OnHandQuantity
        End Get
        Set(ByVal value As Decimal)
            _OnHandQuantity = value
        End Set
    End Property
    Public Property UPC() As String
        Get
            Return _UPC
        End Get
        Set(ByVal value As String)
            _UPC = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal oi As BVC5WebServices.LineItem)
        Me.Attribute = String.Empty
        Me.SKU = oi.ProductSku
        Me.Desc1 = oi.ProductName
        Me.Desc2 = Me.SKU
        Me.Cost = 0
        Me.MSRP = oi.BasePrice
        Me.Price1 = Math.Round((oi.LineTotal / oi.Quantity), 2)
        Me.OnHandQuantity = 0
        Me.UPC = String.Empty
        Me.ItemType = QBItemType.InventoryItem
    End Sub

    Public Sub New(ByVal item As QBFC7Lib.IItemInventoryRet)
        If item IsNot Nothing Then

            If item.Name IsNot Nothing Then
                Me.SKU = item.Name.GetValue
            End If
            If item.PurchaseCost IsNot Nothing Then
                Me.Cost = CDec(item.PurchaseCost.GetValue)
            End If
            If item.SalesDesc IsNot Nothing Then
                Me.Desc1 = item.SalesDesc.GetValue
            End If
            'If item. IsNot Nothing Then
            '    Me.Desc2 = item.Desc2.GetValue
            'End If
            If item.SalesPrice IsNot Nothing Then
                Me.MSRP = CDec(item.SalesPrice.GetValue)
            End If
            If item.SalesPrice IsNot Nothing Then
                Me.Price1 = CDec(item.SalesPrice.GetValue)
            End If
            If item.QuantityOnHand IsNot Nothing Then
                Me.OnHandQuantity = CDec(item.QuantityOnHand.GetValue())
            End If
            'If item.UPC IsNot Nothing Then
            '    Me.UPC = item.UPC.GetValue
            'End If
        End If
    End Sub

End Class

