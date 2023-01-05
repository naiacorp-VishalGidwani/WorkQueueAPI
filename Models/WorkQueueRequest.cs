namespace WorkQueueAPI.Model
{
    public class WorkQueueRequest
    {
        public DateTime ExpiryEndDate { get; set; }
        public HashSet<String>? Fields { get; set; }
    }
}
