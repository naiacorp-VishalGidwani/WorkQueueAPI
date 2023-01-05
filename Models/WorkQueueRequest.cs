namespace WorkQueueAPI.Model
{
    public class WorkQueueRequest
    {
        public DateOnly ExpiryEndDate { get; set; }
        public HashSet<String>? Fields { get; set; }
    }
}
