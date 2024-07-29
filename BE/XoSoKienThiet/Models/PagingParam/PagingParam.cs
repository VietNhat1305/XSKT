namespace XoSoKienThiet.Models.PagingParam
{
    public class PagingParam
    {
        public int Start { get; set; } = 1;
        public int Limit { get; set; } = 10;
        

        public string? SortBy { get; set; }
        
        public bool SortDesc { get; set; }
        public string? Content { get; set; }
        public int? Level { get; set; } = null;
        
        
        
        public int? IdDonViCha { get; set; } = null;

        public string? LoaiAnhId { get; set; } = null;


        public int Skip
        {
            get
            {
                return (Start > 0 ? Start - 1 : 0) * Limit;
            }
        }
        public int Year { get; set; }
    }
    
    
    
    
    
    public class ModelContentPagingParam : PagingParam
    {
       public string MenuId { get; set; }
        
       
        public DateTime?  StartDate { get; set; }
        
       public DateTime?  EndDate { get; set; }
       
    }
    
    

    public class PagingModel<T>
    {
        public long TotalRows { get; set; }
        
        public IEnumerable<T> Data { get; set; }
    }

    
}