using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using WorkQueueAPI.Constants;
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

            //fixme: for the POC assuming timely time frame to be 1 month for all payer
            filter.Add("EncounterDate", new BsonDocument()
                    .Add("$gte", request.FilingExpiryStartDate.AddMonths(-1).ToDateTime(new TimeOnly(0, 0)))
                    .Add("$lte", request.FilingExpiryEndDate.AddMonths(-1).ToDateTime(new TimeOnly(0, 0)))
            );

            filter.Add("BillingStatus", new BsonDocument()
                    .Add("$nin", new BsonArray()
                            .Add("Billed")
                            .Add("Not Billable")
                    )
            );

            BsonDocument projection = new BsonDocument();

            foreach (String field in request.Fields)
            {
                if (!WorkQueue.TIMELY_FILING_KEY_MAP.ContainsKey(field))
                {
                    throw new InvalidOperationException("Field '" + field + "' is not supported.");
                }

                projection.Add(WorkQueue.TIMELY_FILING_KEY_MAP.GetValueOrDefault(field), 1.0);
            }

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
                       
                        foreach (String field in request.Fields)
                        {
                            // todo: handle different value types in better way
                            String fieldValue = document.GetElement(WorkQueue.TIMELY_FILING_KEY_MAP.GetValueOrDefault(field)).Value.ToString();
                            if (fieldValue.Equals("BsonNull"))
                            {
                                fieldValue = null;
                            }

                            singleRowData.Add(field, fieldValue);

                        }

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
