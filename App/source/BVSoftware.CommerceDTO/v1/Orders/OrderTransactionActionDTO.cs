using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Orders
{
    [DataContract]
    public enum OrderTransactionActionDTO
    {
        [EnumMember]
        Uknown = 0,
        [EnumMember]
        CreditCardInfo = 1, // A transaction that does nothing but store CC information on the server
        [EnumMember]
        CreditCardHold = 100, // Holds funds as an authorization against future capture
        [EnumMember]
        CreditCardCapture = 101, // Captured funds previously held/authorized
        [EnumMember]
        CreditCardCharge = 102, // A hold + capture or charge in a single step
        [EnumMember]
        CreditCardRefund = 103, // Refunds money to the CC
        [EnumMember]
        CreditCardVoid = 104, // Voids a previous transaction (usually only works before batch is settled)

        [EnumMember]
        CheckReceived = 201, // Receive Payment as a Check
        [EnumMember]
        CheckReturned = 202, // Send Payment as a Check

        [EnumMember]
        CashReceived = 301, // Receive a Payment as Cash
        [EnumMember]
        CashReturned = 302, // Return Cash

        [EnumMember]
        PurchaseOrderInfo = 401, // Purchase Order Number Info Stored
        [EnumMember]
        PurchaseOrderAccepted = 402, // Consider PO as Valid Payment        

        [EnumMember]
        CompanyAccountInfo = 450, // Company Account Number Saved
        [EnumMember]
        CompanyAccountAccepted = 451, // Company Account Number Accecpted as Payment

        [EnumMember]
        GiftCardInfo = 501, // Record Gift Card Information
        [EnumMember]
        GiftCardHold = 502, // Hold a Specific Amount on a Gift Card
        [EnumMember]
        GiftCardCapture = 503, // Capture a Held Amount on a Gift Card
        [EnumMember]
        GiftCardDecrease = 504, // Reduce Value of Gift Card
        [EnumMember]
        GiftCardIncrease = 505, // Increase Value of Gift Card
        [EnumMember]
        GiftCardActivate = 506, // Activate a Gift Card Number
        [EnumMember]
        GiftCardBalanceInquiry = 507, // Find the current balance of a gift card

        [EnumMember]
        RewardPointsInfo = 551, // Record Reward Points Information
        [EnumMember]
        RewardPointsHold = 552, // Hold a Specific Amount of Reward Points
        [EnumMember]
        RewardPointsCapture = 553, // Capture a Held Amount of Reward Points
        [EnumMember]
        RewardPointsDecrease = 554, // Reduce Points Available
        [EnumMember]
        RewardPointsIncrease = 555, // Increase Points Available
        [EnumMember]
        RewardPointsBalanceInquiry = 557, // Find the current balance of points available
        [EnumMember]
        RewardPointsUnHold = 558, // Hold a Specific Amount of Reward Points

        [EnumMember]
        PayPalHold = 601, // Funds Held at PayPal
        [EnumMember]
        PayPalCapture = 602, // Capture previously held Funds
        [EnumMember]
        PayPalCharge = 603, // Charge a PayPal Account
        [EnumMember]
        PayPalRefund = 604, // Send Money to a PayPal Account
        [EnumMember]
        PayPalVoid = 605, // Void a Pending Transaction
        [EnumMember]
        PayPalExpressCheckoutInfo = 606, // Customer Requests PayPal Express Checkout

        [EnumMember]
        OfflinePaymentRequest = 9999 // Records customer request to pay offline
    }
}
