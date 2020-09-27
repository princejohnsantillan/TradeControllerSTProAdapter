Imports SterlingLib

Public Class ErrorCodeHandler
    Public Shared Function CreateOrderReject(ErroCode As Integer, OrderStruct As structSTIOrder) As structSTIOrderReject
        Dim OrderRejectStruct As structSTIOrderReject

        OrderRejectStruct.bMoc = OrderStruct.bMoc
        OrderRejectStruct.bMoo = OrderStruct.bMoo
        OrderRejectStruct.bOpportunisticTrade = OrderStruct.bOpportunisticTrade
        OrderRejectStruct.bPossDupe = OrderStruct.bPossDupe
        OrderRejectStruct.bPriceCheck = OrderStruct.bPriceCheck
        OrderRejectStruct.bPriceQtyOverride = OrderStruct.bPriceQtyOverride
        OrderRejectStruct.bstrAccount = OrderStruct.bstrAccount
        OrderRejectStruct.bstrAccountType = OrderStruct.bstrAccountType
        OrderRejectStruct.bstrBatchId = OrderStruct.bstrBatchId
        OrderRejectStruct.bstrClOrderId = OrderStruct.bstrClOrderId
        OrderRejectStruct.bstrCoverUncover = OrderStruct.bstrCoverUncover
        OrderRejectStruct.bstrCurrency = OrderStruct.bstrCurrency
        OrderRejectStruct.bstrDestination = OrderStruct.bstrDestination
        OrderRejectStruct.bstrEndTime = OrderStruct.bstrEndTime
        OrderRejectStruct.bstrExecBroker = OrderStruct.bstrExecBroker
        OrderRejectStruct.bstrExecInst = OrderStruct.bstrExecInst
        OrderRejectStruct.bstrFutSettDate = OrderStruct.bstrFutSettDate
        OrderRejectStruct.bstrInstrument = OrderStruct.bstrInstrument
        OrderRejectStruct.bstrListingExchange = OrderStruct.bstrListingExchange
        OrderRejectStruct.bstrMaturity = OrderStruct.bstrMaturity
        OrderRejectStruct.bstrOpenClose = OrderStruct.bstrOpenClose
        OrderRejectStruct.bstrPairId = OrderStruct.bstrPairId
        OrderRejectStruct.bstrPriceType = OrderStruct.bstrStrPriceType
        OrderRejectStruct.bstrPutCall = OrderStruct.bstrPutCall
        OrderRejectStruct.bstrSettCurrency = OrderStruct.bstrSettCurrency
        OrderRejectStruct.bstrSettleDate = OrderStruct.bstrFutSettDate
        OrderRejectStruct.bstrSide = OrderStruct.bstrSide
        OrderRejectStruct.bstrSorPreference = OrderStruct.bstrSorPreference
        OrderRejectStruct.bstrStartTime = OrderStruct.bstrStartTime
        OrderRejectStruct.bstrStrategy = OrderStruct.bstrStrategy
        OrderRejectStruct.bstrSymbol = OrderStruct.bstrSymbol
        OrderRejectStruct.bstrTif = OrderStruct.bstrTif
        OrderRejectStruct.bstrUnderlying = OrderStruct.bstrUnderlying
        OrderRejectStruct.bstrUser = OrderStruct.bstrUser
        OrderRejectStruct.bTakeLiquidity = OrderStruct.bTakeLiquidity
        OrderRejectStruct.fAvgPriceLmt = OrderStruct.fAvgPriceLmt
        OrderRejectStruct.fCashComponent = OrderStruct.fCashComponent
        OrderRejectStruct.fDiscretion = OrderStruct.fDiscretion
        OrderRejectStruct.fExecPriceLmt = OrderStruct.fExecPriceLmt
        OrderRejectStruct.fLmtPrice = OrderStruct.fLmtPrice
        OrderRejectStruct.fMaxHighPctDeviation = OrderStruct.fMaxHighPctDeviation
        OrderRejectStruct.fMaxLowPctDeviation = OrderStruct.fMaxLowPctDeviation
        OrderRejectStruct.fPctPerSlice = OrderStruct.fPctPerSlice
        OrderRejectStruct.fPegDiff = OrderStruct.fPegDiff
        OrderRejectStruct.fPremium = OrderStruct.fPremium
        OrderRejectStruct.fRatio = OrderStruct.fRatio
        OrderRejectStruct.fReactPrice = OrderStruct.fReactPrice
        OrderRejectStruct.fStpPrice = OrderStruct.fStpPrice
        OrderRejectStruct.fStrikePrice = OrderStruct.fStrikePrice
        OrderRejectStruct.fTargetPctVolume = OrderStruct.fTargetPctVolume
        OrderRejectStruct.fTargetPrice = OrderStruct.fTargetPrice
        OrderRejectStruct.fTilt = OrderStruct.fTilt
        OrderRejectStruct.fTrailAmt = OrderStruct.fTrailAmt
        OrderRejectStruct.fTrailInc = OrderStruct.fTrailInc
        OrderRejectStruct.nAuction = OrderStruct.nAuction
        OrderRejectStruct.nDisplay = OrderStruct.nDisplay
        OrderRejectStruct.nDuration = OrderStruct.nDuration
        OrderRejectStruct.nExecAggression = OrderStruct.nExecAggression
        OrderRejectStruct.nMarketStructure = OrderStruct.nMarketStructure
        OrderRejectStruct.nMaxPctVolume = OrderStruct.nMaxPctVolume
        OrderRejectStruct.nMinPctVolume = OrderStruct.nMinPctVolume
        OrderRejectStruct.nMinQuantity = OrderStruct.nMinQuantity
        OrderRejectStruct.nOrderCompletion = OrderStruct.nOrderCompletion
        OrderRejectStruct.nPingInterval = OrderStruct.nPingInterval
        OrderRejectStruct.nPriceTolerance = OrderStruct.nPriceTolerance
        OrderRejectStruct.nPriceType = OrderStruct.nPriceType
        OrderRejectStruct.nQtyTolerancePct = OrderStruct.nQtyTolerancePct
        OrderRejectStruct.nQtyToleranceSize = OrderStruct.nQtyToleranceSize
        OrderRejectStruct.nQuantity = OrderStruct.nQuantity
        OrderRejectStruct.nRefreshInterval = OrderStruct.nRefreshInterval
        OrderRejectStruct.nRefreshQty = OrderStruct.nRefreshQty
        OrderRejectStruct.nSizeLow = OrderStruct.nSizeLow
        OrderRejectStruct.nSizeMax = OrderStruct.nSizeMax

        OrderRejectStruct.nRejectReason = ErroCode
        OrderRejectStruct.bstrText = SubmitOrderErrorMessage(ErroCode)

        Return OrderRejectStruct
    End Function

    Public Shared Function SubmitOrderErrorMessage(Code As Integer) As String
        Dim ErrorCodeMap As New Dictionary(Of Integer, String) From {
            {0, "No Errors"},
            {-1, "Invalid Account"},
            {-2, "Invalid Side"},
            {-3, "Invalid Qty"},
            {-4, "Invalid Symbol"},
            {-5, "Invalid PriceType"},
            {-6, "Invalid Tif"},
            {-7, "Invalid Destination"},
            {-8, "Exposure Limit Violation"},
            {-9, "NYSE+ Rules Violation"},
            {-10, "NYSE+ 30-Second Violation"},
            {-11, "Disable SelectNet Short Sales"},
            {-12, "Long Sale Position Rules Violation"},
            {-13, "Short Sale Position Rules Violation"},
            {-14, "GTC Orders Not Enabled"},
            {-15, "ActiveX API Not Enabled"},
            {-16, "Sterling Trader® Pro is Offline"},
            {-17, "Security Not Marked as Located"},
            {-18, "Order Size Violation"},
            {-19, "Position Limit Violation"},
            {-20, "Buying Power /Margin Control Violation"},
            {-21, "P/L Control Violation"},
            {-22, "Account Not Enabled for this Product"},
            {-23, "Trader Not Enabled for Futures"},
            {-24, "Minimum Balance Violation"},
            {-25, "Trader Not Enabled for odd lots"},
            {-26, "Order dollar limit exceeded"},
            {-27, "Trader Not Enabled for Optio"},
            {-28, "Soft share limit exceeded"},
            {-29, "Loss from max profit control violation (Title builds only)"},
            {-30, "Desk quantity enforcement violation"},
            {-31, "Account not enabled for Sell to Open (Options)"},
            {-32, "Account allowed to 'Close/Cxl' only"},
            {-33, "Trader not enabled for security locating"},
            {-34, "Order not able to be replaced (ReplaceOrder only)"},
            {-35, "Trader not enabled for 'Buy to Cover'"},
            {-36, "Invalid maturity date"},
            {-37, "Only one cancel and/or replace allowed per order per second"},
            {-38, "Account's maximum position value for this symbol exceeded"},
            {-39, "Symbol violates the account's min/max price settings"},
            {-40, "Quote Unavailable to calculate Order dollar limit"},
            {-41, "Quote Unavailable to calculate Maximum Position Cost"},
            {-42, "Quote Unavailable to calculate Buying Power"},
            {-43, "Quote Unavailable to calculate Margin Control"},
            {-44, "Floating BP Violation"},
            {-45, "Market order would remove liquidity (Front end setting)"},
            {-46, "Not enabled for Server Stop orders"},
            {-47, "Not enabled for Trail Stop orders"},
            {-48, "Order would exceed the Max Open orders per side on this symbol"},
            {-49, "Quote Unavailable or Compliance threshold exceeded or quote unavailable"},
            {-50, "Neither last nor Close price available for MKT order"},
            {-51, "Quote Unavailable or Does not meet min average daily volume"}
        }

        If ErrorCodeMap.ContainsKey(Code) Then
            Return ErrorCodeMap(Code)
        Else
            Return "Contact Sterling Support for Error Code (" + Code.ToString + ") Description."
        End If
    End Function

    Public Shared Function MaintainAccountErrorMessage(Code As Integer) As String
        Dim ErrorCodeMap As New Dictionary(Of Integer, String) From {
            {0, "No Errors"},
            {-1, "Pro is offline"},
            {-2, "Traders are not allowed to maintain accounts"},
            {-3, "Invalid account"},
            {-4, "Manager is not entitled to change field"}
        }

        If ErrorCodeMap.ContainsKey(Code) Then
            Return ErrorCodeMap(Code)
        Else
            Return "Contact Sterling Support for Error Code (" + Code.ToString + ") Description."
        End If
    End Function
End Class