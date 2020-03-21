namespace MyData.Core.Models
{
    public class XRoadLog
    {
        public long Id { get; set; }

        public string QueryId { get; set; }

        public string MemberClass { get; set; }

        public string MemberCode { get; set; }

        public string SubSystemCode { get; set; }

        public string Message { get; set; }

        public long Time { get; set; }

        public long? Attachment { get; set; }

        public string XRequestId { get; set; }

        public bool? Response { get; set; }

        public string Discriminator { get; set; }

        public override string ToString()
        {
            return $"Id={Id},QueryId={QueryId}," +
                   $"MemberClass={MemberClass},MemberCode={MemberCode},SubSystemCode={SubSystemCode}" +
                   $"Message={Message},Time={Time},Attachment={Attachment},XRequestId={XRequestId},Response={Response},Discriminator={Discriminator}";
        }
    }
}