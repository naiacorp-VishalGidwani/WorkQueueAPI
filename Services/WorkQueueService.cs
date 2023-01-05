using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using WorkQueueAPI.Model;

namespace WorkQueueAPI.Services
{
    public class WorkQueueService
    {
        private MongoDBService _mongoDBService;
        public WorkQueueService(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<WorkQueueResponse> GetWorkQueueResponse(WorkQueueRequest request) {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            
            IMongoDatabase db = _mongoDBService.GetDB();
            IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("CurrentBillings");

            BsonDocument filter = new BsonDocument();

            filter.Add("EncounterDate", new BsonDocument()
                    .Add("$gt", new BsonDateTime(request.ExpiryEndDate))
            );

            BsonDocument projection = new BsonDocument();

            projection.Add("PatientFirstName", 1.0);
            projection.Add("PatientLastName", 1.0);

            BsonDocument sort = new BsonDocument();

            sort.Add("EncounterDate", 1.0);

            var options = new FindOptions<BsonDocument>()
            {
                Projection = projection,
                Sort = sort,
                Limit = 10
            };

            using (var cursor = await collection.FindAsync(filter, options))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Dictionary<string, string> singleRowData = new Dictionary<string, string>();
                        singleRowData.Add("First Name", document.GetElement("PatientFirstName").Value.ToString());
                        singleRowData.Add("Last Name", document.GetElement("PatientLastName").Value.ToString());

                        data.Add(singleRowData);
                    }
                }
            }

            return new WorkQueueResponse()
            {
                Data = data
            };
        }
    }
}
