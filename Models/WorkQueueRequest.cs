namespace WorkQueueAPI.Model
{
    public class WorkQueueRequest
    {
        public DateOnly FilingExpiryStartDate { get; set; }
        public DateOnly FilingExpiryEndDate { get; set; }
        public HashSet<String>? Fields { get; set; }
    }
}
