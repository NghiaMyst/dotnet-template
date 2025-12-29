namespace dotnet_boilderplate.SharedKernel.Results
{
    public class ResponseList<T>
    {
        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public List<T> Data { get; set; }

        public ResponseList() 
        { 
            Data = [];
        }

        public ResponseList(int pageSize, int pageNumber, int total, List<T> data)
        {
            PageSize = pageSize;
            PageCount = pageNumber;
            TotalCount = total;
            Data = data;
        }
    }
}
