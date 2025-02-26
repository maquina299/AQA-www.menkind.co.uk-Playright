using Newtonsoft.Json;

namespace www.menkind.co.uk.Models
{
    public class CartSummaryResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("channel_id")]
        public int ChannelId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("line_item_count")]
        public int LineItemCount { get; set; }

        [JsonProperty("total_price")]
        public decimal TotalPrice { get; set; }

        [JsonProperty("total_quantity")]
        public int TotalQuantity { get; set; }

        [JsonProperty("item_quantities")]
        public Dictionary<string, int> ItemQuantities { get; set; }

        [JsonProperty("promotions")]
        public List<string> Promotions { get; set; }
    }
}
