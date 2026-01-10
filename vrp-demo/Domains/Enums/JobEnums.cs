namespace vrp_demo.Domains.Enums
{
    public enum TaskType
    {
        Service = 1,
        Shipment = 2
    }

    public enum ShipmentType
    {
        Pick = 0,
        Drop = 1
    }

    public enum TaskStatus
    {
        New = 0,
        Scheduled = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }

    public enum JobType
    {
        Service = 1,
        Shipment = 2,
    }

    public enum JobStatus
    {
        New = 0,
        Scheduled = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
}
