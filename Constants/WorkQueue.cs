namespace WorkQueueAPI.Constants
{
    public class WorkQueue

    {

        public static Dictionary<String, String> TIMELY_FILING_KEY_MAP { get; } = new Dictionary<string, string>
        {
            ["FIRST NAME"] = "PatientFirstName",
            ["LAST NAME"] = "PatientLastName",
            ["DOB"] = "PatientDOB",
            ["DATE OF SERVICE"] = "EncounterDate",
            ["SUPERBILL BILLING STATUS"] = "BillingStatus",
        };
    }
}
