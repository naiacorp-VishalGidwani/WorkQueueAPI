using MongoDB.Driver;
using WorkQueueAPI.Model;

namespace WorkQueueAPI.Services
{
    public class WorkQueueService
    {
        public WorkQueueResponse GetWorkQueueResponse(IMongoDatabase db, WorkQueueRequest request) {
            Dictionary<string, string> singleRowData = new Dictionary<string, string>();
            singleRowData.Add("First Name", "Thomas");
            singleRowData.Add("Last Name", "Shelby");

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            data.Add(singleRowData);

            return new WorkQueueResponse()
            {
                Data = data
            };
        }
    }
}
