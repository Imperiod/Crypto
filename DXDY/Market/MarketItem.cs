using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXDY.Markets
{
    public class MarketItem
    {
        public string Market { get; init; }
        public MarketStatus Status { get; init; }
        public string BaseAsset { get; init; }
        public string QuoteAsset { get; init; }
        public decimal StepSize { get; init; }
        public decimal TickSize { get; init; }
        public decimal IndexPrice { get; init; }
        public decimal OraclePrice { get; init; }
        public decimal PriceChange24H { get; init; }
        public decimal NextFundingRate { get; init; }
        public DateTimeOffset NextFundingAt { get; init; }
        public decimal MinOrderSize { get; init; }
        public string Type { get; init; } = "PERPETUAL";
        public decimal InitialMarginFraction { get; init; }
        public decimal MaintenanceMarginFraction { get; init; }
        public decimal BaselinePositionSize { get; init; }
        public decimal IncrementalPositionSize { get; init; }
        public decimal IncrementalInitialMarginFraction { get; init; }
        public decimal Volume24H { get; init; }
        public decimal Trades24H { get; init; }
        public decimal OpenInterest { get; init; }
        public decimal MaxPositionSize { get; init; }
        public decimal AssetResolution { get; init; }
        public string SyntheticAssetId { get; init; }

        public MarketItem(string market, MarketStatus marketStatus, string baseAsset, string quoteAsset, decimal stepSize, decimal tickSize,
                          decimal indexPrice, decimal oraclePrice, decimal priceChange24H, decimal nextFundingRate, DateTimeOffset nextFuncingAt,
                          decimal minOrderSize, decimal initialMarginFraction, decimal maintenanceMarginFraction, decimal baselinePositionSize,
                          decimal incrementalPositionSize, decimal incrementalInitialMarginFraction, decimal volume24H, decimal trades24H, decimal openInterest,
                          decimal maxPositionSize, decimal assetResolution, string syntheticAssetId)
        {
            Market = market;
            Status = marketStatus;
            BaseAsset = baseAsset;
            QuoteAsset = quoteAsset;
            StepSize = stepSize;
            TickSize = tickSize;
            IndexPrice = indexPrice;
            OraclePrice = oraclePrice;
            PriceChange24H = priceChange24H;
            NextFundingRate = nextFundingRate;
            NextFundingAt = nextFuncingAt;
            MinOrderSize = minOrderSize;
            InitialMarginFraction = initialMarginFraction;
            MaintenanceMarginFraction = maintenanceMarginFraction;
            BaselinePositionSize = baselinePositionSize;
            IncrementalPositionSize = incrementalPositionSize;
            IncrementalInitialMarginFraction = incrementalInitialMarginFraction;
            Volume24H = volume24H;
            Trades24H = trades24H;
            OpenInterest = openInterest;
            MaxPositionSize = maxPositionSize;
            AssetResolution = assetResolution;
            SyntheticAssetId = syntheticAssetId;
        }

        public string GetCouple()
        {
            return $"{BaseAsset}-{QuoteAsset}";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"[{nameof(Market)}: {Market}]");
            sb.AppendLine($"{nameof(Status)}: {Status}");
            sb.AppendLine($"{nameof(BaseAsset)}: {BaseAsset}");
            sb.AppendLine($"{nameof(QuoteAsset)}: {QuoteAsset}");
            sb.AppendLine($"{nameof(StepSize)}: {StepSize}");
            sb.AppendLine($"{nameof(TickSize)}: {TickSize}");
            sb.AppendLine($"{nameof(IndexPrice)}: {IndexPrice}");
            sb.AppendLine($"{nameof(OraclePrice)}: {OraclePrice}");
            sb.AppendLine($"{nameof(PriceChange24H)}: {PriceChange24H}");
            sb.AppendLine($"{nameof(NextFundingRate)}: {NextFundingRate}");
            sb.AppendLine($"{nameof(NextFundingAt)}: {NextFundingAt}");
            sb.AppendLine($"{nameof(MinOrderSize)}: {MinOrderSize}");
            sb.AppendLine($"{nameof(Type)}: {Type}");
            sb.AppendLine($"{nameof(InitialMarginFraction)}: {InitialMarginFraction}");
            sb.AppendLine($"{nameof(MaintenanceMarginFraction)}: {MaintenanceMarginFraction}");
            sb.AppendLine($"{nameof(BaselinePositionSize)}: {BaselinePositionSize}");
            sb.AppendLine($"{nameof(IncrementalPositionSize)}: {IncrementalPositionSize}");
            sb.AppendLine($"{nameof(IncrementalInitialMarginFraction)}: {IncrementalInitialMarginFraction}");
            sb.AppendLine($"{nameof(Volume24H)}: {Volume24H}");
            sb.AppendLine($"{nameof(Trades24H)}: {Trades24H}");
            sb.AppendLine($"{nameof(OpenInterest)}: {OpenInterest}");
            sb.AppendLine($"{nameof(MaxPositionSize)}: {MaxPositionSize}");
            sb.AppendLine($"{nameof(AssetResolution)}: {AssetResolution}");
            sb.AppendLine($"{nameof(SyntheticAssetId)}: {SyntheticAssetId}");
            sb.AppendLine("---------------------------------------------------------------------------------------------------------------------");

            return sb.ToString();
        }
    }

    public enum MarketStatus
    {
        ONLINE,
        OFFLINE,
        POST_ONLY,
        CANCEL_ONLY
    }
}
